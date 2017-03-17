using UnityEngine;
using BeeGame.Core;
using BeeGame.Serialization;
using BeeGame.Quest;

public class LoadResources : MonoBehaviour
{
    void Start()
    {
        LoadPrefabs.PrefabLoad();
        LoadSprites.SpriteLoad();

        Serialization.Load();

        Quests.SetQuestEvents();
    }

    private void FixedUpdate()
    {
        Serialization.Save();
    }
}
