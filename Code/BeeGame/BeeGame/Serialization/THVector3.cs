using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeeGame
{
    [System.Serializable]
    public struct THVector3
    {
        public float x;
        public float y;
        public float z;

        public THVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
        public THVector3(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }

        #region Hashcode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object b)
        {
            if (b == null) return false;
            if (b.GetType() != GetType()) return false;
            return (this == (THVector3)b);
        }
        #endregion

        #region Operators
        public static bool operator ==(THVector3 a, THVector3 b)
        {

            if(a.x == b.x && a.y == b.y && a.z == b.z)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(THVector3 a, THVector3 b)
        {
            return !(a == b);
        }
        public static THVector3 operator +(THVector3 a, THVector3 b)
        {
            return new THVector3(a.x + b.x, a.y + b.y, a.z + b.z);
        }
        public static THVector3 operator -(THVector3 a, THVector3 b)
        {
            return new THVector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static THVector3 operator *(THVector3 a, THVector3 b)
        {
            return new THVector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }
        public static THVector3 operator *(THVector3 a, float b)
        {
            return new THVector3(a.x * b, a.y * b, a.z * b);
        }
        public static THVector3 operator /(THVector3 a, THVector3 b)
        {
            return new THVector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        public static THVector3 operator /(THVector3 a, float b)
        {
            return new THVector3(a.x / b, a.y / b, a.z / b);
        }
        #endregion
    }
}