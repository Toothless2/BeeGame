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

        public void SetChestInventory()
        {
            //if (InventorySet())
                SetInventorySize(36);

            //inventory = Instantiate(PrefabDictionary.GetPrefab("PlayerInventory"), transform);
            inventory.SetActive(false);

            inventoryName = $"Chest @ {(ChunkWorldPos)inventoryPosition}";
            //Serialization.Serialization.DeSerializeInventory(this, inventoryName);
        }

        void Update()
        {
            if (playerinventory != null)
            {
                SetPlayerItems();
                UpdateBase();
            }
        }

        void SetPlayerItems()
        {
            for (int i = 0; i < playerinventory.items.itemsInInventory.Length - 9; i++)
            {
                items.itemsInInventory[i] = playerinventory.items.itemsInInventory[i];
            }
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
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
}
