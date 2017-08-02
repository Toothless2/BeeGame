using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Core.Dictionaries;
using BeeGame.Items;
using UnityEngine;

namespace BeeGame.Quest
{
    public class QuestBook : Item
    {
        public override int maxStackCount => 1;

        public QuestBook() : base("Quest Book")
        {

        }

        public override bool InteractWithObject()
        {
            return true;
        }

        public override Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("TestSprite");
        }



        /// <summary>
        /// Returns the hashcode for <see cref="this"/> <see cref="Item"/>
        /// </summary>
        /// <returns>10</returns>
        public override int GetHashCode()
        {
            return 10;
        }
    }
}
