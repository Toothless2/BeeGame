using UnityEngine;
using BeeGame.Core;
using BeeGame.Serialization;

public class LoadResources : MonoBehaviour
{
    void Start()
    {
        LoadPrefabs.PrefabLoad();
        LoadSprites.SpriteLoad();

        Serialization.Load();
    }

    private void FixedUpdate()
    {
        Serialization.Save();
    }
}
