using System;
using UnityEngine;

namespace BeeGame.Core
{
    /// <summary>
    /// Serilializable version of <see cref="Vector2"/>
    /// </summary>
    [Serializable]
    public struct THVector2
    {
        #region Data
        /// <summary>
        /// X position
        /// </summary>
        public float x;
        /// <summary>
        /// Y position
        /// </summary>
        public float y;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor from 2 floats
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        public THVector2(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        /// <summary>
        /// Constructor from another <see cref="THVector2"/>
        /// </summary>
        /// <param name="vec2">Vector to make this from</param>
        public THVector2(THVector2 vec2)
        {
            this = vec2;
        }

        /// <summary>
        /// Constructor from <see cref="Vector2"/>
        /// </summary>
        /// <param name="vec2">Vector to make this from</param>
        public THVector2(Vector2 vec2)
        {
            this = vec2;
        }
        #endregion
        
        #region Overrides
        public override bool Equals(object obj)
        {
            if (!(obj is THVector2))
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

                return hash;
            }
        }

        public override string ToString()
        {
            return $"{x}, {y}";
        }

        public static bool operator ==(THVector2 a, THVector2 b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(THVector2 a, THVector2 b)
        {
            return !(a == b);
        }

        public static THVector2 operator +(THVector2 a, THVector2 b)
        {
            a.x += b.x;
            a.y += b.y;

            return a;
        }
        public static THVector2 operator +(THVector2 a, float b)
        {
            a.x += b;
            a.y += b;

            return a;
        }
        public static THVector2 operator +(float a, THVector2 b)
        {
            return new THVector2(a + b.x, a + b.y);
        }
        public static THVector2 operator -(THVector2 a, THVector2 b)
        {
            a.x -= b.x;
            a.y -= b.y;

            return a;
        }
        public static THVector2 operator -(THVector2 a, float b)
        {
            a.x += b;
            a.y += b;

            return a;
        }
        public static THVector2 operator -(float a, THVector2 b)
        {
            return new THVector2(a - b.x, a - b.y);
        }
        public static THVector2 operator *(THVector2 a, THVector2 b)
        {
            a.x *= b.x;
            a.y *= b.y;

            return a;
        }
        public static THVector2 operator *(THVector2 a, float b)
        {
            a.x *= b;
            a.y *= b;

            return a;
        }
        public static THVector2 operator *(float a, THVector2 b)
        {
            return new THVector2(a * b.x, a * b.y);
        }
        public static THVector2 operator /(THVector2 a, THVector2 b)
        {
            a.x /= b.x;
            a.y /= b.y;

            return a;
        }
        public static THVector2 operator /(THVector2 a, float b)
        {
            a.x /= b;
            a.y /= b;

            return a;
        }
        public static THVector2 operator /(float a, THVector2 b)
        {
            return new THVector2(a / b.x, a / b.y);
        }
        #endregion

        #region Implicit Operators
        public static implicit operator Vector2(THVector2 vec2)
        {
            return new Vector2(vec2.x, vec2.y);
        }

        public static implicit operator THVector2(Vector2 vec2)
        {
            return new THVector2(vec2.x, vec2.y);
        }
        #endregion
    }
}
