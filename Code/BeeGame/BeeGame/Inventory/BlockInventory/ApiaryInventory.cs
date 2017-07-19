using UnityEngine;
using UnityEngine.UI;
using BeeGame.Blocks;

namespace BeeGame.Inventory
{
    /// <summary>
    /// Inventory for Apiarys <see cref="Apiary"/>
    /// </summary>
    /// <remarks>
    /// The Apiary can exted thhe normal inventory as the basic functionality is the same (Items inside need to be saved, input/optut items, etc)
    /// </remarks>
    public class ApiaryInventory : ChestInventory
    {
        #region Data
        /// <summary>
        /// Are bees currently combineing
        /// </summary>
        private bool beesCombineing;

        /// <summary>
        /// How long does the current combineing bee have left
        /// </summary>
        public float combinationTime = 0;

        /// <summary>
        /// Sider to give a visual indication of <see cref="combinationTime"/>
        /// </summary>
        [System.NonSerialized]
        public Slider timerSlideer;
        #endregion

        #region Unity Methods
        /// <summary>
        /// Updates the block every frame
        /// </summary>
        private void Update()
        {
            //* Updates the base class as unity Update function does not run on parent classes
            UpdateChestInventory();

            //* if the apiary is not an item on the ground and bees are not currently combineing check is bees should be combineing
            if(items.itemsInInventory.Length > 0 && !beesCombineing)
                CheckforBees();

            //* if the currently combineing bees has finished combineing
            if (combinationTime < 0 && beesCombineing)
            {
                //* make the items that the bees should make and destroy the spent queen
                ((Apiary)myblock).MakeBees(items.itemsInInventory[0] as Items.Bee, ref items.itemsInInventory);
                beesCombineing = false;
                items.itemsInInventory[0] = null;

                //* save the channges to the inventory
                SaveInv();
            }
        }

        /// <summary>
        /// Updates the combination time because of this was frame rate dependand weird things would happen
        /// </summary>
        private void FixedUpdate()
        {
            //* if bees are combineing reduce the combination time
            if (beesCombineing)
                timerSlideer.value = combinationTime -= 0.1f;
        }
        #endregion

        #region Apiary Stuff
        /// <summary>
        /// Checks and combines bees in inventory slots 1 and 2 (<see cref="items.itemsInInventory"/> index 0 and 1)
        /// </summary>
        private void CheckforBees()
        {
            Items.Item posOneItem = items.itemsInInventory[0];
            Items.Item posTwoItem = items.itemsInInventory[1];

            //* the item is checkd if it is a bee and if it is then a new variable is made for convenience
            //* if it is a queen then just set the combination time and go
            if (posOneItem is Items.Bee b && b.beeType == Core.Enums.BeeType.QUEEN)
            {
                combinationTime = ((float)b.queenBee.queen.pLifespan + 1) * 2;
                beesCombineing = true;
                SaveInv();

                timerSlideer.maxValue = combinationTime;

                return;
            }

            //* of one bee is a princess and another is a drone in the correct slots combine them
            if(posOneItem is Items.Bee b1 && posTwoItem is Items.Bee b2 && b1.beeType == Core.Enums.BeeType.PRINCESS && b2.beeType == Core.Enums.BeeType.DRONE)
            {
                //* comvert the princess to a queen with the paired drone
                b1.ConvertToQueen(b2.normalBee);

                //* reduce number of drones in slot by 1 and check it is a valid stack number
                items.itemsInInventory[1].itemStackCount -= 1;
                slots[0].item = b1;

                if (items.itemsInInventory[1].itemStackCount <= 0)
                    items.itemsInInventory[1] = null;

                //* set the combination time
                combinationTime = ((float)b1.queenBee.queen.pLifespan + 1) * 2;
                beesCombineing = true;

                SaveInv();

                //* set the slider max to the combination time
                timerSlideer.maxValue = combinationTime;
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Sets the size and name of this <see cref="Inventory"/>
        /// </summary>
        /// <param name="invName"></param>
        public override void SetChestInventory(string invName = "Apiary")
        {
            base.SetChestInventory("Apiary" );
        }
        #endregion
    }
}