using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Enums;
using BeeGame.Core;

namespace BeeGame.Bee
{
    [System.Serializable]
    public struct HoneyComb
    {
        HoneyCombType type;

        public Color colour
        {
            get
            {
                return BeeDictionarys.GetHoneyColour(type);
            }
        }


        public HoneyComb(HoneyCombType _type)
        {
            type = _type;
        }

        public override int GetHashCode()
        {
            return (int)type;
        }
    }
}
