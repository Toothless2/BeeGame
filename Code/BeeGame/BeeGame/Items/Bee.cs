using System;
using System.Globalization;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Core.Enums;

namespace BeeGame.Items
{
    [Serializable]
    public class Bee : Item
    {
        #region Data
        /// <summary>
        /// Can all of the bee data be seen when hovered over?
        /// </summary>
        public bool canSeeBeeData = false;

        /// <summary>
        /// This bees <see cref="BeeType"/>
        /// </summary>
        public BeeType beeType { get; set; }
        /// <summary>
        /// What was this bees <see cref="BeeType"/>?
        /// </summary>
        private BeeType previousBeeType { get; set; }

        /// <summary>
        /// This bees <see cref="Sprite"/>
        /// </summary>
        [NonSerialized]
        private Sprite itemSprite;

        /// <summary>
        /// If this bee is a <see cref="BeeType.QUEEN"/> this will be not null
        /// </summary>
        /// <remarks>
        /// Possibly change this to an array to 2 <see cref="NormalBee"/>s
        /// </remarks>
        public QueenBee queenBee { get; set; }
        /// <summary>
        /// If this bee is not a <see cref="BeeType.QUEEN"/> this will be not null
        /// </summary>
        public NormalBee normalBee { get; set; }
        #endregion

        #region Constructors
        public Bee()
        {
            normalBee = new NormalBee();
        }


        /// <summary>
        /// Create a bee from <see cref="NormalBee"/>
        /// </summary>
        /// <param name="beeType"><see cref="BeeType"/> of the bee</param>
        /// <param name="normalBee"><see cref="NormalBee"/> data</param>
        public Bee(BeeType beeType, NormalBee normalBee) : base(new CultureInfo("en-US", false).TextInfo.ToTitleCase($"{normalBee.pSpecies} {beeType}".ToLower()))
        {
            this.beeType = beeType;
            this.normalBee = normalBee;
        }

        /// <summary>
        /// Create a bee from <see cref="QueenBee"/>
        /// </summary>
        /// <param name="beeType"><see cref="BeeType"/> of the bee</param>
        /// <param name="normalBee"><see cref="QueenBee"/> data</param>
        public Bee(BeeType beeType, QueenBee queenBee) : base(new CultureInfo("en-US", false).TextInfo.ToTitleCase($"{queenBee.queen.pSpecies} {beeType}".ToLower()))
        {
            this.beeType = beeType;
            this.queenBee = queenBee;
        }
        #endregion

        #region Item Overrides
        /// <summary>
        /// Returns the sprite for this, of the correct colour
        /// </summary>
        /// <returns><see cref="Sprite"/></returns>
        public override Sprite GetItemSprite()
        {
            //* if the bee has not change in any way dont rebuild the sprite as that takes time
            if(previousBeeType == beeType && itemSprite != null)
            {
                return itemSprite;
            }

            previousBeeType = beeType;

            //* set the correct sprite and colour
            if (beeType == BeeType.QUEEN)
            {
                //* avoids the crown, black body, yellow body, and both colours of the wings
                Color[] colorsToAvoid = { new Color(0, 0, 0), new Color(232f, 200f, 42f, 255f) / 255f, new Color(232f, 213f, 106f, 255f) / 255f, new Color(156f, 146f, 130f, 255f) / 255f, new Color(225f, 223f, 219f, 255f) / 255f };
                return itemSprite = SpriteDictionary.GetSprite("Queen").ColourSprite(BeeDictionarys.GetBeeColour((BeeSpecies)(queenBee?.queen.pSpecies)), coloursToAvoid: colorsToAvoid);
            }
            else if (beeType == BeeType.PRINCESS)
            {
                //* avoids the tiara, black body, yellow body, and both colours of the wings
                Color[] colorsToAvoid = { new Color(0, 0, 0), new Color(191f, 195f, 45f, 255f) / 255f, new Color(191f, 195f, 44f, 255f) / 255f, new Color(156f, 146f, 130f, 255f) / 255f, new Color(225f, 223f, 219f, 255f) / 255f, new Color(232f, 200, 42, 255f) / 255f };
                return itemSprite = SpriteDictionary.GetSprite("Princess").ColourSprite(BeeDictionarys.GetBeeColour((BeeSpecies)(normalBee?.pSpecies)), coloursToAvoid: colorsToAvoid);
            }
            else
            {
                //* avoids the block body, yellow body, and both wing colours
                Color[] colorsToAvoid = { new Color(0, 0, 0), new Color(156f, 146f, 130f, 255f) / 255f, new Color(225f, 223f, 219f, 255f) / 255f, new Color(232f, 200, 42, 255f) / 255f };
                return itemSprite = SpriteDictionary.GetSprite("Drone").ColourSprite(BeeDictionarys.GetBeeColour((BeeSpecies)normalBee?.sSpecies), coloursToAvoid: colorsToAvoid);
            }
        }

