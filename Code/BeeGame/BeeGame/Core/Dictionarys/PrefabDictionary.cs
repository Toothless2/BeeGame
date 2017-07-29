using System.Collections.Generic;
using UnityEngine;

namespace BeeGame.Core.Dictionaries
{
    /// <summary>
    /// The prefabs available to the game
    /// </summary>
    public static class PrefabDictionary
    {
        /// <summary>
        /// All of the prefabs available to spawn in
        /// </summary>
        private static Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

        /// <summary>
        /// Loads the prefabs into the Dictionary
        /// </summary>
        public static void LoadPrefabs()
        {
            prefabDictionary = Resources.Resources.GetPrefabs();
        }

        /// <summary>
        /// Returns a GameObject in the prefab dictionary
        /// </summary>
        /// <param name="prefab">Name of th prefab to get</param>
        /// <returns>Prefab of the given name</returns>
        public static GameObject GetPrefab(string prefab)
        {
            return prefabDictionary[prefab];
        }
    }
}
