using System;
using System.Collections.Generic;
using Assets.Scripts.Item;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Screens
{
    public class Inventory : MonoBehaviour
    {
        public Transform ItemsParent;
        public ItemDetails ItemDetails;

        private readonly List<GameObject> _items = new List<GameObject>();
        private Vector2 _scale;
        private Vector2 _positionOffset;
        private int _sortingOrder;

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
        }

        public void AddItem(ItemBase item)
        {
            var itemObject = Instantiate(item, Vector2.zero, Quaternion.identity, ItemsParent);
            // ReSharper disable once PossibleLossOfFraction
            itemObject.transform.localPosition = new Vector2(_items.Count % 6 * _positionOffset.x, _items.Count / 6 * _positionOffset.y);
            itemObject.transform.localScale = _scale;
            itemObject.GetComponent<SpriteRenderer>().sortingOrder = _sortingOrder;

            itemObject.GetComponent<Interaction>().OnClick += (sender, args) =>
            {
                ItemDetails.UpdateTarget(itemObject.gameObject);
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
