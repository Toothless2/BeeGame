using UnityEngine;
using BeeGame.Core;

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
            SpriteDictionary.LoadSprites();
            PrefabDictionary.LoadPrefabs();
        }
    }
}
