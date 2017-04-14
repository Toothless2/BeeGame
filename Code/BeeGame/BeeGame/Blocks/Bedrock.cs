using System;
using BeeGame.Core.Enums;
using BeeGame.Items;
using BeeGame.Core;

namespace BeeGame.Blocks
{
    /// <summary>
    /// Bedrock Block
    /// </summary>
    [Serializable]
    public class Bedrock : Block
    {
        #region Data
        /// <summary>
        /// Name of the item
        /// </summary>
        private string itemName = "Bedrock";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Bedrock() : base()
        {
            breakable = false;
        }
        #endregion

        #region Break Block
        /// <summary>
        /// The block cannot be broken so nothing is done
        /// </summary>
        /// <param name="pos">positon of the block</param>
        public override void BreakBlock(THVector3 pos)
        {
            return;
        }
        #endregion

        #region Mesh
        /// <summary>
        /// Position if te bedrock texture in the atlas
        /// </summary>
        /// <param name="direction"><see cref="Direction"/></param>
        /// <returns>Position in the texture atlas</returns>
        public override Tile TexturePosition(Direction direction)
        {
            return new Tile() { x = 0, y = 0};
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the ID of the item
        /// </summary>
        /// <returns>-1</returns>
        public override int GetHashCode()
        {
            return -1;
        }

        /// <summary>
        /// The item name and ID as a string
        /// </summary>
        /// <returns>A nicely formatted string</returns>
        public override string ToString()
        {
            return $"{itemName} ID: {GetItemID()}";
        }
        #endregion
    }
}
