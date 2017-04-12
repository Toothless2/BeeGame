using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Items;

namespace BeeGame.Inventory
{
    public class Inventory : MonoBehaviour
    {
        private ItemsInInventory items;
        public InventorySlot[] slots;
        internal Item floatingItem;

        public void UpdateBase()
        {
            PutItemsInSlots();
        }

        void PutItemsInSlots()
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].slotIndex = i;
                slots[i].myInventory = this;
                slots[i].item = items.itemsInInventory[i];
            }
        }

        public void RemoveItemFromSlot(int slotIndex)
        {

        }

        public void AddItemToSlots(int slotIndex, Item item)
        {
            items.AddItem(slotIndex, item);
        }

        public void SetInventorySize(int inventorySize)
        {
            items = new ItemsInInventory(slots.Length);
        }

        public bool InventorySet()
        {
            if (items == null)
                return true;

            return false;
        }

        public bool AddItemToInventory(Item item)
        {
            return items.AddItem(item);
        }
    }
}
