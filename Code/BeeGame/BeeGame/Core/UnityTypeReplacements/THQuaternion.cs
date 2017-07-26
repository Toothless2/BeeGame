using System;
using UnityEngine;

namespace BeeGame.Core.UnityTypeReplacements
{
    [Serializable]
    public struct THQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public static Quaternion identity => new Quaternion(0, 0, 0, 1);

        public THQuaternion(float x, float y, float z, float w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public THQuaternion(THVector3 vector, float w)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
            this.w = w;
        }

        #region Overrides
        public static THQuaternion operator +(THQuaternion a, THQuaternion b)
        {
            a.x += b.x;
            a.y += b.y;
            a.z += b.z;
            a.w += b.w;

            return a;
        }

        public static THQuaternion operator +(THQuaternion a, float b)
        {
            a.x += b;
            a.y += b;
            a.z += b;
            a.w += b;

            return a;
        }

        public static THQuaternion operator -(THQuaternion a, THQuaternion b)
        {
            a.x -= b.x;
            a.y -= b.y;
            a.z -= b.z;
            a.w -= b.w;

            return a;
        }

        public static THQuaternion operator -(THQuaternion a, float b)
        {
            a.x -= b;
            a.y -= b;
            a.z -= b;
            a.w -= b;

            return a;
        }

        public static THQuaternion operator *(THQuaternion a, THQuaternion b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;
            a.w *= b.w;

            return a;
        }

        public static THQuaternion operator *(THQuaternion a, float b)
        {
            a.x *= b;
            a.y *= b;
            a.z *= b;
            a.w *= b;

            return a;
        }

        public static THQuaternion operator /(THQuaternion a, THQuaternion b)
        {
            a.x /= b.x;
            a.y /= b.y;
            a.z /= b.z;
            a.w /= b.w;

            return a;
        }

        public static THQuaternion operator /(THQuaternion a, float b)
        {
            a.x /= b;
            a.y /= b;
            a.z /= b;
            a.w /= b;

            return a;
        }
        #endregion

        #region Implicit Operators
        public static implicit operator THQuaternion(Quaternion q)
        {
            return new THQuaternion(q.x, q.y, q.z, q.w);
        }

        public static implicit operator Quaternion(THQuaternion q)
        {
            return new Quaternion(q.x, q.y, q.z, q.w);
        }
        #endregion
    }
}
