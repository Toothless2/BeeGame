﻿using System;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Terrain.Chunks;
using BeeGame.Core.Enums;
using BeeGame.Items;
using BeeGame.Inventory;
using BeeGame.Core.Dictionaries;

namespace BeeGame.Blocks
{
    /// <summary>
    /// Chest Block
    /// </summary>
    [Serializable]
    public class Chest : Block
    {
        #region Data
        /// <summary>
        /// Chest model for when it is placed
        /// </summary>
        [NonSerialized]
        private GameObject myGameobject;

        public new static int ID => 8;
        #endregion

        #region Constructors
        /// <summary>
        /// Makes a new chest from a parameterless constructor
        /// </summary>
        public Chest() : base("Chest")
        {
            usesGameObject = true;
        }
        #endregion

        #region Block Overrides
        /// <summary>
        /// Gets the gme object for this chest
        /// </summary>
        /// <returns>The chest game object</returns>
        public override GameObject GetGameObject()
        {
            return PrefabDictionary.GetPrefab("Chest");
        }

        /// <summary>
        /// Returns the texture for the chest <see cref="Block"/>
        /// </summary>
        /// <param name="direction"><see cref="Direction"/> of thhe desired face</param>
        /// <returns><see cref="Tile"/> with the textture coordinates of the <see cref="Block"/> texture</returns>
        /// <remarks>
        /// Returns a transparent texture as the chest model already has a texture applied
        /// </remarks>
        public override Tile TexturePosition(Direction direction)
        {
            return new Tile() { x = 0, y = 9 };
        }

        /// <summary>
        /// The data that this block adds to the mesh
        /// </summary>
        /// <param name="chunk">Chunk the block is in</param>
        /// <param name="x">X pos of the block</param>
        /// <param name="y">Y pos of the block</param>
        /// <param name="z">Z pos of the block</param>
        /// <param name="meshData">meshdata to add to chunk</param>
        /// <param name="addToRenderMesh">should the block also be added to the render mesh not just the collision mesh</param>
        /// <returns>Given <paramref name="meshData"/> with this blocks data added to it</returns>
        /// <remarks>
        /// Only adds to the collision mesh as the model is handlled by the unity prefab system
        /// </remarks>
        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            if (myGameobject == null)
            {
                myGameobject = UnityEngine.Object.Instantiate(PrefabDictionary.GetPrefab("Chest"), new THVector3(x, y, z) + chunk.chunkWorldPos, Quaternion.identity, chunk.transform);
                myGameobject.GetComponent<ChestInventory>().inventoryPosition = new THVector3(x, y, z) + chunk.chunkWorldPos;
                myGameobject.GetComponent<ChestInventory>().SetChestInventory(); 
            }
            return base.BlockData(chunk, x, y, z, meshData, true);
        }

        /// <summary>
        /// Breaks the block
        /// </summary>
        /// <param name="pos">Position of the block</param>
        public override void BreakBlock(THVector3 pos)
        {
            //* removes the blocks blocks inventory save file and destroys the game object
            Serialization.Serialization.DeleteFile(myGameobject.GetComponent<ChestInventory>().inventoryName);
            UnityEngine.Object.Destroy(myGameobject);
            //* removes the collision mesh from the chunk
            base.BreakBlock(pos);
        }

        /// <summary>
        /// Gets the Chest <see cref="Sprite"/>
        /// </summary>
        /// <returns>The Chest <see cref="Sprite"/></returns>
        public override Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("Chest");
        }
        #endregion

        #region Inventory Suff
        /// <summary>
        /// Opens the <see cref="ChestInventory"/> when clicked on
        /// </summary>
        /// <param name="inv">Inventory that the chest is interacting with</param>
        /// <returns>true</returns>
        public override bool InteractWithBlock(BeeGame.Inventory.Inventory inv)
        {
            myGameobject.GetComponent<ChestInventory>().ToggleInventory(inv);
            return true;
        }
        #endregion
        
        #region Overrides
        /// <summary>
        /// Gets the ID of the <see cref="Block"/>
        /// </summary>
        /// <returns>8</returns>
        public override int GetHashCode()
        {
            return ID;
        }

        /// <summary>
        /// Returns the <see cref="Block"/> name and ID formatted nicely
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{itemName}\nID{GetItemID()}";
        }
        #endregion
    }
}
