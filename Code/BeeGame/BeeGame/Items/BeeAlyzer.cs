using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Inventory;
using UnityEngine;
using BeeGame.Core.Dictionaries;

namespace BeeGame.Items
{
    [Serializable]
    public class BeeAlyzer : Item
    {
        #region Data
        public new int ID = 13;

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

        private Item[] itemsInInventory;

        [NonSerialized]
        public GameObject myInventory;
        #endregion

        #region Constructors
        public BeeAlyzer() : base("BeeAlyzer")
        {
        }
        #endregion

        #region ItemInventory
        public virtual void OpenItemInvnetory(Inventory.Inventory playerInventory)
        {
            myInventory = (GameObject)UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/BeeAlyzerInventory"));

            myInventory.GetComponent<BeeAlyzerInventory>().ToggleInventory(playerInventory);
            myInventory.GetComponent<BeeAlyzerInventory>().myblock = new Blocks.Dirt();
        }

        public virtual void CloseItemInventory(Item[] itemsInInventory)
        {
            this.itemsInInventory = itemsInInventory;

            UnityEngine.Object.Destroy(myInventory);

            myInventory = null;
        }
        #endregion

        #region Item Overrides
        public override void InteractWithItem(Inventory.Inventory playerInventory)
        {
            MonoBehaviour.print("Interact With Me!");
            OpenItemInvnetory(playerInventory);
        }

        public override bool InteractWithObject()
        {
            return true;
        }

        public override Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("BeeAlyzer");
        }

        public override string GetItemID()
        {
            return $"{GetHashCode()}";
        }
        #endregion

        #region Overrides
        public override int GetHashCode()
        {
            return ID;
        }
        #endregion
    }
}
