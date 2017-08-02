using System;
using UnityEngine;
using BeeGame.Items;
using BeeGame.Core.Dictionaries;

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
        /// <summary>
        /// The sprite at the cursor
        /// </summary>
        private GameObject spriteAtCursor;
        /// <summary>
        /// The block class that this inventory is part of
        /// </summary>
        /// <remarks>
        /// currently only used for the <see cref="Blocks.Apiary"/> but could be used so that block inventorys are stord in the chunk and not in a seperate file
        /// </remarks>
        public Blocks.Block myblock;
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
            DrawItemAtCursor();
        }

        /// <summary>
        /// Draws the <see cref="floatingItem"/>s <see cref="Item.GetItemSprite()"/> at the mouse position
        /// </summary>
        private void DrawItemAtCursor()
        {
            if(floatingItem != null)
            {
                if (spriteAtCursor == null)
                {
                    spriteAtCursor = Instantiate(PrefabDictionary.GetPrefab("ItemIcon"));
                    spriteAtCursor.GetComponentInChildren<UnityEngine.UI.Image>().sprite = floatingItem.GetItemSprite();
                }
                //* will update a the sprite of in item is swapped between a slot and teh floating item if the previous item wasnt put into a slot first
                else if(spriteAtCursor != null)
                {
                    spriteAtCursor.GetComponentInChildren<UnityEngine.UI.Image>().sprite = floatingItem.GetItemSprite();
                }

                spriteAtCursor.transform.GetChild(0).position = Input.mousePosition;
            }
            else
            {
                Destroy(spriteAtCursor);
            }
        }
        #endregion

        #region Edit Inventory
        public virtual void ToggleInventory(Inventory inv)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Saves the inventory
        /// </summary>
        /// <remarks>
        /// Used when closeing a chest so the changes to the player inventory are saved
        /// </remarks>
        public virtual void SaveInv()
        {
            Serialization.Serialization.SerializeInventory(this, inventoryName);
        }

        public virtual void SetItemInventory(Item[] items)
        {

        }

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
        public virtual void AddItemToSlots(int slotIndex, Item item)
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
