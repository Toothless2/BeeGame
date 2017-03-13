using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeeGame.Core
{
    public static class ExtenstionMethods
    {
        public static Vector3 ToUnityVector3(this THVector3 _thVector3)
        {
            return new Vector3(_thVector3.x, _thVector3.y, _thVector3.z);
        }

        public static THVector3 ToTHVecotr3(this Vector3 _vector3)
        {
            return new THVector3(_vector3);
        }
    }
}
