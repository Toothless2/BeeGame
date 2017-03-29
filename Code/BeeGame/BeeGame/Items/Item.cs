using System;
using UnityEngine;
using BeeGame.Enums;
using BeeGame.Bee;
using BeeGame.Core;
using BeeGame.Bee.ColourChanger;

namespace BeeGame.Items
{
    /// <summary>
    /// Storage for the Item Data
    /// </summary>
    [Serializable]
    public struct Item
    {
        #region Data
        /// <summary>
        /// The items current positon (only used if not in an inventory
        /// </summary>
        public THVector3 pos;

        /// <summary>
        /// This items <see cref="ItemType"/>
        /// </summary>
        public ItemType itemType;

        #region General Item Data
        /// <summary>
        /// Items ID
        /// </summary>
        public string itemId;
        /// <summary>
        /// Items Name
        /// </summary>
        public string name;
        /// <summary>
        /// Items Description
        /// </summary>
        public string description;
        /// <summary>
        /// Is the item Placeable
        /// </summary>
        public bool isPlaceable;
        /// <summary>
        /// Is the item stackable
        /// </summary>
        public bool isStackable;
        /// <summary>
        /// The max stack count of the item
        /// </summary>
        public int maxStackCount;
        /// <summary>
        /// The current stack count of the item
        /// </summary>
        public int stackCount;

        /// <summary>
        /// Name of the items GameObject
        /// </summary>
        public string objectName;
        /// <summary>
        /// The items GameObjetc
        /// </summary>
        [NonSerialized]
        public GameObject itemGameobject;

        /// <summary>
        /// The items sprite name
        /// </summary>
        public string spriteName;
        /// <summary>
        /// This items Sprite
        /// </summary>
        [NonSerialized]
        public Sprite itemSpriteObject;
        #endregion GeneralItemData

        #region Specific Item Data
        /// <summary>
        /// The Items Bee Data (null by default)
        /// </summary>
        public BeeData? beeItem;
        public HoneyComb? honeyComb;
        #endregion
        #endregion

        #region Common Methods
        /// <summary>
        /// Applies the item's default data and updates the sprite/gameobejct
        /// </summary>
        /// <param name="_itemID">Item ID</param>
        public Item(string _itemID)
        {
            this = ItemDictionary.GetItem(_itemID);
            this.stackCount = 1;
            this.UpdateSpriteAndObject();
        }

        /// <summary>
        /// Constructor here incase it is needed
        /// </summary>
        /// <param name="item"><see cref="Item"/></param>
        public Item(Item item)
        {
            this = item;
            this.UpdateSpriteAndObject();
        }

        /// <summary>
        /// Updates the Item's Sprite and GameObject
        /// </summary>
        public void UpdateSpriteAndObject()
        {
            this.itemGameobject = PrefabDictionary.GetGameObjectItemFromDictionary(objectName);
            this.itemSpriteObject = SpriteDictionary.GetSpriteItemFromDictionary(spriteName);
        }
        #endregion

        #region Bee Stuff
        /// <summary>
        /// Can the bees data be seen in the inventory
        /// </summary>
        /// <returns>true of data can be seen</returns>
        public bool CanSeeBeeData()
        {
            return beeItem.Value.canSeeBeeData;
        }

        /// <summary>
        /// Applies the default <see cref="BeeData"/> to the <see cref="beeItem"/>
        /// </summary>
        /// <param name="dominantSpecies">Dominant <see cref="BeeSpecies"/></param>
        public void ApplyDefaultBeeData(BeeSpecies dominantSpecies)
        {
            beeItem = new BeeData();
            beeItem = BeeDictionarys.GetDefaultBeeData(dominantSpecies);
            
            //updates the Name and ID of the bee
            UpdateItemNameAndIDWithBeeData();
        }

        /// <summary>
        /// Only used when the default bees are made for quests
        /// </summary>
        /// <param name="beetype"><see cref="BeeType"/></param>
        public void SetBeeType(BeeType beetype)
        {
            beeItem?.SetBeeType(beetype);

            //updates the name and ID if the bee
            UpdateItemNameAndIDWithBeeData();
        }

