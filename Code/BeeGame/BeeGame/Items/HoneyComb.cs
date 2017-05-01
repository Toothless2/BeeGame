using System;
using System.Globalization;
using BeeGame.Core;
using BeeGame.Core.Enums;
using UnityEngine;

namespace BeeGame.Items
{
    [Serializable]
    public class HoneyComb : Item
    {
        public HoneyCombType type;

        public Color CombColour
        {
            get
            {
                return BeeDictionarys.GetCombColour(type);
            }
        }

        public HoneyComb(HoneyCombType type) : base(new CultureInfo("en-US", false).TextInfo.ToTitleCase($"{type.ToString()} Comb"))
        {
            usesGameObject = true;
            this.type = type;
        }

        public override Sprite GetItemSprite()
        {
            Sprite comb = SpriteDictionary.GetSprite("HoneyComb");
            
            return comb;
        }

        public override GameObject GetGameObject()
        {
            GameObject obj = PrefabDictionary.GetPrefab("HoneyComb");
            obj.GetComponent<Transform>().localScale = new THVector3(0.2f, 0.2f, 0.2f);
            obj.transform.GetChild(0).GetComponentInChildren<Renderer>().sharedMaterial.SetColor("_OverlayColour", CombColour);
            return obj;
        }

        public override string GetItemID()
        {
            return $"{GetHashCode()}\\{(int)type}";
        }

        public override int GetHashCode()
        {
            return 8;
        }
    }
}
