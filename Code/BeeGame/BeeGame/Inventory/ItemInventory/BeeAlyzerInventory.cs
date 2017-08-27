using BeeGame.Core;
using UnityEngine;
using UnityEngine.UI;
using static BeeGame.Core.THInput;
using BeeGame.Items;

namespace BeeGame.Inventory
{
    /// <summary>
    /// Incentory for the chests
    /// </summary>
    public class BeeAlyzerInventory : Inventory
    {
        #region Data
        /// <summary>
        /// Text box that shows the bee data
        /// </summary>
        public Text infoText;
        /// <summary>
        /// The players inventory
        /// </summary>
        private Inventory playerInventory;
        /// <summary>
        /// Item that this is attached to
        /// </summary>
        internal BeeAlyzer myItem;
        #endregion
        
        protected new void Update()
        {
            if (GetButtonDown("Close Menu/Inventory"))
                ToggleInventory(playerInventory);
            UpdateBase();
            PutItemsInSlots();

            CheckForBeeAndHoney();
        }
        
        /// <summary>
        /// This <see cref="Inventory"/> should not be saved
        /// </summary>
        public override void SaveInv()
        {
            return;
        }

        #region Open/Close Inventory
        /// <summary>
        /// Opens and closes this inventory
        /// </summary>
        /// <param name="inv"></param>
        public override void ToggleInventory(Inventory inv)
        {
            thisInventoryOpen = !thisInventoryOpen;

            isAnotherInventoryOpen = thisInventoryOpen;

            if (this.gameObject.activeInHierarchy && !thisInventoryOpen)
            {
                chestOpen = false;

                //* removes all of the items from thsi inventory
                DropItemsFromInventory();

                //* tells item that inventory has been closed
                myItem.OpenItemInvnetory();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                //* applies the chanegs to the players inventory
                ApplyPlayerItems();
                //* destroys this as it is not needed
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

        /// <summary>
        /// Removes the items from the inventory when it is closed, so they are not destroyed
        /// </summary>
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
        #endregion
        
        #region Inventory Function
        /// <summary>
        /// checks if a <see cref="Bee"/>s and <see cref="Honey"/>(currently disabled) are in the correct slots
        /// </summary>
        public void CheckForBeeAndHoney()
        {
            if(slots[0].item == null)
            {
                //if (slots[1].item.GetHashCode() == Honey.ID && slots[1].item.itemStackCount >= 1)
                if (slots[2].item is Bee b)
                {
                    b.canSeeBeeData = true;
                    items.itemsInInventory[0] = slots[2].item;
                    items.itemsInInventory[2] = null;
                    //items.itemsInInventory[1].itemStackCount -= 1;

                    infoText.text = ReturnData(b);
                }
                else
                {
                    infoText.text = "";
                }
            }
        }

        /// <summary>
        /// Returns the formatted bee data
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private string ReturnData(Bee b)
        {
            string returnString = "";

            if (b.beeType == Core.Enums.BeeType.QUEEN)
            {

            }

            returnString += $"Primary Species: {b.normalBee.pSpecies}\nSecondary Species: {b.normalBee.sSpecies}\nPrimary Fertility: {b.normalBee.pFertility}\nSecondary Fertility: {b.normalBee.sFertility}\nPrimary Lifespan: {b.normalBee.pLifespan}\nSecondary Lifespan: {b.normalBee.sLifespan}\nPrimary Production Speed: {b.normalBee.pProdSpeed}\nSecondary Production Speed: {b.normalBee.sProdSpeed}";

            return returnString;
        }
        #endregion


        #region Set inventory
        /// <summary>
        /// Applies the players inventory to this inventory
        /// </summary>
        void SetPlayerItems()
        {
            for (int i = 0; i < playerInventory.items.itemsInInventory.Length; i++)
            {
                items.itemsInInventory[i + (slots.Length - 36)] = playerInventory.items.itemsInInventory[i];
            }
        }

        /// <summary>
        /// Applies this inventory to the player once it is closed
        /// </summary>
        void ApplyPlayerItems()
        {
            for (int i = 0; i < playerInventory.items.itemsInInventory.Length; i++)
            {
                playerInventory.items.itemsInInventory[i] = items.itemsInInventory[i + (slots.Length - 36)];
            }

            playerInventory.SaveInv();
        }
        #endregion
    }
}