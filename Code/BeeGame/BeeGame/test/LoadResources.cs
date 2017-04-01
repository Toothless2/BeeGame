using UnityEngine;
using BeeGame.Core;
using BeeGame.Serialization;
using BeeGame.Quest;

public class LoadResources : MonoBehaviour
{
    private static int currentTime = 0;
    private static int saveWaitTime = 3000;

    void Awake()
    {
        LoadPrefabs.PrefabLoad();
        LoadSprites.SpriteLoad();

        Serialization.Load();

        Quests.SetQuestEvents();
    }

    private void FixedUpdate()
    {
        if (currentTime > saveWaitTime)
        {
            currentTime = 0;
            Serialization.Save();
        }

        currentTime++;
    }
}
