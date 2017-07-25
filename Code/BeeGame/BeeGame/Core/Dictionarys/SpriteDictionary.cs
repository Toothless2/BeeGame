using System.Collections.Generic;
using UnityEngine;

namespace BeeGame.Core.Dictionaries
{
    /// <summary>
    /// All of the sprites avaliable to the game
    /// </summary>
    public static class SpriteDictionary
    {
        /// <summary>
        /// All of the sprites avaliable to spawn in
        /// </summary>
        private static Dictionary<string, Sprite> itemSpriteDictionary = new Dictionary<string, Sprite>();

        /// <summary>
        /// Get a sprite of the given name
        /// </summary>
        /// <param name="spriteName">Name of sprite to get</param>
        /// <returns>A sprite of the given name, null if no sprite of that name exists</returns>
        public static Sprite GetSprite(string spriteName)
        {
            itemSpriteDictionary.TryGetValue(spriteName, out Sprite sprite);

            if (sprite == null)
                return new Sprite();

            return sprite;
        }

        /// <summary>
        /// Loads the sprites into the dictionary
        /// </summary>
        public static void LoadSprites()
        {
            itemSpriteDictionary = Resources.Resources.GetSprites();
        }
    }
}
