using System;
using System.Collections.Generic;
using Assets.Scripts.Item;
using Assets.Scripts.Scenes;
using Assets.Scripts.Tower;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public class Inventory : MonoBehaviour
    {
        public bool IsMoveable;
        public Transform ItemsParent;
        public ItemDetails ItemDetails;

        private GameState _gameState;
        private readonly List<GameObject> _items = new List<GameObject>();
        private Vector2 _scale;
        private Vector2 _positionOffset;
        private int _sortingOrder;

        private RectTransform _transform;
        private bool _isMoving;
        private Vector2 _initialPosition;
        private Vector2 _initialMousePosition;

        [UsedImplicitly]
        private void Start()
        {
            _gameState = GameState.GetGameState(gameObject);

            var parent = ItemsParent.parent;
            _sortingOrder = parent.GetComponent<Canvas>().sortingOrder;

            while (Math.Abs(parent.localScale.x - 1) < 0.00000001f)
            {
                parent = parent.parent;
            }
            _scale = new Vector2(1f / parent.localScale.x, 1f / parent.localScale.y);
            _positionOffset = new Vector2(_scale.x / 2f + 7.6f, -(_scale.y / 2f + 7.6f));

            _transform = GetComponent<RectTransform>();

            gameObject.SetActive(false);
        }

        [UsedImplicitly]
        private void Update()
        {
            if (_isMoving)
            {
                _transform.anchoredPosition = _initialPosition + Camera.main.ScreenToWorldPoint(Input.mousePosition) * _scale - _initialMousePosition;
            }
        }

        [UsedImplicitly]
        private void OnMouseDown()
        {
            if (!IsMoveable)
            {
                return;
            }

            _isMoving = true;
            _initialPosition = _transform.anchoredPosition;
            _initialMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) * _scale;
        }

        [UsedImplicitly]
        private void OnMouseUp()
        {
            if (!IsMoveable)
            {
                return;
            }
            _isMoving = false;
            PlayerPrefs.SetFloat(Settings.InventoryX, _transform.anchoredPosition.x);
            PlayerPrefs.SetFloat(Settings.InventoryY, _transform.anchoredPosition.y);
        }

        public void AddItem(ItemBase item)
        {
            var itemObject = Instantiate(item, Vector2.zero, Quaternion.identity, ItemsParent);
            itemObject.Effects = item.Effects;
            UpdateItemPosition(itemObject.gameObject, _items.Count, Vector2.zero);
            itemObject.transform.localScale = _scale;
            itemObject.GetComponent<SpriteRenderer>().sortingOrder = _sortingOrder;

            var interaction = itemObject.GetComponent<Interaction>();
            interaction.OnClick += HandleItemClick;
            interaction.OnEnter += HandleItemMouseEnter;
            interaction.OnExit += HandleItemMouseExit;
            interaction.OnMoveStart += (sender, args) =>
            {
                ItemDetails.UpdateTarget(itemObject.gameObject, false);
            };
            interaction.OnMove += (sender, delta) =>
            {
                var index = _items.IndexOf(itemObject.gameObject);
                UpdateItemPosition(_items[index], index, delta * _scale);
            };

            interaction.OnMoveEnd += (sender, position) =>
            {
                var index = _items.IndexOf(itemObject.gameObject);
                var tower = TowerForPosition(position);
                if (tower != null)
                {
                    _gameState.RemoveItem(index);
                    tower.AddItem(item);
                    Destroy(itemObject.gameObject);
                }
                else
                {
                    UpdateItemPosition(_items[index], index, Vector2.zero);
                }
                ItemDetails.UpdateTarget(null, false);
            };

            _items.Add(itemObject.gameObject);
        }

        public void RemoveItem(int index)
        {
            _items.RemoveAt(index);
            for (var i = index; i < _items.Count; i++)
            {
                UpdateItemPosition(_items[i], i, Vector2.zero);
            }
        }

        private void UpdateItemPosition(GameObject item, int index, Vector2 delta)
        {
            // ReSharper disable once PossibleLossOfFraction
            item.transform.localPosition = new Vector3(index % 6 * _positionOffset.x + delta.x, index / 6 * _positionOffset.y + delta.y, -1f);
        }

        private void HandleItemClick(object sender, GameObject item)
        {
            ItemDetails.UpdateTarget(item, false);
        }

        private void HandleItemMouseEnter(object sender, GameObject item)
        {
            ItemDetails.UpdateTarget(item);
        }

        private void HandleItemMouseExit(object sender, EventArgs e)
        {
            ItemDetails.UpdateTarget(null);
        }

        private TowerBase TowerForPosition(Vector2 position)
        {
            foreach (var hit in Physics2D.RaycastAll(position, Vector2.zero))
            {
                if (hit.collider is CircleCollider2D)
                {
                    continue;
                }

                if (hit.transform.GetComponent<Inventory>() == this)
                {
                    continue;
                }

                var parent = hit.transform.parent;
                var tower = parent.GetComponentInChildren<TowerBase>();
                if (tower == null)
                {
                    var towerDetails = parent.GetComponent<TowerDetails>();
                    if (towerDetails != null)
                    {
                        tower = towerDetails.Base;
                    }
                }

                if (tower != null)
                {
                    return tower;
                }
            }

            return null;
        }

        public void ResetItems()
        {
            _items.ForEach(Destroy);
            _items.Clear();
            ItemDetails.UpdateTarget(null);
        }
    }
}
