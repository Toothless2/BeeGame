using System;
using System.IO;
using UnityEngine;

namespace BeeGame.Core
{
    public static class LoadPrefabs
    {
        private static string prefabPath;
        private static string[] splitCharacters = new string[2] { "/", "." };

        /// <summary>
        /// Loads the prefabs from file into prefab dictionary as GameObjects
        /// </summary>
        public static void PrefabLoad()
        {
            prefabPath = Application.dataPath + "/Resources/Prefabs/";

            //finds all .prefab files in the directory
            foreach (string s in Directory.GetFiles(prefabPath, "*.prefab"))
            {
                string prefabName;
                string[] splitPath;

                splitPath = s.Split(splitCharacters, StringSplitOptions.None);

                prefabName = splitPath[splitPath.Length - 2];

                //loads found prefab into the profab dictionary
                PrefabDictionary.AddToPrefabDictionary(prefabName, (GameObject)Resources.Load("Prefabs/" + prefabName, typeof(GameObject)));
            }
        }
    }
}