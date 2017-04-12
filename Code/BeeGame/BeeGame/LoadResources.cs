using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using BeeGame.Core;

namespace BeeGame
{
    public class LoadResources : MonoBehaviour
    {
        void Awake()
        {
            SpriteDictionary.LoadSprites();
            PrefabDictionary.LoadPrefabs();
            //Thread.CurrentThread.Name = "Bee Game Main Thread";
        }
    }
}
