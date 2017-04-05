using UnityEngine;
using BeeGame.Core;
using BeeGame.Serialization;
using BeeGame.Quest;

public class LoadResources : MonoBehaviour
{
    private static int currentTime = 0;
    private static int saveWaitTime = 1000;

    void Awake()
    {
        LoadPrefabs.PrefabLoad();

        Serialization.SaveWorld();
        //LoadSprites.SpriteLoad();

        //Serialization.Load();

        //Quests.SetQuestEvents();
    }

    //private void FixedUpdate()
    //{
    //    //\todo when pause menu is implemented save when it is open
    //    if (currentTime > saveWaitTime)
    //    {
    //        currentTime = 0;
    //        Serialization.Save();
    //    }

    //    currentTime++;
    //}
}
