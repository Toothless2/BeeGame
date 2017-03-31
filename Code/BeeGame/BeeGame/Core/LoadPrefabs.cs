using System;
using System.IO;
using UnityEngine;

namespace BeeGame.Core
{
    public static class LoadPrefabs
    {
        private static object[] prefabs;

        /// <summary>
        /// Loads the prefabs from file into prefab dictionary as GameObjects
        /// </summary>
        public static void PrefabLoad()
        {
            //loads all prefabs in the Resources/Prefabs folder into memory
            prefabs = UnityEngine.Resources.LoadAll("Prefabs");

            //adds each prefab to the prefab dictionary so it can be used by the game
            foreach (GameObject item in prefabs)
            {
                PrefabDictionary.AddToPrefabDictionary(item.name, item);
            }
        }
    }
}