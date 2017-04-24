using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Core.Enums;
using BeeGame.Items;

namespace BeeGame.Blocks
{
    [Serializable]
    public class Leaves : Block
    {

        public Leaves() : base("Leaves")
        {

        }

        public override Tile TexturePosition(Direction direction)
        {
            return new Tile() { x = 5, y = 9 };
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }

        #region Overrides
        /// <summary>
        /// Base ID of the block
        /// </summary>
        /// <returns>5</returns>
        public override int GetHashCode()
        {
            return 7;
        }

        /// <summary>
        /// Returns the name and ID of the block as a string
        /// </summary>
        /// <returns>A nicely formatted string</returns>
        public override string ToString()
        {
            return $"{itemName} \nID: {GetItemID()}";
        }
        #endregion
    }
}
