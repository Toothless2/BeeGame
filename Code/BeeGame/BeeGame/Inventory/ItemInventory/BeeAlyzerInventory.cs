using BeeGame.Core;
using BeeGame.Terrain;
using UnityEngine;
using System;
using static BeeGame.Core.THInput;
using BeeGame.Inventory.Player_Inventory;
using BeeGame.Items;

namespace BeeGame.Inventory
{
    /// <summary>
    /// Incentory for the chests
    /// </summary>
    public class BeeAlyzerInventory : Inventory
    {
        private Inventory playerInventory;
        internal BeeAlyzer myItem;

        protected new void Awake()
        {
        }

        protected new void Update()
        {
            if (GetButtonDown("Close Menu/Inventory"))
                ToggleInventory(playerInventory);
            UpdateBase();
            PutItemsInSlots();
        }
        
        public override void SaveInv()
        {
            return;
        }

        public void PutItemsInSlots(Inventory inv)
        {
            for (int i = slots.Length; i >=0; i--)
            {
                slots[i].slotIndex = i;
                slots[i].myInventory = this;
                slots[i].item = items.itemsInInventory[i] = inv.slots[i].item;
            }
        }

        public override void ToggleInventory(Inventory inv)
        {
            thisInventoryOpen = !thisInventoryOpen;

            isAnotherInventoryOpen = thisInventoryOpen;

            if (this.gameObject.activeInHierarchy && !thisInventoryOpen)
            {
                chestOpen = false;

                DropItemsFromInventory();

                myItem.OpenItemInvnetory(inv);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                ApplyPlayerItems();
                Destroy(this.gameObject);
            }
            else
            {
                chestOpen = true;

                SetInventorySize(slots.Length);

                playerInventory = inv;

                SetPlayerItems();

                PutItemsInSlots();
                //* hides and locks the cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public virtual void DropItemsFromInventory()
        {
            //* looks at every item in the crafting grid
            for (int i = 0; i < 3; i++)
            {
                if (items.itemsInInventory[i] != null)
                {
                    //* spawns it and removes it from the inventory if an items exists within
                    for (int j = 0; j < items.itemsInInventory[i].itemStackCount; j++)
                    {
                        MonoBehaviour.print(GameObject.FindGameObjectWithTag("Player").transform.position);
                        items.itemsInInventory[i].SpawnItem((THVector3)GameObject.FindGameObjectWithTag("Player").transform.position + new THVector3(0, 1, 0));
                    }
                    items.itemsInInventory[i] = null;
                }
            }
        }

        void SetPlayerItems()
        {
            for (int i = 0; i < playerInventory.items.itemsInInventory.Length; i++)
            {
                items.itemsInInventory[i + (slots.Length - 36)] = playerInventory.items.itemsInInventory[i];
            }
        }

        void ApplyPlayerItems()
        {
            for (int i = 0; i < playerInventory.items.itemsInInventory.Length; i++)
            {
                playerInventory.items.itemsInInventory[i] = items.itemsInInventory[i + (slots.Length - 36)];
            }

            playerInventory.SaveInv();
        }
    }
}