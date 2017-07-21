using UnityEngine;
using BeeGame.Terrain.Chunks;
using BeeGame.Core.Enums;
using BeeGame.Items;
using BeeGame.Core;
using BeeGame.Core.Dictionarys;

namespace BeeGame.Blocks
{
    /// <summary>
    /// Base class for blocks
    /// </summary>
    [System.Serializable]
    public class Block : Item
    {
        #region Data
        public new static int ID = 1;
        /// <summary>
        /// Can this <see cref="Block"/> be broken
        /// </summary>
        public bool breakable = true;
        /// <summary>
        /// Has this block been placed by the player
        /// </summary>
        public bool changed = true;
        /// <summary>
        /// Sets so that blocks can be placed
        /// </summary>
        public override bool placeable => true;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor sets the <see cref="Item.placeable"/> to true
        /// </summary>
        public Block() : base()
        {
            itemName = "Stone";
        }

        /// <summary>
        /// Sets placeabel to true and sets name of the block/item
        /// </summary>
        /// <param name="name">Name of the block/item</param>
        public Block(string name) : base(name)
        {
        }
        #endregion

        #region Item Stuff
        public override Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("Stone");
        }
        #endregion

        #region Update/Break Block
        /// <summary>
        /// Spawns an item with the same texture as the broken block
        /// </summary>
        /// <param name="pos">position to spawn the <see cref="Item"/></param>
        public virtual void BreakBlock(THVector3 pos)
        {
            GameObject go = Object.Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, pos, Quaternion.identity) as GameObject;
            go.GetComponent<ItemGameObject>().item = this;
        }

        /// <summary>
        /// Should this <see cref="Block"/> be updated when the mesh is made
        /// </summary>
        /// <param name="x">X pos if the block</param>
        /// <param name="y">Y pos of the block</param>
        /// <param name="z">Z pos of the block</param>
        /// <param name="chunk">Chunk that the block is in</param>
        public virtual void UpdateBlock(int x, int y, int z, Chunk chunk) { }

        /// <summary>
        /// Can this block be interacted with?
        /// </summary>
        /// <returns>False by default</returns>
        public virtual bool InteractWithBlock(BeeGame.Inventory.Inventory inv)
        {
            return false;
        }
        #endregion
        
        #region Mesh
        /// <summary>
        /// The data that this block adds to the mesh
        /// </summary>
        /// <param name="chunk">Chunk the block is in</param>
        /// <param name="x">X pos of the block</param>
        /// <param name="y">Y pos of the block</param>
        /// <param name="z">Z pos of the block</param>
        /// <param name="meshData">meshdata to add to</param>
        /// <param name="addToRenderMesh">should the block also be added to the render mesh not just the collsion mesh</param>
        /// <returns>Given <paramref name="meshData"/> with this blocks data added to it</returns>
        /// <remarks>
        /// If no data of either collider or render should be added override to return the givn mesh. \n
        /// If only collsion data should be added override to say render mesh false.
        /// </remarks>
        public virtual MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            //* Adds the Top face of the block
            if (!chunk.GetBlock(x, y + 1, z, false).IsSolid(Direction.DOWN))
            {
                meshData = FaceDataUp(x, y, z, meshData, addToRenderMesh);
            }

            //* Adds the Bottom face of the block
            if (!chunk.GetBlock(x, y - 1, z, false).IsSolid(Direction.UP))
            {
                meshData = FaceDataDown(x, y, z, meshData, addToRenderMesh);
            }

            //* Adds the North face of the block
            if (!chunk.GetBlock(x, y, z + 1, false).IsSolid(Direction.SOUTH))
            {
                meshData = FaceDataNorth(x, y, z, meshData, addToRenderMesh);
            }

            //* Adds the South face of the block
            if (!chunk.GetBlock(x, y, z - 1, false).IsSolid(Direction.NORTH))
            {
                meshData = FaceDataSouth(x, y, z, meshData, addToRenderMesh);
            }

            //* Adds the East face of the block
            if (!chunk.GetBlock(x + 1, y, z, false).IsSolid(Direction.WEST))
            {
                meshData = FaceDataEast(x, y, z, meshData, addToRenderMesh);
            }

            //* Adds the West face of the block
            if (!chunk.GetBlock(x - 1, y, z, false).IsSolid(Direction.EAST))
            {
                meshData = FaceDataWest(x, y, z, meshData, addToRenderMesh);
            }

            return meshData;
        }

        /// <summary>
        /// What <see cref="Direction"/>s is this <see cref="Block"/> solid in
        /// </summary>
        /// <param name="direction"><see cref="Direction"/> to check</param>
        /// <returns>Default returns true for all sides</returns>
        public virtual bool IsSolid(Direction direction)
        {
            return true;
        }
        #endregion

        #region Overrides
        /// <summary>
        /// Hascode for the <see cref="Block"/>
        /// </summary>
        /// <returns>1</returns>
        public override int GetHashCode()
        {
            return ID;
        }

        /// <summary>
        /// Returns the <see cref="Block"/> name and Id formatted nicely
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{itemName} \nID: {GetHashCode()}";
        }
        #endregion
    }
}
