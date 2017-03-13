using System;
using BeeGame.Items;
using BeeGame.Enums;
using BeeGame.Core;

namespace BeeGame.Bee
{
    /// <summary>
    /// Data Storgae for the Bee's in the game
    /// </summary>
    [Serializable]
    public struct BeeData
    {
        /// <summary>
        /// Can the bees data be seen in the inventory?
        /// </summary>
        public bool canSeeBeeData;

        #region Data
        /// <summary>
        /// <see cref="BeeType"/> of the Bee
        /// </summary>
        public BeeType beeType;

        #region Phenotype
        //General bee info
        /// <summary>
        /// Primary <see cref="BeeGame.Enums.BeeSpecies"/> of the Bee
        /// </summary>
        public BeeSpecies pSpecies;
        /// <summary>
        /// Primary <see cref="BeeGame.Enums.BeeLifeSpan"/> of the Bee
        /// </summary>
        public BeeLifeSpan pLifespan;
        /// <summary>
        /// Primary Fertility of the Bee
        /// </summary>
        public uint pFertility;
        /// <summary>
        /// Primary <see cref="BeeGame.Enums.BeeEffect"/> of the Bee
        /// </summary>
        public BeeEffect pEffect;
        /// <summary>
        /// Primary <see cref="BeeGame.Enums.BeeProductionSpeed"/> of the Bee
        /// </summary>
        public BeeProductionSpeed pProdSpeed;
        #endregion Phenotype#

        #region Secondary
        //General bee info
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

        //Additional info
        /// <summary>
        /// Preferd <see cref="BeeGame.Enums.BeeTempPreferance"/> of the Bee
        /// </summary>
        public BeeTempPreferance tempPref;
        /// <summary>
        /// The variance of the prefered <see cref="BeeGame.Enums.BeeTempPreferance"/> that the bee can withstand eg [-1, +2]
        /// </summary>
        public int[] tempTol;
        /// <summary>
        /// Preferd <see cref="BeeGame.Enums.BeeHumidityPreferance"/> of the Bee
        /// </summary>
        public BeeHumidityPreferance humidPref;
        /// <summary>
        /// The variance of the prefered <see cref="BeeGame.Enums.BeeHumidityPreferance"/> that the bee can withstand eg [-1, +2]
        /// </summary>
        public int[] humidTol;
        /// <summary>
        /// Will the bee work at night
        /// </summary>
        public bool? nocturnal;
        /// <summary>
        /// Will the bee work during the rain/snow/wind
        /// </summary>
        public bool? flyer;

        /// <summary>
        /// Only used when the bee is a Queen and conations the data that the bee is being combined with. Value is also non-serialized
        /// </summary>
        public CombiningBeeData combiningData;

        /// <summary>
        /// The items produced by the bee other than offspring
        /// </summary>
        public Item[] producedItems;
        #endregion

        /// <summary>
        /// Bee data constructor, sets the phenotype and the secondary to the same values
        /// </summary>
        /// <param name="data"><see cref="BeeData"/></param>
        public BeeData(BeeData data)
        {
            this = data;

            this.sSpecies = data.pSpecies;
            this.beeType = data.beeType;
            this.sLifespan = data.pLifespan;
            this.sFertility = data.pFertility;
            this.sEffect = data.pEffect;
            this.sProdSpeed = data.pProdSpeed;

            this.producedItems = BeeDictionarys.GetItems(pSpecies);
        }

        public void SetBeeType(BeeType type)
        {
            this.beeType = type;
        }

        /// <summary>
        /// Updates <see cref="producedItems"/> to the current items that whould be produced by the phenotype bee species
        /// </summary>
        public void UpdateProducedItems()
        {
            this.producedItems = BeeDictionarys.GetItems(pSpecies);
        }

