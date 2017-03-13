using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace BeeGame.Core
{
    public static class SpriteDictionary
    {
        /// <summary>
        /// Stores all sprites that can be loaded by the game
        /// </summary>
        public static Dictionary<string, Sprite> sprites = new Dictionary<string, Sprite>();

        public static string Key()
        {
            string returnKeys = "";
            foreach(string k in sprites.Keys)
            {
                returnKeys += k + " ";
            }

            return returnKeys;
        }
        
        /// <summary>
        /// Adds a sprite to the dictionary
        /// </summary>
        /// <param name="spriteName">Name of sprite</param>
        /// <param name="spriteToAdd">Sprite</param>
        public static void AddToSpriteDictionary(string spriteName, Sprite spriteToAdd)
        {
            if (GetSpriteItemFromDictionary(spriteName) == null)
            {
                sprites.Add(spriteName, spriteToAdd);
            }
            else
            {
                sprites[spriteName] = spriteToAdd;
            }
        }
        
        /// <summary>
        /// Returns a Sprite fromthe given sprite name
        /// </summary>
        /// <param name="spriteName">Item sprite name</param>
        /// <returns>Sprite</returns>
        public static Sprite GetSpriteItemFromDictionary(string spriteName)
        {
            if (spriteName != null)
            {
                if (sprites.ContainsKey(spriteName))
                {
                    return sprites[spriteName];
                }
            }
            
            return null;
        }
    }
}