        /// <summary>
        /// Updates the bee data for the item
        /// </summary>
        /// <param name="_beedata"><see cref="BeeData"/></param>
        public void UpdateBeeData(BeeData _beedata)
        {
            //Sets the new bee data
            beeItem = _beedata;

            //Updates the Name and ID of the Bee
            UpdateItemNameAndIDWithBeeData();
        }

        /// <summary>
        /// Returns this items <see cref="BeeData"/>
        /// </summary>
        /// <returns><see cref="BeeData"/></returns>
        public BeeData ReturnBeeData()
        {
            return beeItem.Value;
        }

        /// <summary>
        /// Returns the <see cref="BeeData"/> as text
        /// </summary>
        /// <returns><see cref="BeeData"/> formated to string</returns>
        public string ReturnBeeDataAsText()
        {
            return beeItem.HasValue ? ("Donimant Species : " + beeItem.Value.pSpecies + "\nDominaint BeeType: " + beeItem.Value.beeType + "\nDominant Lifespan: " + beeItem.Value.pLifespan + "\nDominant Fertility: " + beeItem.Value.pFertility + "Dominant Effect: " + beeItem.Value.pEffect + "Dominant Production Speed: " + beeItem.Value.pProdSpeed) : null;
        }
        #endregion

        #region Rename and Update ID
        /// <summary>
        /// Updates this <see cref="Item"/>'s <see cref="name"/> <see cref="itemId"/> with the bee data
        /// </summary>
        void UpdateItemNameAndIDWithBeeData()
        {
            //Updates the Name and ID
            name = (char.ToUpper(beeItem.Value.pSpecies.ToString()[0]).ToString() + beeItem.Value.pSpecies.ToString().ToLower().Substring(1)) + " " + (char.ToUpper(beeItem.Value.beeType.ToString()[0]).ToString() + beeItem.Value.beeType.ToString().ToLower().Substring(1));
            itemId = itemId + "/" + beeItem.GetHashCode().ToString();
        }
        #endregion

        /// <summary>
        /// Overrides the default ToString to make it more useful
        /// </summary>
        /// <returns>The <see cref="Item"/>'s <see cref="name"/> and the <see cref="itemId"/></returns>
        public override string ToString()
        {
            return name + " : " + itemId;
        }

        #region Equality Checking
        /// <summary>
        /// Makes c# happy
        /// </summary>
        /// <param name="obj">Objetc</param>
        /// <returns>true if objetcs are the same</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        
        /// <summary>
        /// Makes a Hashcode for each item that is the same for 2 items with same same data but dirrerent for items with different data
        /// </summary>
        /// <returns>Item HashCode(int)</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                int itemNameValue = 0;
                int itemIdValue = 0;

                //Gives the item name a value depending on the sum of the characters
                if (this.name != null)
                {
                    for (int i = 0; i < this.name.Length; i++)
                    {
                        itemNameValue += this.name[i];
                    }

                    for (int i = 0; i < this.itemId.Length; i++)
                    {
                        itemIdValue += this.itemId[i];
                    }
                }

                //sets the starting hascode numeber
                var hashcode = 13;
                //uses the itemid
                hashcode = itemIdValue;
                //adds on the itemname value
                hashcode += itemNameValue;
                //adds on the bee data of it has a value
                hashcode += beeItem.HasValue ? beeItem.GetHashCode() : 1;
                //add the hashcode of the honey coumb if it has a value
                hashcode += honeyComb.HasValue ? honeyComb.GetHashCode() : 1;
                return hashcode;
            }
        }

        /// <summary>
        /// Overriding equals operator
        /// </summary>
        /// <param name="a">Item 1</param>
        /// <param name="b">Item 2</param>
        /// <returns>true if items are the same (have the same hashcode)</returns>
        public static bool operator ==(Item a, Item b)
        {
            if ((object)a == null && (object)b != null) return false;
            if ((object)a != null && (object)b == null) return false;
            return a.GetHashCode() == b.GetHashCode();
        }

        /// <summary>
        /// Overiding the not equals operator
        /// </summary>
        /// <param name="a">Item a</param>
        /// <param name="b">Item b</param>
        /// <returns>inverse of ==</returns>
        public static bool operator !=(Item a, Item b)
        {
            return !(a == b);
        }
        #endregion
    }
}