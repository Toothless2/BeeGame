using System.Collections.Generic;
using System.Linq;
using BeeGame.Core.Enums;
using UnityEngine;
using BeeGame.Core.Dictionarys;

namespace BeeGame.Core
{
    public static class BeeDictionarys
    {
        #region Bee Combination Weights
        private static Dictionary<BeeSpecies, float> beeCombinationWeights = new Dictionary<BeeSpecies, float>()
        {
            {BeeSpecies.COMMON, 0.15f },
            {BeeSpecies.HEROIC, 0.06f }
        };

        public static float[] GetWeights(BeeSpecies[] species)
        {
            var returnArray = new float[species.Length];

            for (int i = 0; i < species.Length; i++)
            {
                if(beeCombinationWeights.ContainsKey(species[i]))
                    returnArray[i] = beeCombinationWeights[species[i]];
                else
                    returnArray[i] = 0.5f;
            }

            return returnArray;
        }
        #endregion

        #region Bee Combinations
        public static Dictionary<BeeSpecies[], BeeSpecies[]> beeCombinations = new Dictionary<BeeSpecies[], BeeSpecies[]>(new BeeCombinationDictionaryEqualityComparer())
        {
             { new BeeSpecies[6] { BeeSpecies.FOREST, BeeSpecies.MEADOWS, BeeSpecies.TROPICAL, BeeSpecies.WINTRY, BeeSpecies.MODEST, BeeSpecies.MARSHY }, new BeeSpecies[1] { BeeSpecies.COMMON } }
        };

        public static BeeSpecies[] GetCombinations(BeeSpecies s1, BeeSpecies s2)
        {
            var beeSpecies = new BeeSpecies[2] { s1, s2 };
            var returnBeeList = new List<BeeSpecies>();

            var keys = beeCombinations.Keys.ToArray();
            var comparor = new BeeCombinationDictionaryEqualityComparer();

            for (int i = 0; i < keys.Length; i++)
            {
                if(comparor.Equals(keys[i], beeSpecies))
                {
                    var temp = beeCombinations[keys[i]];

                    for (int j = 0; j < temp.Length; j++)
                    {
                        returnBeeList.Add(temp[i]);
                    }
                }
            }

            returnBeeList.Add(s1);
            returnBeeList.Add(s2);

            return returnBeeList.ToArray();
        }
        #endregion

        #region Bee Produce
        private static Dictionary<BeeSpecies, Items.Item[]> beeProduce = new Dictionary<BeeSpecies, Items.Item[]>()
        {
            {BeeSpecies.FOREST, new Items.Item[]{new Items.HoneyComb(HoneyCombType.HONEY) } },
            {BeeSpecies.COMMON, new Items.Item[]{new Items.HoneyComb(HoneyCombType.HONEY) } }
        };

        public static Items.Item[] GetBeeProduce(BeeSpecies species)
        {
            beeProduce.TryGetValue(species, out Items.Item[] produce);

            //* of the produce cant be found then return a honey comb as it is probly a bug
            return produce ?? new Items.Item[1] { new Items.HoneyComb(HoneyCombType.HONEY) };
        }
        #endregion

        #region Bee Colours
        private static Dictionary<BeeSpecies, Color> beeColour = new Dictionary<BeeSpecies, Color>()
        {
            {BeeSpecies.FOREST, CombColour(0, 255, 0) },
            {BeeSpecies.COMMON, CombColour(255, 0, 0) }
        };

        public static Color GetBeeColour(BeeSpecies species)
        {
            beeColour.TryGetValue(species, out Color colour);

            return colour != null ? colour : new Color();
        }
        #endregion

        #region Comb Colours
        /// <summary>
        /// The colour of the <see cref="BeeGame.Items.HoneyComb"/> for each of teh <see cref="HoneyCombType"/>s
        /// </summary>
        private static Dictionary<HoneyCombType, Color> honeyCoumbColour = new Dictionary<HoneyCombType, Color>()
        {
            {HoneyCombType.HONEY, CombColour(255, 164, 56) },
            {HoneyCombType.ICEY, CombColour(78, 231, 231) }
        };

        /// <summary>
        /// Makes a new colour given Red, <paramref name="r"/>, Green, <paramref name="g"/>, Blue, <paramref name="b"/>, optionaly an Alpha, <paramref name="a"/>.
        /// Rangeing from 0f-255f
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <param name="a">Alpha, Default no alpha</param>
        /// <returns>new <see cref="Color"/> made with the given r, g, b values</returns>
        private static Color CombColour(float r, float g, float b, float a = 255f)
        {
            return new Color(r / 255f, g / 255f, b / 255f);
        }

        /// <summary>
        /// Returns colour if the given honey coumb
        /// </summary>
        /// <param name="type">Type of the comb</param>
        /// <returns>The <see cref="Color"/> of the comb and a new <see cref="Color.red"/> if the given <see cref="HoneyCombType"/> does not exists as a key in the <see cref="honeyCoumbColour"/> dictionary</returns>
        public static Color GetCombColour(HoneyCombType type)
        {
            honeyCoumbColour.TryGetValue(type, out var temp);

            if (temp == null)
                return new Color(1, 0, 0);

            return temp;
        }
        #endregion
    }
}
