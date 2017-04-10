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

        public void SetInventorySize(int inventorySize)
        {
            items = new ItemsInInventory(inventorySize);
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
