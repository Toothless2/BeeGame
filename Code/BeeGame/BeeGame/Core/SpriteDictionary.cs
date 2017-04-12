using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeeGame.Core
{
    public static class SpriteDictionary
    {
        private static Dictionary<string, Sprite> itemSpriteDictionary = new Dictionary<string, Sprite>();

        public static Sprite GetSprite(string spriteName)
        {
            itemSpriteDictionary.TryGetValue(spriteName, out Sprite sprite);

            if (sprite == null)
                return new Sprite();

            return sprite;
        }

        public static void LoadSprites()
        {
            itemSpriteDictionary = Resources.Resources.GetSprites();
        }
    }
}
