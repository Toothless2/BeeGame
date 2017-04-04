using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Core;

namespace BeeGame
{
    [Serializable]
    public struct THVector2
    {

        public float x;
        public float y;

        /// <summary>
        /// Constructor, from a <see cref="Vector2"/>
        /// </summary>
        /// <param name="vector"><see cref="Vector2"/></param>
        public THVector2(Vector2 vector)
        {
            x = vector.x;
            y = vector.y;
        }
        /// <summary>
        /// Cronstructor, from 3 floats
        /// </summary>
        /// <param name="_x">x compoent</param>
        /// <param name="_y">y component</param>
        public THVector2(float _x, float _y)
        {
            x = _x;
            y = _y;
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
            return (this == (THVector2)b);
        }
        #endregion

        #region Operators
        //equality operators
        public static bool operator ==(THVector2 a, THVector2 b)
        {

            if (a.x == b.x && a.y == b.y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool operator !=(THVector2 a, THVector2 b)
        {
            return !(a == b);
        }
        //allowing addition, subtraction, miltiplication, and division of vector by other vectors
        public static THVector2 operator +(THVector2 a, THVector2 b)
        {
            return new THVector2(a.x + b.x, a.y + b.y);
        }
        public static THVector2 operator -(THVector2 a, THVector2 b)
        {
            return new THVector2(a.x - b.x, a.y - b.y);
        }
        public static THVector2 operator *(THVector2 a, THVector2 b)
        {
            return new THVector2(a.x * b.x, a.y * b.y);
        }
        public static THVector2 operator /(THVector2 a, THVector2 b)
        {
            return new THVector2(a.x / b.x, a.y / b.y);
        }
        //allowing addition, subtraction, miltiplication, and division of vector by floats (affectes all numbers in the vector the same)
        public static THVector2 operator +(THVector2 a, float b)
        {
            return new THVector2(a.x + b, a.y + b);
        }
        public static THVector2 operator -(THVector2 a, float b)
        {
            return new THVector2(a.x - b, a.y - b);
        }
        public static THVector2 operator *(THVector2 a, float b)
        {
            return new THVector2(a.x * b, a.y * b);
        }
        public static THVector2 operator /(THVector2 a, float b)
        {
            return new THVector2(a.x / b, a.y / b);
        }
        //allowing addition, subtraction, miltiplication, and division of vector by floats (affectes all numbers in the vector the same) the other way around
        public static THVector2 operator +(float a, THVector2 b)
        {
            return new THVector2(a + b.x, a + b.y);
        }
        public static THVector2 operator -(float a, THVector2 b)
        {
            return new THVector2(a - b.x, a - b.y);
        }
        public static THVector2 operator *(float a, THVector2 b)
        {
            return new THVector2(a * b.x, a * b.y);
        }
        public static THVector2 operator /(float a, THVector2 b)
        {
            //could be useful if you want the inverse of a vector, I guess
            return new THVector2(a / b.x, a / b.y);
        }
        //allowing implicet convensions between the two vector types as they are effectivly the same
        public static implicit operator THVector2(Vector2 vec2)
        {
            return new THVector2(vec2);
        }
        public static implicit operator Vector2(THVector2 vec2)
        {
            return vec2.ToUnityVector2();
        }
        #endregion
    }
}
