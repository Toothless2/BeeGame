using System;
using System.Globalization;
using BeeGame.Core;
using BeeGame.Core.Enums;
using UnityEngine;

namespace BeeGame.Items
{
    /// <summary>
    /// Honey comb item produced by bees
    /// </summary>
    [Serializable]
    public class HoneyComb : Item
    {
        #region Data
        /// <summary>
        /// The type of comb this is, <see cref="HoneyCombType"/>
        /// </summary>
        public HoneyCombType type { get; set; }

        /// <summary>
        /// The colour if this coumb, <see cref="BeeDictionarys.GetCombColour(HoneyCombType)"/>
        /// </summary>
        public Color CombColour
        {
            get
            {
                return BeeDictionarys.GetCombColour(type);
            }
        }

        /// <summary>
        /// The <see cref="Sprite"/> for this honey comb
        /// </summary>
        [NonSerialized]
        private Sprite itemSprite;
        #endregion

        #region Constructors
        /// <summary>
        /// Make the <see cref="Item"/> from no arguments giveing it the default honey comb value <see cref="HoneyCombType.HONEY"/>
        /// </summary>
        public HoneyComb() : base(new CultureInfo("en-US", false).TextInfo.ToTitleCase($"{HoneyCombType.HONEY} Comb".ToLower()))
        {
            usesGameObject = true;
            type = HoneyCombType.HONEY;
        }

        /// <summary>
        /// Makes a <see cref="HoneyComb"/> for the given <see cref="HoneyCombType"/>
        /// </summary>
        /// <param name="type"><see cref="HoneyCombType"> that this comb is</see></param>
        public HoneyComb(HoneyCombType type) : base(new CultureInfo("en-US", false).TextInfo.ToTitleCase($"{type.ToString()} Comb".ToLower()))
        {
            usesGameObject = true;
            this.type = type;
        }
        #endregion
        
        #region Item Overrides
        /// <summary>
        /// Retuens the sprite for the this of the correct colour
        /// </summary>
        /// <returns><see cref="Sprite"/></returns>
        public override Sprite GetItemSprite()
        {
            return itemSprite ?? (itemSprite = SpriteDictionary.GetSprite("HoneyComb").ColourSprite(CombColour));
        }

        /// <summary>
        /// Returns the game object for this and gives the object the correct colouring
        /// </summary>
        /// <returns><see cref="GameObject"/> for <see cref="this"/></returns>
        public override GameObject GetGameObject()
        {
            GameObject obj = PrefabDictionary.GetPrefab("HoneyComb");
            //* cannot acess the instance material from here have to do it on the obejct
            obj.GetComponent<ApplyColour>().colour = CombColour;
            return obj;
        }

        /// <summary>
        /// Makes the item ID. For this it is the Normal ID \ the <see cref="int"/> value of the <see cref="type"/> this comb is
        /// </summary>
        /// <returns><see cref="Item"/> ID as a <see cref="string"/></returns>
        public override string GetItemID()
        {
            return $"{GetHashCode()}\\{(int)type}";
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the hashcode for <see cref="this"/> <see cref="Item"/>
        /// </summary>
        /// <returns>8</returns>
        public override int GetHashCode()
        {
            return 8;
        }
        #endregion
    }
}
