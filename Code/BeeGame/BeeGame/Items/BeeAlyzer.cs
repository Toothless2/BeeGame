using System;
using BeeGame.Inventory;
using UnityEngine;
using BeeGame.Core.Dictionaries;

namespace BeeGame.Items
{
    [Serializable]
    public class BeeAlyzer : Item
    {
        #region Data
        /// <summary>
        /// Item ID
        /// </summary>
        public new int ID = 13;

        /// <summary>
        /// 1
        /// </summary>
        public override int maxStackCount
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// False
        /// </summary>
        public override bool placeable
        {
            get
            {
                return false;
            }
        }
        
        /// <summary>
        /// Inventory this item is attached to
        /// </summary>
        [NonSerialized]
        public GameObject myInventory;
        #endregion

        #region Constructors
        public BeeAlyzer() : base("BeeAlyzer")
        {
        }

        /// <summary>
        /// Used of another object wants to use this as a parent class
        /// </summary>
        /// <param name="s">Name of child object</param>
        public BeeAlyzer(string s) : base(s)
        {}
        #endregion

        #region ItemInventory
        /// <summary>
        /// opens and closes the items inventory
        /// </summary>
        /// <param name="playerInventory">Used when opening inventory to give the new inventory the players inventory(<see cref="Inventory.Player_Inventory.PlayerInventory"/>)</param>
        public virtual void OpenItemInvnetory(Inventory.Inventory playerInventory = null)
        {
            if (myInventory == null)
            {
                //* makes the inventory
                myInventory = (GameObject)UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/BeeAlyzerInventory"));

                //* opens the inventory and gives it the players inventory
                myInventory.GetComponent<BeeAlyzerInventory>().ToggleInventory(playerInventory);
                myInventory.GetComponent<BeeAlyzerInventory>().myItem = this;
            }
            else
            {
                myInventory = null;
            }
        }
        #endregion

        #region Item Overrides
        /// <summary>
        /// Tells the rest of the game how to interact with this item
        /// </summary>
        /// <param name="playerInventory"></param>
        public override void InteractWithItem(Inventory.Inventory playerInventory)
        {
            OpenItemInvnetory(playerInventory);
        }

        /// <summary>
        /// this object can be intereacted with
        /// </summary>
        /// <returns></returns>
        public override bool InteractWithObject()
        {
            return true;
        }

        /// <summary>
        /// Returns the items <see cref="Sprite"/>
        /// </summary>
        /// <returns></returns>
        public override Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("BeeAlyzer");
        }

        /// <summary>
        /// Returns the items <see cref="ID"/>
        /// </summary>
        /// <returns></returns>
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
