using System;
using BeeGame.Core;
using BeeGame.Terrain;
using UnityEngine;
using BeeGame.Blocks;
using static BeeGame.Core.THInput;

namespace BeeGame.Inventory
{
    /// <summary>
    /// Inventory for Apiarys <see cref="BeeGame.Blocks.Apiary"/>
    /// </summary>
    /// <remarks>
    /// The Apiary can exted thhe normal inventory as the basic functionality is the same (Items inside need to be saved, input/optut items, etc)
    /// </remarks>
    public class ApiaryInventory : ChestInventory
    {
        private bool beesCombineing;

        public float combinationTime = 0;

        private void Update()
        {
            UpdateChestInventory();

            if(items.itemsInInventory.Length > 0 && !beesCombineing)
                CheckforBees();

            if (combinationTime < 0 && beesCombineing)
            {
                ((Apiary)myblock).MakeBees(items.itemsInInventory[0] as Items.Bee, ref items.itemsInInventory);
                beesCombineing = false;
                items.itemsInInventory[0] = null;

                SaveInv();
            }
        }

        /// <summary>
        /// Updates the combination time because of this was frame rate dependand weird things would happen
        /// </summary>
        private void FixedUpdate()
        {
            if (beesCombineing && combinationTime > 0)
                combinationTime -= 0.1f;
        }

        private void CheckforBees()
        {
            Items.Item posOneItem = items.itemsInInventory[0];
            Items.Item posTwoItem = items.itemsInInventory[1];

            //* the item is checkd if it is aa bee and if it is then a new variable is made for convenience
            if(posOneItem is Items.Bee b && b.beeType == Core.Enums.BeeType.QUEEN)
            {
                combinationTime = ((float)b.queenBee.queen.pLifespan + 1) * 2;
                beesCombineing = true;
                SaveInv();
                MonoBehaviour.print($"Bee is a Queen");
            }

            if(posOneItem is Items.Bee b1 && posTwoItem is Items.Bee b2 && b1.beeType == Core.Enums.BeeType.PRINCESS && b2.beeType == Core.Enums.BeeType.DRONE)
            {
                b1.ConvertToQueen(b2.normalBee);
                items.itemsInInventory[1].itemStackCount -= 1;
                slots[0].item = b1;

                if (items.itemsInInventory[1].itemStackCount <= 0)
                    items.itemsInInventory[1] = null;

                combinationTime = ((float)b1.queenBee.queen.pLifespan + 1) * 2;
                beesCombineing = true;
                SaveInv();
                MonoBehaviour.print($"Converted to Queen");
            }
        }

        /// <summary>
        /// Sets the size and name of this <see cref="Inventory"/>
        /// </summary>
        /// <param name="invName"></param>
        public override void SetChestInventory(string invName = "Apiary")
        {
            base.SetChestInventory("Apiary" );
        }


    }
}