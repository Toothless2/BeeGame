using System.Collections.Generic;
using UnityEngine;

namespace BeeGame.Core
{
    public static class PrefabDictionary
    {
        private static Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

        public static void LoadPrefabs()
        {
            prefabDictionary = Resources.Resources.GetPrefabs();
        }

        public static GameObject GetPrefab(string prefab)
        {
            return prefabDictionary[prefab];
        }
    }
}
