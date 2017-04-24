using System.Runtime.Serialization;

namespace BeeGame.Blocks
{
    /// <summary>
    /// Apiary Block
    /// </summary>
    public class Apiary : Block
    {
        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Apiary() : base("Apiary")
        {
        }
        #endregion

        #region Overrides
        /// <summary>
        /// ID of the item
        /// </summary>
        /// <returns>3</returns>
        public override int GetHashCode()
        {
            return 3;
        }

        /// <summary>
        /// The item name and ID as a string
        /// </summary>
        /// <returns>A nicely formatted string</returns>
        public override string ToString()
        {
            return $"{itemName} \nID: {GetItemID()}";
        }
        #endregion
    }
}
