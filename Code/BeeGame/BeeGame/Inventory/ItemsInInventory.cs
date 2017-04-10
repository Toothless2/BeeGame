using System;
using BeeGame.Items;

namespace BeeGame.Inventory
{
    [Serializable]
    public class ItemsInInventory
    {
        public Item[] itemsInInventory;

        public ItemsInInventory(int numberOfInventorySlots)
        {
            itemsInInventory = new Item[numberOfInventorySlots];
        }

        public bool AddItem(Item item)
        {
            for (int i = 0; i < itemsInInventory.Length; i++)
            {
                if (itemsInInventory[i] == null)
                {
                    itemsInInventory[i] = item;
                    return true;
                }
                if (itemsInInventory[i] == item && itemsInInventory[i].itemStackCount + 1 <= itemsInInventory[i].maxStackCount)
                {
                    itemsInInventory[i].itemStackCount++;
                    return true;
                }
            }

            return false;
        }
    }
}
