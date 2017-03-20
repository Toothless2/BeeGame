using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame.Items;
using BeeGame.Inventory;

namespace BeeGame.Blocks
{
    /// <summary>
    /// Interface between GameObject and Block data as MonoBehaviour dreived classes are Non-Serializable using c# BinaryFormatter
    /// </summary>
    public class BlockGameObjectInterface : MonoBehaviour
    {
        #region Common Properties
        /// <summary>
        /// This blocks block data
        /// </summary>
        public Block block;

        /// <summary>
        /// Updates the <see cref="Block"/> that this GameObject is reprsenting
        /// </summary>
        /// <param name="item">Item this this block is</param>
        /// <param name="_position">positon of the block</param>
        public void UpdateBlockData(Item item, THVector3 _position)
        {
            block = new Block();
            block.item = item;
            block.position = _position;
            block.item.stackCount = 1;
        }

        /// <summary>
        /// Overload that can be used if <see cref="Block"/> possibly contains items eg a chest
        /// </summary>
        /// <param name="blockData">Blocks data</param>
        public void UpdateBlockData(Block blockData)
        {
            block = blockData;
            block.item.stackCount = 1;
        }

        /// <summary>
        /// Returns the block's block data
        /// </summary>
        /// <returns><see cref="Block"/></returns>
        public Block ReturnBlockData()
        {
            return block;
        }

        /// <summary>
        /// Returns the blocks item data
        /// </summary>
        /// <returns><see cref="Block.item"/></returns>
        public Item ReturnItemData()
        {
            return block.item;
        }

        /// <summary>
        /// Returns the blocks GameObject
        /// </summary>
        /// <returns>GameObject</returns>
        public GameObject GetBlockGameobject()
        {
            block.item.UpdateSpriteAndObject();
            return block.item.itemGameobject;
        }

        /// <summary>
        /// Removes this script and adds the <see cref="ItemGameObjectInterface"/> onto the game object
        /// </summary>
        public void DestroyBlock()
        {
            Serialization.Serialization.RemoveFromSaveBlocks(gameObject);
            EmptyBlockInventory(gameObject);

            for (int i = this.GetComponents<Component>().Length - 1; i >= 3; i--)
            {
                Destroy(GetComponents<Component>()[i]);
            }

            gameObject.AddComponent<ItemGameObjectInterface>().UpdateItemData(ReturnItemData());
            gameObject.tag = "Item";
            gameObject.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            gameObject.layer = LayerMask.NameToLayer("NoCollisions");
            Destroy(this);
        }

        /// <summary>
        /// Emptys the blocks inventory
        /// </summary>
        /// <param name="_gameObject">GameoObject to destroy</param>
        void EmptyBlockInventory(GameObject _gameObject)
        {
            if (_gameObject.GetComponent<ChestInventory>())
            {
                _gameObject.GetComponent<ChestInventory>().ChestBroken();
            }
        }
        #endregion

        #region Chest Properties
        /// <summary>
        /// updates the chest inventory data
        /// </summary>
        /// <param name="items"><see cref="Item"/></param>
        public void UpdateChestData(Item[] items)
        {
            block.inventoryItems = new Item[items.Length];
            block.inventoryItems = items;
        }

        /// <summary>
        /// Returns the items currently in the chests inventory
        /// </summary>
        /// <returns><see cref="Item[]"/></returns>
        public Item[] ApplyItemArray()
        {
            return block.inventoryItems;
        }

        /// <summary>
        /// updates <see cref="Block.inventoryItems"/>
        /// </summary>
        /// <param name="items"><see cref="Item[]"/></param>
        public void UpdateItemArray(Item[] items)
        {
            if (items != null)
            {
                block.inventoryItems = new Item[items.Length];
                block.inventoryItems = items;
            }
        }
        #endregion
    }
}