        /// <summary>
        /// Makes the item ID. For this it is the Normal ID \ the <see cref="int"/> value of the <see cref="queenBee.GetHashCode()"/> or <see cref="normalBee.GetHashCode()"/> as a <see cref="string"/>
        /// </summary>
        /// <returns><see cref="Item"/> ID as a <see cref="string"/></returns>
        public override string GetItemID()
        {
            return $"{GetHashCode()}\\{(int)beeType}{queenBee?.GetHashCode() ?? normalBee?.GetHashCode()}";
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Retuens the hashcode for <see cref="this"/> <see cref="Item"/>
        /// </summary>
        /// <returns>9</returns>
        public override int GetHashCode()
        {
            return 9;
        }
        #endregion
    }

    [Serializable]
    public class QueenBee
    {
        //* Properties so that they can be copied by reflection as it does not copy variables only properties
        /// <summary>
        /// Original princess traits
        /// </summary>
        public NormalBee queen { get; set; }
        /// <summary>
        /// Paired drone traits
        /// </summary>
        public NormalBee drone { get; set; }

        public override int GetHashCode()
        {
            return queen.GetHashCode() ^ drone.GetHashCode();
        }
    }

    [Serializable]
    public class NormalBee
    {
        #region Phenotype
        //* Currently shown traits of the bee

        /// <summary>
        /// Primary <see cref="BeeSpecies"/> of the Bee
        /// </summary>
        public BeeSpecies pSpecies;
        /// <summary>
        /// Primary <see cref="BeeLifeSpan"/> of the Bee
        /// </summary>
        public BeeLifeSpan pLifespan;
        /// <summary>
        /// Primary Fertility of the Bee
        /// </summary>
        public uint pFertility;
        /// <summary>
        /// Primary <see cref="BeeEffect"/> of the Bee
        /// </summary>
        public BeeEffect pEffect;
        /// <summary>
        /// Primary <see cref="BeeProductionSpeed"/> of the Bee
        /// </summary>
        public BeeProductionSpeed pProdSpeed;
        #endregion

        #region Secondary
        //* Traits of the bee used in the bees combination

        /// <summary>
        /// Secondary <see cref="BeeGame.Enums.BeeSpecies"/> of the Bee
        /// </summary>
        public BeeSpecies sSpecies;
        /// <summary>
        /// Secondary <see cref="BeeGame.Enums.BeeLifeSpan"/> of the Bee
        /// </summary>
        public BeeLifeSpan sLifespan;
        /// <summary>
        /// Secondary Fertility of the Bee
        /// </summary>
        public uint sFertility;
        /// <summary>
        /// Secondary <see cref="BeeGame.Enums.BeeEffect"/> of the Bee
        /// </summary>
        public BeeEffect sEffect;
        /// <summary>
        /// Secondary <see cref="BeeGame.Enums.BeeProductionSpeed"/> of the Bee
        /// </summary>
        public BeeProductionSpeed sProdSpeed;
        #endregion Secondary

        public override int GetHashCode()
        {
            unchecked
            {
                int hashcode = 13;

                hashcode += ((int)pSpecies ^ (int)pLifespan ^ (int)pFertility ^ (int)pEffect ^ (int)pProdSpeed) * 127;
                hashcode += ((int)sSpecies ^ (int)sLifespan ^ (int)sFertility ^ (int)sEffect ^ (int)sProdSpeed) * 307;

                return hashcode;
            }
        }
    }
}
