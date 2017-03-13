using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace BeeGame.Core
{
    public static class PrefabDictionary
    {
        /// <summary>
        /// Stores all prefabs that can be loaded by the game
        /// </summary>
        private static Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

        /// <summary>
        /// Adds a prefab to the dictionary
        /// </summary>
        /// <param name="objectName">Name of prefab</param>
        /// <param name="objectToAdd">Prefab GameObject</param>
        public static void AddToPrefabDictionary(string objectName, GameObject objectToAdd)
        {
            if (GetGameObjectItemFromDictionary(objectName) == null)
            {
                prefabs.Add(objectName, objectToAdd);
            }
            else
            {
                prefabs[objectName] = objectToAdd;
            }
        }

        /// <summary>
        /// Returns a GameObject from given name
        /// </summary>
        /// <param name="objectName">item object name</param>
        /// <returns>GameObject</returns>
        public static GameObject GetGameObjectItemFromDictionary(string objectName)
        {
            if (objectName != null)
            {
                if (prefabs.ContainsKey(objectName))
                {
                    return prefabs[objectName];
                }
            }
            return null;
        }
    }
}