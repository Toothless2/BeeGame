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
            CraftingRecipies.AddShaplessRecipie(new object[] { new Grass(), 1 }, new Dirt());
            CraftingRecipies.AddShaplessRecipie(new object[] { new Wood(), 4, new Leaves(), 2 }, new Apiary());

            CraftingRecipies.GetShaplessRecipeResult(new Item[] { new Leaves(), new Wood(), new Wood(), new Wood(), new Leaves(), new Wood()});
        }
    }
}
