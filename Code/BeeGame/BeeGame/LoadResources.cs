using UnityEngine;
using BeeGame.Core.Dictionaries;

namespace BeeGame
{
    /// <summary>
    /// Loads all of the resources in the game
    /// </summary>
    public class LoadResources : MonoBehaviour
    {
        /// <summary>
        /// Loads the sprites and prefab dictionarys
        /// </summary>
        void Awake()
        {
            Serialization.Serialization.MakeDirectorys();

            Serialization.Serialization.LoadPlayerPosition(GameObject.Find("Player").GetComponent<Transform>());
            Serialization.Serialization.LoadQuests();

            SpriteDictionary.LoadSprites();
            PrefabDictionary.LoadPrefabs();
        }

        int delay = 0;

        private void Update()
        {
            if (delay++ >= 1000)
                Serialization.Serialization.SaveQuests();
        }
    }
}