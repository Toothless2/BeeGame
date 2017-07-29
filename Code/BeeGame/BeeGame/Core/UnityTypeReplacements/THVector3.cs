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
        #region Data
        /// <summary>
        /// X position
        /// </summary>
        public float x;
        /// <summary>
        /// Y postion
        /// </summary>
        public float y;
        /// <summary>
        /// Z position
        /// </summary>
        public float z;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor from 3 floats
        /// </summary>
        /// <param name="x">X position</param>
        /// <param name="y">Y position</param>
        /// <param name="z">Z position</param>
        public THVector3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        /// <summary>
        /// Constructor from another <see cref="THVector3"/>
        /// </summary>
        /// <param name="vec3">Vector to make this from</param>
        public THVector3(THVector3 vec3)
        {
            this = vec3;
        }

        /// <summary>
        /// Constructor from another <see cref="Vector3"/>
        /// </summary>
        /// <param name="vec3">Vector to make this from</param>
        public THVector3(Vector3 vec3)
        {
            this = vec3;
        }

        /// <summary>
        /// Constructor from another <see cref="Terrain.ChunkWorldPos"/>
        /// </summary>
        /// <param name="vec3">Vector to make this from</param>
        public THVector3(Terrain.ChunkWorldPos vec3)
        {
            this = vec3;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Distance between 2 vectors
        /// </summary>
        /// <param name="a">First Vector</param>
        /// <param name="b">Second Vector</param>
        /// <returns>Distance between <paramref name="a"/> and <paramref name="b"/></returns>
        public static float Distance(THVector3 a, THVector3 b)
        {
            return (float)Math.Sqrt(Math.Pow((a.x - b.x), 2) + Math.Pow((a.y - b.y), 2) + Math.Pow((a.z - b.z), 2));
        }
        #endregion

        #region Overrides
        /// <summary>
        /// This this vector == to another
        /// </summary>
        /// <param name="obj">object to check against</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (!(obj is THVector3))
                return false;
            if (obj.GetHashCode() == GetHashCode())
                return true;
            return false;
        }
        
        /// <summary>
        /// Gets the hascode for the vector
        /// </summary>
        /// <returns></returns>
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
        
        /// <summary>
        /// Formats the vector as a nice string
        /// </summary>
        /// <returns>The vector as a nice string</returns>
        public override string ToString()
        {
            return $"{x}, {y}, {z}";
        }

        /// <summary>
        /// Checks if <paramref name="a"/> == <paramref name="b"/>
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>true if <paramref name="a"/> == <paramref name="b"/></returns>
        public static bool operator ==(THVector3 a, THVector3 b)
        {
            return a.Equals(b);
        }
        /// <summary>
        /// Inverse of ==
        /// </summary>
        /// <param name="a">First vector</param>
        /// <param name="b">Second vector</param>
        /// <returns>true if <paramref name="a"/> != <paramref name="b"/></returns>
        public static bool operator !=(THVector3 a, THVector3 b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Adds vector a and b
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>returns new vector that is the sum of a and b</returns>
        public static THVector3 operator +(THVector3 a, THVector3 b)
        {
            a.x += b.x;
            a.y += b.y;
            a.z += b.z;

            return a;
        }
        /// <summary>
        /// Adds b to vector a
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">float b</param>
        /// <returns>returns new vector that is the sum of a and b</returns>
        public static THVector3 operator +(THVector3 a, float b)
        {
            a.x += b;
            a.y += b;
            a.z += b;

            return a;
        }
        /// <summary>
        /// Adds a to vector b
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">float b</param>
        /// <returns>returns new vector that is the sum of a and b</returns>
        public static THVector3 operator +(float a, THVector3 b)
        {
            return new THVector3(a + b.x, a + b.y, a + b.z);
        }
        /// <summary>
        /// Subtracs vector a and b
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>returns new vector that is the subtraction of a from b</returns>
        public static THVector3 operator -(THVector3 a, THVector3 b)
        {
            a.x -= b.x;
            a.y -= b.y;
            a.z -= b.z;

            return a;
        }
        /// <summary>
        /// Subtracts b from vector a
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">float b</param>
        /// <returns>returns new vector that is the subtraction of a from b</returns>
        public static THVector3 operator -(THVector3 a, float b)
        {
            a.x += b;
            a.y += b;
            a.z += b;

            return a;
        }
        /// <summary>
        /// Subtracts a from vector b
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">float b</param>
        /// <returns>returns new vector that is the subtraction of a from b</returns>
        public static THVector3 operator -(float a, THVector3 b)
        {
            return new THVector3(a - b.x, a - b.y, a - b.z);
        }
        /// <summary>
        /// Multiplies vector a and b
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>returns new vector that is the product of a and b</returns>
        public static THVector3 operator *(THVector3 a, THVector3 b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;

            return a;
        }
        /// <summary>
        /// Multiples b to vector a
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">float b</param>
        /// <returns>returns new vector that is the product of a and b</returns>
        public static THVector3 operator *(THVector3 a, float b)
        {
            a.x *= b;
            a.y *= b;
            a.z *= b;

            return a;
        }
        /// <summary>
        /// Multiples a to vector b
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">float b</param>
        /// <returns>returns new vector that is the product of a and b</returns>
        public static THVector3 operator *(float a, THVector3 b)
        {
            return new THVector3(a * b.x, a * b.y, a * b.z);
        }
        /// <summary>
        /// Divides vector a by vector b
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">Vector b</param>
        /// <returns>returns new vector that is the division of a by b</returns>
        public static THVector3 operator /(THVector3 a, THVector3 b)
        {
            a.x /= b.x;
            a.y /= b.y;
            a.z /= b.z;

            return a;
        }
        /// <summary>
        /// Divides vector a by vector b
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">float b</param>
        /// <returns>returns new vector that is the division of a by b</returns>
        public static THVector3 operator /(THVector3 a, float b)
        {
            a.x /= b;
            a.y /= b;
            a.z /= b;

            return a;
        }
        /// <summary>
        /// Divides vector b by vector a
        /// </summary>
        /// <param name="a">Vector a</param>
        /// <param name="b">float b</param>
        /// <returns>returns new vector that is the division of b by a</returns>
        public static THVector3 operator /(float a, THVector3 b)
        {
            return new THVector3(a / b.x, a / b.y, a / b.z);
        }
        #endregion

        #region Implicit Operators
        /// <summary>
        /// Converts <see cref="THVector3"/> to <see cref="Vector3"/> implicetly
        /// </summary>
        /// <param name="vec3">Vector to convert</param>
        public static implicit operator Vector3(THVector3 vec3)
        {
            return new Vector3(vec3.x, vec3.y, vec3.z);
        }

        /// <summary>
        /// Converts <see cref="Vector3"/> to <see cref="THVector3"/> implicetly
        /// </summary>
        /// <param name="vec3">Vector to convert</param>
        public static implicit operator THVector3(Vector3 vec3)
        {
            return new THVector3(vec3.x, vec3.y, vec3.z);
        }
        #endregion

        #region Explicit Operators
        /// <summary>
        /// Converts a THVector3 to a <see cref="Quaternion"/>
        /// </summary>
        /// <param name="vec3">Vector to convert to <see cref="Quaternion"/></param>
        /// <remarks>
        /// Explicit as conversion is not exact
        /// </remarks>
        public static explicit operator Quaternion(THVector3 vec3)
        {
            return new Quaternion(vec3.x, vec3.y, vec3.z, 0);
        }
        #endregion
    }
}
