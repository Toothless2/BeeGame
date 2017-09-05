using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BeeGame.Core.Dictionaries;
using BeeGame.Inventory;

namespace BeeGame.Items
{
    [System.Serializable]
    public class QuestBook : BeeAlyzer
    {
        public static new int ID = 15;
        public override int maxStackCount
        {
            get
            {
                return 1;
            }
        }
        public override bool placeable
        {
            get
            {
                return false;
            }
        }

        public QuestBook(): base("Quest Book")
        {

        }

        public override void OpenItemInvnetory(Inventory.Inventory playerInventory = null)
        {
            if (myInventory == null)
            {
                //* makes the inventory
                myInventory = (GameObject)UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/QuestBookInventory"));

                //* opens the inventory and gives it the players inventory
                myInventory.GetComponent<QuestBookInventory>().ToggleInventory(playerInventory);
                myInventory.GetComponent<QuestBookInventory>().myItem = this;
            }
            else
            {
                myInventory = null;
            }
        }

        public override Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("QuestBook");
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
