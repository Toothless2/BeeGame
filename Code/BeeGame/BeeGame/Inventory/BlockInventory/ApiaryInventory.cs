using System;
using BeeGame.Core;
using BeeGame.Terrain;
using UnityEngine;
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
        private void Update()
        {
            UpdateChestInventory();

            if(items.itemsInInventory.Length > 0)
                CheckforBees();
        }

        private void CheckforBees()
        {
            Items.Item posOneItem = items.itemsInInventory[0];
            Items.Item posTwoItem = items.itemsInInventory[1];

            if(posOneItem is Items.Bee b && b.beeType == Core.Enums.BeeType.QUEEN)
            {
                MonoBehaviour.print($"Bee is a Queen");
            }

            if(posOneItem is Items.Bee b1 && posTwoItem is Items.Bee b2 && b1.beeType == Core.Enums.BeeType.PRINCESS && b2.beeType == Core.Enums.BeeType.DRONE)
            {
                b1.ConvertToQueen(b2.normalBee);
                items.itemsInInventory[1].itemStackCount -= 1;
                slots[0].item = b1;
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