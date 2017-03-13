using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame.Items;
using BeeGame.Core;
using BeeGame.Enums;
using System;

public class SpawnItem : MonoBehaviour
{
    BeeSpecies[] species = new BeeSpecies[4] { BeeSpecies.AGRARIAN, BeeSpecies.AUSTER, BeeSpecies.AVENGING, BeeSpecies.BOGGY };
    float[] weights = new float[4] { 0.5f, 0.1f, 0.2f, 0.2f };

    void Start()
    {
        //Item item = new Item("1");
        //item.ApplyDefaultBeeData(BeeSpecies.FOREST);
        //item.SetBeeType(BeeType.PRINCESS);

        //GameObject temp = Instantiate(item.itemGameobject);
        //temp.GetComponent<ItemGameObjectInterface>().item = item;
        //QuestManager.beeMade += Test;
    }

    void Test(string thign)
    {
        print(thign);
    }

    void Update()
    {
        //print(GetRandomItem());
    }

    float Rand(float min, float max)
    {
        UnityEngine.Random rand = new UnityEngine.Random();
        float number = (max - min);

        return UnityEngine.Random.Range(min, number);
    }

    BeeSpecies GetRandomItem()
    {
        var totalWeight = 0f;

        for (int i = 0; i < weights.Length; i++)
        {
            totalWeight += weights[i];
        }

        var randomNum = Rand(0, totalWeight);
        Math.Round(randomNum, 2);
        var weightSum = 0f;

        for (int i = 0; i < species.Length; i++)
        {
            weightSum += weights[i];

            if ((float)randomNum <= weightSum)
            {
                return species[i];
            }
        }

        return 0;
    }
}
