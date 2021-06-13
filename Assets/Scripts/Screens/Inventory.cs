using System;
using System.Collections.Generic;
using Assets.Scripts.Item;
using Assets.Scripts.Scenes;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public class Inventory : MonoBehaviour
    {
        public bool IsMoveable;
        public Transform ItemsParent;
        public ItemDetails ItemDetails;

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
            var parent = ItemsParent.parent;
            _sortingOrder = parent.GetComponent<Canvas>().sortingOrder;

            while (Math.Abs(parent.localScale.x - 1) < 0.00000001f)
            {
                parent = parent.parent;
            }
            _scale = new Vector2(1f / parent.localScale.x, 1f / parent.localScale.y);
            _positionOffset = new Vector2(_scale.x / 2f + 7.6f, -(_scale.y / 2f + 7.6f));

            _transform = GetComponent<RectTransform>();
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
            // ReSharper disable once PossibleLossOfFraction
            var initialPosition = new Vector2(_items.Count % 6 * _positionOffset.x, _items.Count / 6 * _positionOffset.y);
            itemObject.transform.localPosition = initialPosition;
            itemObject.transform.localScale = _scale;
            itemObject.GetComponent<SpriteRenderer>().sortingOrder = _sortingOrder;

            var interaction = itemObject.GetComponent<Interaction>();
            interaction.OnClick += (sender, args) =>
            {
                ItemDetails.UpdateTarget(itemObject.gameObject);
            };
            interaction.OnMove += (sender, delta) =>
            {
                itemObject.transform.localPosition = initialPosition + delta * _scale;
            };
            interaction.OnMoveEnd += (sender, args) =>
            {
                itemObject.transform.localPosition = initialPosition;
            };

            _items.Add(itemObject.gameObject);
        }

        public void ResetItems()
        {
            _items.ForEach(Destroy);
            _items.Clear();
            ItemDetails.UpdateTarget(null);
        }
    }
}
