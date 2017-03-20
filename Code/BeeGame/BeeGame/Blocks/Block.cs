using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame.Items;

namespace BeeGame.Blocks
{
    [System.Serializable]
    public class Block
    {
        /// <summary>
        /// Item that this block is
        /// </summary>
        public Item item;
        /// <summary>
        /// Objects positon as a <see cref="THVector3"/>
        /// </summary>
        public THVector3 position;
        
        /// <summary>
        /// Items in this blocks inventory (may not be used)
        /// </summary>
        public Item[] inventoryItems;

        #region Hashcode
        /// <summary>
        /// Makes c# happy
        /// </summary>
        /// <returns>Object HashCode</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Operators
        /// <summary>
        /// Makes c# happy
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>true if object are the same</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        /// <summary>
        /// Returns true if block a and block b have the same position
        /// </summary>
        /// <param name="a">Block 1</param>
        /// <param name="b">Block 2</param>
        /// <returns>true if both blocks have the same position</returns>
        public static bool operator ==(Block a, Block b)
        {
            if (a.GetType() != b.GetType()) return false;
            if (a.position != b.position) return false;
            return true;
        }
        /// <summary>
        /// Retuns inverse of ==
        /// </summary>
        /// <param name="a">Block 1</param>
        /// <param name="b">Block 2</param>
        /// <returns>Inverse if ==</returns>
        public static bool operator !=(Block a, Block b)
        {
            return (a == b);
        }
        #endregion
    }
}