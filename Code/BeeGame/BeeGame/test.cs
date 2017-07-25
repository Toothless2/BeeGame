using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using BeeGame.Core.Dictionaries;
using BeeGame.Items;
using BeeGame.Blocks;
using BeeGame.Core;

namespace BeeGame
{
    public class Test : MonoBehaviour
    {
        private void Start()
        {
            CraftingRecipies.AddShapedRecipie(new object[] { "   ", " X ", "   ", "X", Dirt.ID }, new Grass());
            CraftingRecipies.AddShaplessRecipie(new object[] { new Grass(), 1 }, new Dirt());

            Events.shapedRecipieCrafted += Print;
        }
        public void Print(Item item) => print(item.GetItemID());
    }
}
