using UnityEngine;
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
        }

        private void Update()
        {
            //var temp = Quest.Quests.ReturnCompleatedQuests();

            //if(temp.Count > 0)
            //{
            //    foreach (var item in temp)
            //    {
            //        Quest.Quests.ClaimQuest(item.Key);
            //    }
            //}
        }
    }
}
