using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using BeeGame.Enums;
using BeeGame.Bee;
using BeeGame.Items;

namespace BeeGame.Core
{
    /// <summary>
    /// \todo add summary tags to all dictionarys
    /// </summary>
    public static class BeeDictionarys
    {
        #region Default Bee Data Dictionary
        /// <summary>
        /// Contains the default data for all the bee species, this will then be edited in the alveary/apiary according to the bees it is combined with
        /// </summary>
        private static Dictionary<BeeSpecies, BeeData> beeDefaultData = new Dictionary<BeeSpecies, BeeData>()
        {
            { BeeSpecies.FOREST, new BeeData {pSpecies = BeeSpecies.FOREST, pLifespan = BeeLifeSpan.NORMAL, pFertility = 3, pEffect = BeeEffect.NONE, pProdSpeed = BeeProductionSpeed.NORMAL, tempPref = BeeTempPreferance.TEMPERATE, tempTol = new int[] {-1, 1}, humidPref = BeeHumidityPreferance.TEMPERATE, humidTol = new int[] {-1, 1}, nocturnal = false, flyer = false} },
            { BeeSpecies.MEADOWS, new BeeData {pSpecies = BeeSpecies.MEADOWS, pLifespan = BeeLifeSpan.NORMAL, pFertility = 3, pEffect = BeeEffect.NONE, pProdSpeed = BeeProductionSpeed.NORMAL, tempPref = BeeTempPreferance.TEMPERATE, tempTol = new int[] {-1, 1}, humidPref = BeeHumidityPreferance.TEMPERATE, humidTol = new int[] {-1, 1}, nocturnal = false, flyer = false} }
        };

        /// <summary>
        /// Returns the default bee data for a given species
        /// </summary>
        /// <param name="beeSpecies"><see cref="BeeSpecies"/></param>
        /// <returns>Returns the default <see cref="BeeData"/> for the given species</returns>
        public static BeeData? GetDefaultBeeData(BeeSpecies beeSpecies)
        {
            if (beeDefaultData.ContainsKey(beeSpecies))
            {
                BeeData bee = new BeeData(beeDefaultData[beeSpecies]);
                return bee;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Bee Combination Dictionary
        private static Dictionary<BeeSpecies[], BeeSpecies[]> beeCombinations = new Dictionary<BeeSpecies[], BeeSpecies[]>(new BeeDictionaryEqualityComparer())
        {
            {new BeeSpecies[6] {BeeSpecies.FOREST, BeeSpecies.MEADOWS, BeeSpecies.TROPICAL, BeeSpecies.WINTRY, BeeSpecies.MODEST, BeeSpecies.MARSHY}, new BeeSpecies[1] {BeeSpecies.COMMON} },
            {new BeeSpecies[2] {BeeSpecies.SETADFAST, BeeSpecies.VALIANT}, new BeeSpecies[1] {BeeSpecies.HEROIC} }
        };

        public static BeeSpecies[] GetCombination(BeeSpecies species1, BeeSpecies species2)
        {
            BeeSpecies[] speciesArray = new BeeSpecies[2] { species1, species2 };

            BeeSpecies[][] keyss = beeCombinations.Keys.ToArray();
            BeeDictionaryEqualityComparer comp = new BeeDictionaryEqualityComparer();

            for (int i = 0; i < keyss.Length; i++)
            {
                if (comp.Equals(keyss[i], speciesArray))
                {
                    return beeCombinations[keyss[i]];
                }
            }

            return null;
        }
        #endregion

        #region Bee Mutation Chance
        private static Dictionary<BeeSpecies, float> beeMutationChance = new Dictionary<BeeSpecies, float>()
        {
            {BeeSpecies.COMMON, 0.15f },
            {BeeSpecies.HEROIC, 0.06f }
        };

        public static float GetMutationChance(BeeSpecies species)
        {
            if (beeMutationChance.ContainsKey(species))
            {
                return beeMutationChance[species];
            }
            else
            {
                return 1f;
            }
        }

        /// <summary>
        /// Returns the mutation chances for each of the <see cref="BeeSpecies[]"/> that are given. Returns values in the same order they are given
        /// </summary>
        /// <param name="species"><see cref="BeeSpecies"/> that the mutation chances are required for</param>
        /// <returns>Return null if the <see cref="BeeSpecies"/> is not in the <see cref="beeMutationChance"/> dictionary else it will return the mutation chances as a float array</returns>
        public static float[] GetMutationChance(BeeSpecies[] species)
        {
            for (int i = 0; i < species.Length; i++)
            {
                if (!beeMutationChance.ContainsKey(species[i]))
                {
                    return null;
                }
            }

            float[] returnValues = new float[species.Length];

            for (int i = 0; i < species.Length; i++)
            {
                returnValues[i] = beeMutationChance[species[i]];
            }

            return returnValues;
        }
        #endregion

        #region Bee Items Dictionary
        private static Dictionary<BeeSpecies, Item[]> items = new Dictionary<BeeSpecies, Item[]>()
        {
            {BeeSpecies.FOREST, new Item[1] },
            {BeeSpecies.MEADOWS, new Item[1] },
            {BeeSpecies.TROPICAL, new Item[1] }
        };


        public static Item[] GetItems(BeeSpecies species)
        {
            return items[species];
        }
        #endregion

        #region BeeColourDictionary
        static Color CombCol(int r, int g, int b)
        {
            return new Color(r / 255f, g / 255f, b / 255f);
        }

        private static Dictionary<HoneyCombType, Color> HoneyCombColour = new Dictionary<HoneyCombType, Color>()
        {
            {HoneyCombType.HONEY, CombCol(255, 164, 56)}
        };

        public static Color GetHoneyColour(HoneyCombType type)
        {
            if (HoneyCombColour.ContainsKey(type))
            {
                return HoneyCombColour[type];
            }

            return HoneyCombColour[HoneyCombType.HONEY];
        }
        #endregion
    }
}