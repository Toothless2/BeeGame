using System.Runtime.Serialization;

namespace BeeGame.Blocks
{
    /// <summary>
    /// Apiary Block
    /// </summary>
    public class Apiary : Block
    {
        #region Data
        /// <summary>
        /// Name of the item
        /// </summary>
        private string itemName = "Apiary";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Apiary() : base() { }
        #endregion

        public Apiary(SerializationInfo info, StreamingContext context)
        {
            //*use info.getvalue("valuename", typeof(valueType))
            UnityEngine.MonoBehaviour.print("hi");
        }

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
            return $"{itemName} ID: {GetItemID()}";
        }
        #endregion
    }
}
