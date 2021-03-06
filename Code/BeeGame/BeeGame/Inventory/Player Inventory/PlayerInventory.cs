﻿using UnityEngine;
using BeeGame.Items;
using BeeGame.Core;
using BeeGame.Quest;

namespace BeeGame.Inventory.Player_Inventory
{
    /// <summary>
    /// Controlls the player inventory
    /// </summary>
    public class PlayerInventory : Inventory
    {
        #region Data
        /// <summary>
        /// Object that the inventory is
        /// </summary>
        public GameObject playerInventory;
        #endregion

        #region Init
        /// <summary>
        /// Sets all requred params for the inventory and loads ant saved versions of it
        /// </summary>
        protected void Awake()
        {
            SetPlayerInventory();
            inventoryName = "PlayerInventory";
            Serialization.Serialization.DeSerializeInventory(this, inventoryName);
        }

        /// <summary>
        /// Set the size of the player inventory
        /// </summary>
        public virtual void SetPlayerInventory(int size = 36)
        {
            if (!InventorySet())
                SetInventorySize(size);
        }
        #endregion

        /// <summary>
        /// Goves the inventory update ticks
        /// </summary>
        protected void Update()
        {
            UpdateBase();

            //* checks if the inventory should be opened/closed
            if ((thisInventoryOpen || !playerInventory.activeInHierarchy) && !THInput.chestOpen && THInput.GetButtonDown("Player Inventory"))
            {
                if (THInput.blockInventoryJustClosed)
                {
                    THInput.blockInventoryJustClosed = false;
                    return;
                }
                else
                {
                    OpenPlayerInventory();
                }
            }

            //* dont pickup items if the inventory is open
            if (THInput.isAnotherInventoryOpen)
                return;

            //* checks if somethig should be picked up and put into the inventory
            RaycastHit[] hit = Physics.SphereCastAll(transform.position, 1f, transform.forward);

            for (int i = hit.Length - 1; i >= 0; i--)
            {
                if (hit[i].collider.GetComponent<ItemGameObject>())
                    PickupItem(hit[i].collider.GetComponent<ItemGameObject>());
            }

        }

        #region Hotbar
        /// <summary>
        /// Updates the currrently selected hotbar slot
        /// </summary>
        /// <param name="index">Slot that is selected</param>
        public void SelectedSlot(int index)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].selectedSlot = false;
            }

            slots[index].selectedSlot = true;
        }

        /// <summary>
        /// Gets an item from the hotbar (9 <see cref="InventorySlot"/>s at the bottom of the screen)
        /// </summary>
        /// <param name="slotIndex">Index to get <see cref="Item"/> from</param>
        /// <param name="outItem"><see cref="Item"/> in the slot</param>
        /// <returns>true if <paramref name="outItem"/> is placeable, false if <paramref name="outItem"/> is null or not placeable</returns>
        public bool GetItemFromHotBar(int slotIndex, out Item outItem)
        {
            //* get the item
            outItem = GetAllItems().itemsInInventory[slotIndex];

            if (outItem == null)
                return false;

            //* if the item is placebale and is not null remove 1 from the inventory as it is assumed it is about to be placed in the world
            if(outItem.placeable)
                RemoveItemFromInventory(slotIndex);
            
            return outItem.placeable;
        }
        #endregion

        #region Interact With Inventory
        /// <summary>
        /// Show/Hide the player inventory
        /// </summary>
        internal void OpenPlayerInventory()
        {
            if (floatingItem != null)
                return;
            thisInventoryOpen = !thisInventoryOpen;
            playerInventory.SetActive(!playerInventory.activeInHierarchy);
            THInput.isAnotherInventoryOpen = !THInput.isAnotherInventoryOpen;

            //* hides/shows the mouse depending on if te inventory is open or not
            if (playerInventory.activeInHierarchy)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        
        /// <summary>
        /// Removes 1 item from the given inventory index
        /// </summary>
        /// <param name="index"></param>
        public void RemoveItemFromInventory(int index)
        {
            //* if the item is already null nothign needs to be removed
            if (GetAllItems().itemsInInventory[index] != null)
            {
                //* remove 1 item and if that was the last in the stack remove the item from the inventory
                GetAllItems().itemsInInventory[index].itemStackCount -= 1;

                if (GetAllItems().itemsInInventory[index].itemStackCount <= 0)
                    GetAllItems().itemsInInventory[index] = null;

                Serialization.Serialization.SerializeInventory(this, inventoryName);
            }
        }
        
        /// <summary>
        /// Pickup an item and put it into the <see cref="Inventory"/>
        /// </summary>
        /// <param name="item">Item to try to put into the inventory</param>
        void PickupItem(ItemGameObject item)
        {
            item.item.itemStackCount = 1;

            //* if the item can be added to the inventory do that
            if (AddItemToInventory(item.item))
            {
                QuestEvents.CallItemPickupEvent(item.item.GetHashCode());
                //* if the item was added destroy its gameobject and save the inventory
                Destroy(item.gameObject);
                Serialization.Serialization.SerializeInventory(this, inventoryName);
            }
        }
        #endregion
    }
}
