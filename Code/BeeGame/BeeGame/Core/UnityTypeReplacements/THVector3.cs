using System;
using UnityEngine;

namespace BeeGame.Core
{
    /// <summary>
    /// Serializable version of <see cref="Vector3"/>
    /// </summary>
    [Serializable]
    public struct THVector3
    {
        public float x;
        public float y;
        public float z;

        #region Constructors
        public THVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public THVector3(THVector3 vec3)
        {
            this = vec3;
        }

        public THVector3(Vector3 vec3)
        {
            this = vec3;
        }

        public THVector3(Terrain.ChunkWorldPos vec3)
        {
            this = vec3;
        }
        #endregion

        public static float Distance(THVector3 a, THVector3 b)
        {
            float temp = (float)Math.Sqrt(Math.Pow((a.x - b.x), 2) + Math.Pow((a.y - b.y), 2) + Math.Pow((a.z - b.z), 2));
            return temp;
        }

        #region Overrides
        public override bool Equals(object obj)
        {
            if (!(obj is THVector3))
                return false;
            if (obj.GetHashCode() == GetHashCode())
                return true;
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 13;

                hash *= 443 * x.GetHashCode();
                hash *= 373 * y.GetHashCode();
                hash *= 127 * z.GetHashCode();

                return hash;
            }
        }

        public override string ToString()
        {
            return $"{x}, {y}, {z}";
        }

        public static bool operator ==(THVector3 a, THVector3 b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(THVector3 a, THVector3 b)
        {
            return !(a == b);
        }

        public static THVector3 operator +(THVector3 a, THVector3 b)
        {
            a.x += b.x;
            a.y += b.y;
            a.z += b.z;

            return a;
        }
        public static THVector3 operator +(THVector3 a, float b)
        {
            a.x += b;
            a.y += b;
            a.z += b;

            return a;
        }
        public static THVector3 operator +(float a, THVector3 b)
        {
            return new THVector3(a + b.x, a + b.y, a + b.z);
        }
        public static THVector3 operator -(THVector3 a, THVector3 b)
        {
            a.x -= b.x;
            a.y -= b.y;
            a.z -= b.z;

            return a;
        }
        public static THVector3 operator -(THVector3 a, float b)
        {
            a.x += b;
            a.y += b;
            a.z += b;

            return a;
        }
        public static THVector3 operator -(float a, THVector3 b)
        {
            return new THVector3(a - b.x, a - b.y, a - b.z);
        }
        public static THVector3 operator *(THVector3 a, THVector3 b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;

            return a;
        }
        public static THVector3 operator *(THVector3 a, float b)
        {
            a.x *= b;
            a.y *= b;
            a.z *= b;

            return a;
        }
        public static THVector3 operator *(float a, THVector3 b)
        {
            return new THVector3(a * b.x, a * b.y, a * b.z);
        }
        public static THVector3 operator /(THVector3 a, THVector3 b)
        {
            a.x /= b.x;
            a.y /= b.y;
            a.z /= b.z;

            return a;
        }
        public static THVector3 operator /(THVector3 a, float b)
        {
            a.x /= b;
            a.y /= b;
            a.z /= b;

            return a;
        }
        public static THVector3 operator /(float a, THVector3 b)
        {
            return new THVector3(a / b.x, a / b.y, a / b.z);
        }
        #endregion

        #region Implicit Operators
        public static implicit operator Vector3(THVector3 vec3)
        {
            return new Vector3(vec3.x, vec3.y, vec3.z);
        }

        public static implicit operator THVector3(Vector3 vec3)
        {
            return new THVector3(vec3.x, vec3.y, vec3.z);
        }
        #endregion
    }
}
