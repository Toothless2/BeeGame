using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Core;
using BeeGame.Core.Enums;
using BeeGame.Items;

namespace BeeGame.Blocks
{
    [Serializable]
    public class Wood : Block
    {
        public Wood() : base("Wood")
        {

        }
        
        #region Item Stuff
        public override Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("Wood");
        }
        #endregion

        public override Tile TexturePosition(Direction direction)
        {
            return new Tile() { x = 7, y = 9 };
        }

        #region Overrides
        /// <summary>
        /// Base ID of the block
        /// </summary>
        /// <returns>5</returns>
        public override int GetHashCode()
        {
            return 6;
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
