using UnityEngine;
using BeeGame.Items;
using BeeGame.Core;

namespace BeeGame.Inventory
{
    /// <summary>
    /// Base class for all inventorys in the game
    /// </summary>
    public class Inventory : MonoBehaviour
    {
        #region Data
        /// <summary>
        /// Items in the invemtory
        /// </summary>
        public ItemsInInventory items;
        /// <summary>
        /// Slots in the inventory
        /// </summary>
        public InventorySlot[] slots;
        /// <summary>
        /// <see cref="Item"/> that is currenty being moved
        /// </summary>
        internal Item floatingItem;
        /// <summary>
        /// Name of this inventory
        /// </summary>
        public string inventoryName = "";
        /// <summary>
        /// is this inventory open?
        /// </summary>
        protected bool thisInventoryOpen = false;
        #endregion

        #region Init
        /// <summary>
        /// Is the inventory set?
        /// </summary>
        /// <returns>true if <see cref="items"/> == null</returns>
        public bool InventorySet()
        {
            if (items == null)
                return true;

            return false;
        }

        /// <summary>
        /// Sets the inventory soze to the number of slots in the invnetory
        /// </summary>
        /// <param name="inventorySize"></param>
        public void SetInventorySize(int inventorySize)
        {
            items = new ItemsInInventory(slots.Length);
        }

        /// <summary>
        /// Sets the <see cref="items"/> to the given <see cref="ItemsInInventory"/>
        /// </summary>
        /// <param name="items">Items to set this inventory to</param>
        ///<remarks>
        /// Used during deserialization to restor the inventory
        ///</remarks>
        public void SetAllItems(ItemsInInventory items)
        {
            this.items = items;
        }
        #endregion

        #region Update
        /// <summary>
        /// Things in the inventory that should be updated
        /// </summary>
        public void UpdateBase()
        {
            PutItemsInSlots();
        }
        #endregion

        #region Edit Inventory
        /// <summary>
        /// Sets an <see cref="Item"/> in the <see cref="ItemsInInventory.itemsInInventory"/> array to a <see cref="InventorySlot.item"/>
        /// </summary>
        void PutItemsInSlots()
        {
            //* goes through all of the items in the array setting then all to a slot
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].slotIndex = i;
                slots[i].myInventory = this;
                slots[i].item = items.itemsInInventory[i];
            }
        }
        
        /// <summary>
        /// Gets all of the items in the invntory
        /// </summary>
        /// <returns>All of the items in the inventory as <see cref="ItemsInInventory"/></returns>
        public ItemsInInventory GetAllItems()
        {
            return items;
        }

        /// <summary>
        /// Adds the given <paramref name="item"/> to the inventory in the given <paramref name="slotIndex"/>
        /// </summary>
        /// <param name="slotIndex">Slot to add item to</param>
        /// <param name="item">Item to add</param>
        public void AddItemToSlots(int slotIndex, Item item)
        {
            items.AddItem(slotIndex, item);
            //* saves the inventory changes
            Serialization.Serialization.SerializeInventory(this, inventoryName);
        }
                
        /// <summary>
        /// Add an item to the inventory
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>true if item wasa added</returns>
        public bool AddItemToInventory(Item item)
        {
            return items.AddItem(item);
        }
        #endregion
    }
}
