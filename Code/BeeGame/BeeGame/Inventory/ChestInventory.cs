using BeeGame.Core;
using BeeGame.Terrain;
using UnityEngine;
using static BeeGame.Core.THInput;

namespace BeeGame.Inventory
{
    public class ChestInventory : Inventory
    {
        public THVector3 inventoryPosition;

        public Inventory playerinventory;
        /// <summary>
        /// The inventory gameobject that will be displayed
        /// </summary>
        public GameObject inventory;

        public int inventorySize;

        public void SetChestInventory()
        {
            SetInventorySize(inventorySize);
            
            inventory.SetActive(false);

            inventoryName = $"Chest @ {(ChunkWorldPos)inventoryPosition}";
            Serialization.Serialization.DeSerializeInventory(this, inventoryName);
        }

        void Update()
        {
            if (playerinventory != null)
                UpdateBase();

            if (GetButtonDown("Player Inventory") && thisInventoryOpen)
                ToggleInventory(playerinventory);
        }

        void SetPlayerItems()
        {
            for (int i = 0; i < playerinventory.items.itemsInInventory.Length; i++)
            {
                items.itemsInInventory[i + (inventorySize - 36)] = playerinventory.items.itemsInInventory[i];
            }
        }

        void ApplyPlayerItems()
        {
            for (int i = 0; i < playerinventory.items.itemsInInventory.Length; i++)
            {
                playerinventory.items.itemsInInventory[i] = items.itemsInInventory[i + (inventorySize - 36)];
            }

            playerinventory.SaveInv();
        }

        public void ToggleInventory(Inventory inv)
        {
            playerinventory = inv;
            
            thisInventoryOpen = !thisInventoryOpen;

            isAnotherInventoryOpen = thisInventoryOpen;

            if(thisInventoryOpen)
                SetPlayerItems();

            inventory.SetActive(!inventory.activeInHierarchy);

            if (inventory.activeInHierarchy)
            {
                blockInventoryJustClosed = true;
                SetPlayerItems();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                ApplyPlayerItems();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}