using System;
using System.Globalization;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Core.Enums;

namespace BeeGame.Items
{
    /// <summary>
    /// The bee item
    /// </summary>
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
        /// Overrided so can be set
        /// </summary>
        public override int maxStackCount { get { return maxStack; } }
        private int maxStack = 64;

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
            if (beeType == BeeType.PRINCESS || beeType == BeeType.QUEEN)
                maxStack = 1;
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
            if (beeType == BeeType.PRINCESS || beeType == BeeType.QUEEN)
                maxStack = 1;
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

        #region Bee Stuff
        /// <summary>
        /// Will convery this bee to a <see cref="BeeType.QUEEN"/> useing this bees stats as the <see cref="BeeType.PRINCESS"/> stats
        /// </summary>
        /// <param name="drone"></param>
        public void ConvertToQueen(NormalBee drone)
        {
            ConvertToQueen(this.normalBee, drone);
        }

        /// <summary>
        /// Will Convert this bee into a <see cref="BeeType.QUEEN"/> Bee
        /// </summary>
        /// <param name="princess">The <see cref="BeeType.PRINCESS"/> Stats</param>
        /// <param name="drone">The <see cref="BeeType.DRONE"/></param>
        public void ConvertToQueen(NormalBee princess, NormalBee drone)
        {
            beeType = BeeType.QUEEN;
            queenBee = new QueenBee(princess, drone);
            normalBee = null;

            itemName = new CultureInfo("en-US", false).TextInfo.ToTitleCase($"{queenBee.queen.pSpecies} {beeType}".ToLower());
        }

        /// <summary>
        /// Make a bee with given stats
        /// </summary>
        /// <param name="beeType"><see cref="BeeType"/></param>
        /// <param name="species"><see cref="BeeSpecies"/></param>
        /// <param name="lifespan"><see cref="BeeLifeSpan"/></param>
        /// <param name="fertility">1 or greater</param>
        /// <param name="effect"><see cref="BeeEffect"/></param>
        /// <param name="prodSpeed"><see cref="BeeProductionSpeed"/></param>
        /// <returns>A <see cref="Bee"/> with the given stats</returns>
        public Bee MakeBeeWithStats(BeeType beeType = BeeType.DRONE, BeeSpecies species = BeeSpecies.FOREST, BeeLifeSpan lifespan = BeeLifeSpan.NORMAL, uint fertility = 2, BeeEffect effect = BeeEffect.NONE, BeeProductionSpeed prodSpeed = BeeProductionSpeed.NORMAL)
        {
            NormalBee normBee = new NormalBee()
            {
                pSpecies = species,
                pLifespan = lifespan,
                pFertility = fertility,
                pProdSpeed = prodSpeed,
                pEffect = effect,
                sEffect = effect,
                sFertility = fertility,
                sLifespan = lifespan,
                sProdSpeed = prodSpeed,
                sSpecies = species
            };

            switch (beeType)
            {
                case BeeType.QUEEN:
                    return new Bee(beeType, new QueenBee(normBee, normBee));
                default:
                    return new Bee(beeType, normalBee);
            }
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

        public QueenBee() { }

        public QueenBee(NormalBee princess, NormalBee drone)
        {
            this.queen = princess;
            this.drone = drone;
        }

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
