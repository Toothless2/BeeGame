using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using BeeGame.Core.Dictionarys;
using BeeGame.Items;
using BeeGame.Blocks;

namespace BeeGame
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            CraftingRecipies.AddShapedRecipie(new object[] { "XXX", "XXX", "XXX", "X", Dirt.ID }, new Grass());
        }
    }
}
