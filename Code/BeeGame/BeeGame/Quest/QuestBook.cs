using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Core.Dictionaries;
using BeeGame.Inventory;
using BeeGame.Items;
using UnityEngine;

namespace BeeGame.Quest
{
    public class QuestBook : Item
    {
        public new static int ID => 15;
        public override int maxStackCount => 1;

        public QuestBook() : base("Quest Book")
        {

        }

        public override void InteractWithItem(Inventory.Inventory playerInventory)
        {
            MonoBehaviour.print("hi");
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
