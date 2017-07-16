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