using System.Collections.Generic;

namespace Assets.Scripts.Item
{
    public interface IHasItems
    {
        public bool IsInventoryFull { get; }

        public void AddItem(ItemBase item);

        public void RemoveItem(int index);
    }
}
