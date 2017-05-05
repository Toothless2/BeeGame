using System.Collections.Generic;
using BeeGame.Core.Enums;
using UnityEngine;

namespace BeeGame.Core
{
    public static class BeeDictionarys
    {
        #region Bee Colours
        private static Dictionary<BeeSpecies, Color> beeColour = new Dictionary<BeeSpecies, Color>()
        {
            {BeeSpecies.FOREST, CombColour(0, 255, 0) }
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
