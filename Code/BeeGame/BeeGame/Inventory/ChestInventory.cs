using BeeGame.Core;
using UnityEngine;
using BeeGame.Blocks;
using BeeGame.Items;

namespace BeeGame.Inventory
{
    public class ChestInventory : InventoryBase
    {
        /// <summary>
        /// The chests inventory GameObject
        /// </summary>
        public GameObject inventory;
        /// <summary>
        /// The chests block interface where the items int he inventory will be stored
        /// </summary>
        protected BlockGameObjectInterface blockInterface;

        /// <summary>
        /// The players inventory
        /// </summary>
        private PlayerInventory playerinventory;

        /*
        /// <summary>
        /// number of slots in the chest inventory - the number of player inventory slots
        /// </summary>
        private uint slots;
        */
        /// <summary>
        /// Is the inventory Open
        /// </summary>
        bool inventoryOpen;

        /// <summary>
        /// When the chest is made the number of slots is calculated and the inventory is set to be inactive
        /// </summary>
        void Awake()
        {
            //slots = (uint)inventoryGUI.Length - 25;
            inventory.SetActive(false);
        }

        /// <summary>
        /// Sets the <see cref="blockInterface"/>
        /// Makes the <see cref="InventoryBase.slotandItem"/> array
        /// </summary>
        void Start()
        {
            blockInterface = GetComponent<BlockGameObjectInterface>();
            slotandItem = blockInterface.ApplyItemArray();

            if(slotandItem == null || slotandItem.Length < 1)
            {
                slotandItem = new Item[inventoryGUI.Length];
            }

            UpdateSlots();
            UpdateBase();
        }

        /// <summary>
        /// If the inventory os open checks if it should be closed, if it sould calls <see cref="CloseChest"/>. Also Updates the chests <see cref="InventoryBase"/> by calling <see cref="InventoryBase.UpdateBase"/>
        /// </summary>
        void Update()
        {
            UpdateChest();
        }

        public void UpdateChest()
        {
            if (inventoryOpen)
            {
                UpdateBase();
            }

            if (InputManager.GetButtonDown("Inventory"))
            {
                if (inventoryOpen)
                {
                    CloseChest();
                }
            }

            SaveChestItems();
        }
        
        /// <summary>
        /// \todo Currently this finction does nothing, must finish, should spawn items in the chests inventory when it is broken
        /// </summary>
        public void ChestBroken()
        {
        }

        /// <summary>
        /// Puts the chests current contence into the item[] in <see cref="BlockGameObjectInterface"/>
        /// </summary>
        void SaveChestItems()
        {
            blockInterface.UpdateItemArray(slotandItem);
        }

        /// <summary>
        /// Hides the chests invetoy and sets <see cref="inventoryOpen"/> to false
        /// </summary>
        public void CloseChest()
        {
            inventory.SetActive(false);
            playerinventory.heldObjectInventory.SetActive(true);
            UpdatePlayerInventory(playerinventory);
            
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            inventoryOpen = false;
        }

        /// <summary>
        /// Shows the chests inventory and displayes the given players inventory within it by calling <see cref="PutPlayerItemsInChest(PlayerInventory)"/> with the given <see cref="PlayerInventory"/>
        /// sets <see cref="inventoryOpen"/> to true
        /// </summary>
        /// <param name="_playerInventory"><see cref="PlayerInventory"/></param>
        public void OpenChest(PlayerInventory _playerInventory)
        {
            _playerInventory.heldObjectInventory.SetActive(false);
            PutPlayerItemsInChest(_playerInventory);

            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            inventory.SetActive(true);

            inventoryOpen = true;
        }

        /// <summary>
        /// When the chest is closed this updates the player inventory to any changes eg and item has been added or removed
        /// </summary>
        /// <param name="_playerinventoy"><see cref="PlayerInventory.UpdateSlots"/></param>
        void UpdatePlayerInventory(PlayerInventory _playerinventoy)
        {
            for(int i = 0; i < _playerinventoy.slotandItem.Length; i++)
            {
                _playerinventoy.slotandItem[i] = slotandItem[i];
            }

            _playerinventoy.UpdateSlots();
        }

        /// <summary>
        /// Puts the items in the players inventory in the correct slots so that they are displayed correctly in the chest
        /// </summary>
        /// <param name="_playerinventory"><see cref="PlayerInventory"/></param>
        void PutPlayerItemsInChest(PlayerInventory _playerinventory)
        {
            for(int i = 0; i < _playerinventory.slotandItem.Length; i++)
            {
                slotandItem[i] = _playerinventory.slotandItem[i];
            }

            playerinventory = _playerinventory;
            UpdateSlots();
        }
    }
}