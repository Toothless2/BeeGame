using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame.Core;

namespace BeeGame
{
    /// <summary>
    /// My Serializable versiton of <see cref="Vector3"/> from the <see cref="UnityEngine"/>
    /// </summary>
    [System.Serializable]
    public struct THVector3
    {
        public float x;
        public float y;
        public float z;

        /// <summary>
        /// Constructor, from a <see cref="Vector3"/>
        /// </summary>
        /// <param name="vector"><see cref="Vector3"/></param>
        public THVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }
        /// <summary>
        /// Cronstructor, from 3 floats
        /// </summary>
        /// <param name="_x">x compoent</param>
        /// <param name="_y">y component</param>
        /// <param name="_z">z component</param>
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
        public override bool Equals(object obj)
        {
            if (!(obj is THVector3))
                return false;

            THVector3 vec3 = (THVector3)obj;

            if (vec3.x == x && vec3.y == y && vec3.z == z)
                return true;

            return false;
        }
        #endregion

        #region Operators
        //equality operators
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
        //allowing addition, subtraction, miltiplication, and division of vector by other vectors
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
        public static THVector3 operator /(THVector3 a, THVector3 b)
        {
            return new THVector3(a.x / b.x, a.y / b.y, a.z / b.z);
        }
        //allowing addition, subtraction, miltiplication, and division of vector by floats (affectes all numbers in the vector the same)
        public static THVector3 operator +(THVector3 a, float b)
        {
            return new THVector3(a.x + b, a.y + b, a.z + b);
        }
        public static THVector3 operator -(THVector3 a, float b)
        {
            return new THVector3(a.x - b, a.y - b, a.z - b);
        }
        public static THVector3 operator *(THVector3 a, float b)
        {
            return new THVector3(a.x * b, a.y * b, a.z * b);
        }
        public static THVector3 operator /(THVector3 a, float b)
        {
            return new THVector3(a.x / b, a.y / b, a.z / b);
        }
        //allowing addition, subtraction, miltiplication, and division of vector by floats (affectes all numbers in the vector the same) the other way around
        public static THVector3 operator +(float a, THVector3 b)
        {
            return new THVector3(a + b.x, a + b.y, a + b.z);
        }
        public static THVector3 operator -(float a, THVector3 b)
        {
            return new THVector3(a - b.x, a - b.y, a - b.z);
        }
        public static THVector3 operator *(float a, THVector3 b)
        {
            return new THVector3(a * b.x, a * b.y, a * b.z);
        }
        public static THVector3 operator /(float a, THVector3 b)
        {
            //could be useful if you want the inverse of a vector, I guess
            return new THVector3(a / b.x, a / b.y, a / b.z);
        }
        //allowing implicet convensions between the two vector types as they are effectivly the same
        public static implicit operator THVector3(Vector3 vec3)
        {
            return new THVector3(vec3);
        }
        public static implicit operator Vector3(THVector3 vec3)
        {
            return vec3.ToUnityVector3();
        }
        #endregion
    }
}