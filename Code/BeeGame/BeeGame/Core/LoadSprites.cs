using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BeeGame.Core
{
    public static class LoadSprites
    {
        /// <summary>
        /// Loads the sprites in the sprite file into the sprite dictionary
        /// </summary>
        public static void SpriteLoad()
        {
            Sprite[] obj = GetSprites();
            List<List<string>> spriteNames = Resources.Resources.SpriteNames;

            //Goes through each sprite that is needed
            for (int i = 0; i < spriteNames.Count; i++)
            {
                //goes through all of the sprites
                for (int j = 0; j < obj.Length; j++)
                {
                    //if the current sprite's name is equal to the sprite name that is needed add it to the sprite dictionary
                    if(obj[j].name == spriteNames[i][1])
                    {
                        SpriteDictionary.AddToSpriteDictionary(spriteNames[i][0], obj[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Gets all Sprites in the Sprite Resources Directory
        /// </summary>
        /// <returns><see cref="Sprite[]"/> of all sprites in the .../Assets/Resources/Sprites/ folder</returns>
        private static Sprite[] GetSprites()
        {
            Sprite[] sprites = UnityEngine.Resources.LoadAll<Sprite>("Sprites");
            return sprites;
        }
    }
}