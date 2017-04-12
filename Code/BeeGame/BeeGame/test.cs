using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace BeeGame
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            Instantiate(BeeGame.Core.PrefabDictionary.GetPrefab("Selector"));
        }
    }
}
