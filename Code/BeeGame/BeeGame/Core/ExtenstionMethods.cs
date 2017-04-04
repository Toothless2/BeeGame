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

        public static Vector2 ToTHVector2(this THVector2 _vector2)
        {
            return new THVector2(_vector2);
        }

        public static Vector2 ToUnityVector2(this THVector2 _thVector2)
        {
            return new Vector2(_thVector2.x, _thVector2.y);
        }

        public static Vector3[] ToUnityVector3Array(this THVector3[] vec3)
        {
            Vector3[] vec3u = new Vector3[vec3.Length];

            for (int i = 0; i < vec3u.Length; i++)
            {
                vec3u[i] = vec3[i];
            }

            return vec3u;
        }

        public static Vector2[] ToUnityVector2Array(this THVector2[] vec2)
        {
            Vector2[] vec2u = new Vector2[vec2.Length];

            for (int i = 0; i < vec2u.Length; i++)
            {
                vec2u[i] = vec2[i];
            }

            return vec2u;
        }

        public static THVector2[] ToTHVector2Array(this Vector2[] vec2)
        {
            THVector2[] vec2u = new THVector2[vec2.Length];

            for (int i = 0; i < vec2u.Length; i++)
            {
                vec2u[i] = vec2[i];
            }

            return vec2u;
        }

        public static List<THVector2> ToTHVector2List(this List<Vector2> vec2)
        {
            List<THVector2> vec2u = new List<THVector2>();

            for (int i = 0; i < vec2u.Count; i++)
            {
                vec2u.Add(vec2[i]);
            }

            return vec2u;
        }
    }
}
