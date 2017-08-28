using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Core;
using BeeGame.Blocks;
using BeeGame.Items;
using BeeGame.Quest;

namespace BeeGame.Inventory.BlockInventory
{
    /// <summary>
    /// Invnetory for the <see cref="CraftingTable"/> <see cref="Block"/>
    /// </summary>
    public class CraftingTableInventory : ChestInventory
    {
        #region Data
        /// <summary>
        /// Makes the delegate
        /// </summary>
        public delegate void ItemRemovedFromResult();
        /// <summary>
        /// Holds the method for the delegate to call
        /// </summary>
        public ItemRemovedFromResult craftingResultRemoved;
        #endregion

        #region Unity Methods
        /// <summary>
        /// Sets the size of the inventory
        /// </summary>
        protected void Start()
        {
            SetChestInventory();
            craftingResultRemoved = CraftedItemRemoved;
        }


        /// <summary>
        /// Updates the base and checks crafting recipies
        /// </summary>
        protected void Update()
        {
            UpdateChestInventory();

            if (inventory.activeInHierarchy)
            {
                var shaped = CheckShapedRecipie();
                var shapless = CheckShapelessRecipie();

                if (shaped != null)
                {
                    items.itemsInInventory[9] = shaped;
                    return;
                }
                //* checks for shapless recipies second 
                else if(shapless != null)
                {
                    items.itemsInInventory[9] = shapless;
                    return;
                }

                items.itemsInInventory[9] = null;
            }
        }

        /// <summary>
        /// Ensureing no memory leaks occur due to the <see cref="delegate"/>
        /// </summary>
        protected void OnDestroy()
        {
            //* just ensures no memory leaks occur
            craftingResultRemoved -= CraftedItemRemoved;
        }
        #endregion

        #region Crafting Stuff
        /// <summary>
        /// Check in the recpie in the grid for a shaped crafting recipe
        /// </summary>
        public virtual Item CheckShapedRecipie()
        {
            var items = new Item[9];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = base.items.itemsInInventory[i];
            }

            //* if it is a recipe put the result into the crafting result slot
            return ((CraftingTable)myblock).ReturnShapedRecipieItem(items);
        }

        /// <summary>
        /// Check in the recipe grid for a shapless crafting recipe
        /// </summary>
        public virtual Item CheckShapelessRecipie()
        {
            var items = new Item[9];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = base.items.itemsInInventory[i];
            }
            
            return ((CraftingTable)myblock).ReturnShapelessRecipieItem(items);
        }

        /// <summary>
        /// Removes the items form the crafting grid one an item has been removed from the crafting result slot, Called via the <see cref="craftingResultRemoved"/> <see cref="delegate"/> from <see cref="InventorySlot.OnPointerClick(UnityEngine.EventSystems.PointerEventData)"/>
        /// </summary>
        public void CraftedItemRemoved()
        {
            if (items.itemsInInventory[9] != null)
            {
                QuestEvents.CallItemCraftedEvent(items.itemsInInventory[9].GetHashCode());
                for (int i = 0; i < 9; i++)
                {
                    if (items.itemsInInventory[i] != null)
                        items.itemsInInventory[i].itemStackCount -= 1;
                }
            }
        }
        #endregion

        #region Inventory Stuff
        /// <summary>
        /// Removes all <see cref="Item"/>s from the inventory when it is closed
        /// </summary>
        /// <remarks>
        /// Called by the output invenotry slot as it is a button
        /// </remarks>
        public virtual void DropItemsFromInventory()
        {
            //* looks at every item in the crafting grid
            for (int i = 0; i < 9; i++)
            {
                if (items.itemsInInventory[i] != null)
                {
                    //* spawns it and removes it from the inventory if an items exists within
                    for (int j = 0; j < items.itemsInInventory[i].itemStackCount; j++)
                    {
                        items.itemsInInventory[i].SpawnItem((THVector3)this.transform.position + new THVector3(0, 1, 0));
                    }
                    items.itemsInInventory[i] = null;
                }
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Opens/Closes the inventory
        /// </summary>
        /// <param name="inv">The inventory to toggle</param>
        public override void ToggleInventory(Inventory inv)
        {
            base.ToggleInventory(inv);

            //* if the inventory was closed drop the items within
            if (!inventory.activeInHierarchy)
               DropItemsFromInventory();
        }

        /// <summary>
        /// Set the size of the <see cref="Inventory"/>
        /// </summary>
        /// <param name="invName">Workbench</param>
        /// <remarks>
        /// overridden here so that no attemp is made to deserialize the inventory helping with performance
        /// </remarks>
        public override void SetChestInventory(string invName = "Workbench")
        {
            SetInventorySize(inventorySize);
            //* sets the UI to not be seen as inventorys cannot start open
            inventory.SetActive(false);
        }

        /// <summary>
        /// Adds an item to a <see cref="InventorySlot"/>
        /// </summary>
        /// <param name="slotIndex"><see cref="InventorySlot.slotIndex"/> to add the items to</param>
        /// <param name="item"><see cref="Item"/> to add</param>
        /// <remarks>
        /// Overriden so serialization does not occur
        /// </remarks>
        public override void AddItemToSlots(int slotIndex, Item item)
        {
            items.AddItem(slotIndex, item);
        }

        /// <summary>
        /// Oerriden so the inventory is not saved in any way
        /// </summary>
        public override void SaveInv()
        {
            //* does not need to be saved so overrided to do nothing
        }
        #endregion
    }
}
