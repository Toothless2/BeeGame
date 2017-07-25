using System;
using BeeGame.Core.Enums;
using BeeGame.Items;
using BeeGame.Core.Dictionaries;
using UnityEngine;

namespace BeeGame.Blocks
{
    /// <summary>
    /// Planks Block
    /// </summary>
    [Serializable]
    public class Planks : Block
    {
        public new static int ID => 7;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Planks() : base("Planks"){}
        #endregion

        #region Item Stuff
        public override Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("Planks");
        }
        #endregion

        #region Mesh
        /// <summary>
        /// Position of the planks texture in the atlas
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public override Tile TexturePosition(Direction direction)
        {
            return new Tile { x = 2, y = 9 };
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Base ID of the block
        /// </summary>
        /// <returns>5</returns>
        public override int GetHashCode()
        {
            return ID;
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
