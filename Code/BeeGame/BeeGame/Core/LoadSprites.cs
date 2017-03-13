using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace BeeGame.Core
{
    public static class LoadSprites
    {
        private static string[] splitCharacters = new string[3] { "/", ".", ","};

        /// <summary>
        /// Loads the sprites in the sprite file into the sprite dictionary
        /// </summary>
        public static void SpriteLoad()
        {            
            List<List<string>> sprites = GetSpriteNames();
            
            for(int i = 0; i < sprites.Count; i++)
            {
                SpriteDictionary.AddToSpriteDictionary(sprites[i][0], (Sprite)Resources.Load("Sprites/" + sprites[i][1], typeof(Sprite)));
            }
        }

        /// <summary>
        /// Looks in the Sprite Names file for the sprite names and filenames
        /// </summary>
        /// <returns>Sprite Names and File names</returns>
        private static List<List<string>> GetSpriteNames()
        {
            string path = Application.dataPath + "/Resources/Sprites/SpriteNames.dat";
            string lineText = "";
            List<List<string>> returnSprites = new List<List<string>>();

            if(File.Exists(path))
            {
                StreamReader objReader;
                objReader = new StreamReader(path);

                do
                {
                    lineText = objReader.ReadLine();
                    string[] splitSprite = lineText.Split(splitCharacters, StringSplitOptions.None);

                    List<string> temp = new List<string>() { splitSprite[0], splitSprite[1] };

                    returnSprites.Add(temp);

                } while (objReader.Peek() != -1);

                objReader.Close();
            }

            return returnSprites;
        }
    }
}