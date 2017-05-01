using BeeGame.Core;
using BeeGame.Terrain;
using UnityEngine;
using static BeeGame.Core.THInput;

namespace BeeGame.Inventory
{
    /// <summary>
    /// Incentory for the chests
    /// </summary>
    public class ChestInventory : Inventory
    {
        #region Data
        /// <summary>
        /// Position in worldspace of the chest
        /// </summary>
        public THVector3 inventoryPosition;
        /// <summary>
        /// Refernce to the players <see cref="Inventory"/> so that it can be updated when chest is closed
        /// </summary>
        public Inventory playerinventory;
        /// <summary>
        /// The inventory gameobject that will be displayed
        /// </summary>
        public GameObject inventory;

        /// <summary>
        /// How many slots are in this <see cref="Inventory"/>
        /// </summary>
        public int inventorySize;
        #endregion

        #region Unity Methods
        /// <summary>
        /// Updates the slots and checks if the inventory should be closed
        /// </summary>
        void Update()
        {
            //* the chest should always have a player inventory when it does this but checks just in case
            if (playerinventory != null)
                UpdateBase();

            //* checks if the inventory should be closed
            if (GetButtonDown("Player Inventory") && thisInventoryOpen)
                ToggleInventory(playerinventory);
        }
        #endregion

        /// <summary>
        /// Sets the Size and name of this <see cref="Inventory"/>
        /// </summary>
        public void SetChestInventory()
        {
            SetInventorySize(inventorySize);
            //* sets the UI to not be seen as inventorys cannot start open
            inventory.SetActive(false);

            //* sets the name and postion if this inventory used during serialization and deserialization
            inventoryName = $"Chest @ {(ChunkWorldPos)inventoryPosition}";

            //* loads the inventory if it had had items put in it last time it existed
            Serialization.Serialization.DeSerializeInventory(this, inventoryName);
        }

        #region Player Inventory
        /// <summary>
        /// Puts the player items into the chest
        /// </summary>
        void SetPlayerItems()
        {
            for (int i = 0; i < playerinventory.items.itemsInInventory.Length; i++)
            {
                items.itemsInInventory[i + (inventorySize - 36)] = playerinventory.items.itemsInInventory[i];
            }
        }

        /// <summary>
        /// Applies the changes made to the <see cref="playerinventory"/> in <see cref="this"/>
        /// </summary>
        void ApplyPlayerItems()
        {
            for (int i = 0; i < playerinventory.items.itemsInInventory.Length; i++)
            {
                playerinventory.items.itemsInInventory[i] = items.itemsInInventory[i + (inventorySize - 36)];
            }

            playerinventory.SaveInv();
        }
        #endregion

        /// <summary>
        /// Opens and closes the inventory
        /// </summary>
        /// <param name="inv"></param>
        public void ToggleInventory(Inventory inv)
        {
            //* sets the player inventory
            playerinventory = inv;
            
            thisInventoryOpen = !thisInventoryOpen;

            isAnotherInventoryOpen = thisInventoryOpen;

            inventory.SetActive(!inventory.activeInHierarchy);

            if (inventory.activeInHierarchy)
            {
                //* stops the player invnetory from being opened immidiatly after this is closed
                blockInventoryJustClosed = true;
                SetPlayerItems();
                //* hides and locks the cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                //* puts the items into the chest
                //* shows and unlocks the cursor
                ApplyPlayerItems();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}