        #region EqualityChecking
        /// <summary>
        /// Makes a unique hashcode for the bee object useing all of the bee's data
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashcode = 13;
                hashcode += (hashcode * 397) ^ (int)pSpecies ^ (int)beeType ^ (int)pLifespan ^ (int)pFertility ^ (int)pEffect ^ (int)pProdSpeed;
                hashcode += (hashcode * 397) ^ (int)sSpecies ^ (int)sLifespan ^ (int)sFertility ^ (int)sEffect ^ (int)sProdSpeed;
                hashcode += (hashcode * 397) ^ (int)tempPref ^ (int)humidPref;

                return hashcode;
            }
        }

        /// <summary>
        /// Overriding to make c# happy
        /// </summary>
        /// <param name="obj">object</param>
        /// <returns>true if objects are equal</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Overriding the Equality operator
        /// </summary>
        /// <param name="a">Bee 1</param>
        /// <param name="b">Bee 2</param>
        /// <returns>true is A Equal to B</returns>
        public static bool operator ==(BeeData a, BeeData b)
        {
            return a.GetHashCode() == b.GetHashCode();
        }

        /// <summary>
        /// Overriding the not equal operator
        /// </summary>
        /// <param name="a">Bee 1</param>
        /// <param name="b">Bee 2</param>
        /// <returns>Inverse ==</returns>
        public static bool operator !=(BeeData a, BeeData b)
        {
            return !(a == b);
        }
        #endregion EqualityChecking
    }

    #region Queen Combing Bee
    [Serializable]
    ///<summary>
    /// Holds the data that the bee queen in conbining with. Exists due to a <see cref="BeeData"/> variable with in a <see cref="BeeData"/> is to deep for serialization
    ///</summary>
    public struct CombiningBeeData
    {
        BeeSpecies species;

        #region Data
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

        /// <summary>
        /// Secondary <see cref="BeeGame.Enums.BeeSpecies"/> of the Bee
        /// </summary>
        public BeeSpecies sSpecies;
        /// <summary>
        /// Preferd <see cref="BeeGame.Enums.BeeTempPreferance"/> of the Bee
        /// </summary>
        public BeeTempPreferance tempPref;
        /// <summary>
        /// The variance of the prefered <see cref="BeeGame.Enums.BeeTempPreferance"/> that the bee can withstand eg [-1, +2]
        /// </summary>
        public int[] tempTol;
        /// <summary>
        /// Preferd <see cref="BeeGame.Enums.BeeHumidityPreferance"/> of the Bee
        /// </summary>
        public BeeHumidityPreferance humidPref;
        /// <summary>
        /// The variance of the prefered <see cref="BeeGame.Enums.BeeHumidityPreferance"/> that the bee can withstand eg [-1, +2]
        /// </summary>
        public int[] humidTol;
        /// <summary>
        /// Will the bee work at night
        /// </summary>
        public bool? nocturnal;
        /// <summary>
        /// Will the bee work during the rain/snow/wind
        /// </summary>
        public bool? flyer;
        #endregion

        /// <summary>
        /// \todo comment this
        /// </summary>
        /// <param name="data"></param>
        public void ToCombiningBeeData(BeeData data)
        {
            species = data.pSpecies;
            sSpecies = data.sSpecies;
            sLifespan = data.sLifespan;
            sFertility = data.sFertility;
            sEffect = data.sEffect;
            sProdSpeed = data.sProdSpeed;
            tempPref = data.tempPref;
            tempTol = data.tempTol;
            humidPref = data.humidPref;
            humidTol = data.humidTol;
            nocturnal = data.nocturnal;
            flyer = data.flyer;
        }

        /// <summary>
        /// Reconverts the <see cref="CombiningBeeData"/> back to normal <see cref="BeeData"/>
        /// </summary>
        /// <param name="data"></param>
        public BeeData ToBeeData()
        {
            BeeData bee = new BeeData();

            bee.nocturnal = nocturnal;
            bee.tempTol = tempTol;
            bee.humidTol = humidTol;
            bee.flyer = flyer;

            return bee;
        }
    }
    #endregion
}