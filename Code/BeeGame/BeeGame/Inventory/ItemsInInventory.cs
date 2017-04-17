using System;
using BeeGame.Items;

namespace BeeGame.Inventory
{
    /// <summary>
    /// Class that holds all of the items in the inventory. Can be serialized so inventory may be saved
    /// </summary>
    [Serializable]
    public class ItemsInInventory
    {
        /// <summary>
        /// All of the items in the inventory
        /// </summary>
        public Item[] itemsInInventory;

        /// <summary>
        /// Sets the size of the inventory
        /// </summary>
        /// <param name="numberOfInventorySlots"></param>
        public ItemsInInventory(int numberOfInventorySlots)
        {
            itemsInInventory = new Item[numberOfInventorySlots];
        }

        /// <summary>
        /// Add an <see cref="Item"/> to a specific index in the inventory
        /// </summary>
        /// <param name="index">Were to add the item</param>
        /// <param name="item">What <see cref="Item"/> to put in the inventory</param>
        public void AddItem(int index, Item item)
        {
            itemsInInventory[index] = item;
        }

        /// <summary>
        /// Adds a <see cref="Item"/> to the inventory
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>true if <paramref name="item"/> was added to the inventory</returns>
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
