using System.Collections.Generic;
using BeeGame.Items;
using BeeGame.Enums;

namespace BeeGame.Core
{
    public static class ItemDictionary
    {
        private static Dictionary<string, Item> items = new Dictionary<string, Item>()
        {
            {"0", AddItem("0", ItemType.ITEM, "Test Item", "Test \n Item", false, 3, "Bee", "Test_Item") },
            {"1", AddItem("1", ItemType.BEE, "Bee", "Bee", false, 1, "Bee", "Bee") },
            {"2", AddItem("2", ItemType.ITEM, "Chest", "Chest", true, 64, "Chest", "Chest") },
            {"3", AddItem("3", ItemType.ITEM, "Apiary", "Apiary", true, 16, "Apiary", "Apiary") },
            {"4", AddItem("4", ItemType.ITEM, "Honey Coumb", "Contains Honey", false, 64, "Comb", "HoneyComb") }
        };

        /// <summary>
        /// Returns an item from the dictionary
        /// </summary>
        /// <param name="itemId">Items ID</param>
        /// <returns><see cref="Item"/></returns>
        public static Item GetItem(string itemId)
        {
            if(items.Count > 0)
            {
                return items[itemId];
            }
            else
            {
                return new Item();
            }            
        }

        /// <summary>
        /// Adds and item to the item dictionary
        /// </summary>
        /// <param name="_itemId">Id of the item</param>
        /// <param name="_itemType"><see cref="ItemType"/></param>
        /// <param name="_itemName">Name of the item</param>
        /// <param name="_itemDescription">Item's description</param>
        /// <param name="_isPlaceable">Can the item be placed</param>
        /// <param name="_maxStackCount">Max stack count of the item</param>
        /// <param name="_objectName">Name of the items game object</param>
        /// <param name="_spriteName">Name of the items sprite</param>
        /// <returns>Item with data</returns>
        static Item AddItem(string _itemId, ItemType _itemType, string _itemName, string _itemDescription, bool _isPlaceable, int _maxStackCount, string _objectName, string _spriteName)
        {
            Item newItem = new Item();

            newItem.itemId = _itemId;
            newItem.itemType = _itemType;
            newItem.name = _itemName;
            newItem.description = _itemDescription;
            newItem.isPlaceable = _isPlaceable;
            newItem.maxStackCount = _maxStackCount;

            if(_maxStackCount > 1)
            {
                newItem.isStackable = true;
            }
            else
            {
                newItem.isStackable = false;
            }

            newItem.objectName = _objectName;
            newItem.spriteName = _spriteName;

            return newItem;
        }
    }
}