using System;
using BeeGame.Core;
using BeeGame.Inventory;
using UnityEngine;
using BeeGame.Items;
using BeeGame.Core.Enums;
using BeeGame.Terrain.Chunks;

namespace BeeGame.Blocks
{
    /// <summary>
    /// Apiary Block
    /// </summary>
    [Serializable]
    public class Apiary : Block
    {
        [NonSerialized]
        private GameObject myGameobject;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Apiary() : base("Apiary")
        {
            usesGameObject = true;
        }
        #endregion

        #region Block Overrides
        /// <summary>
        /// Gets the game object for this apiary
        /// </summary>
        /// <returns>THe chest game object</returns>
        public override GameObject GetGameObject()
        {
            return PrefabDictionary.GetPrefab("Apiary");
        }

        /// <summary>
        /// Returns the texture for the apiary <see cref="Block"/>
        /// </summary>
        /// <param name="direction"><see cref="Direction"/> of thhe desired face</param>
        /// <returns><see cref="Tile"/> with the textture coordinates of the <see cref="Block"/> texture</returns>
        /// <remarks>
        /// Returns a trnasparent texture as the chest model already has a texture applied
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
        /// <param name="meshData">meshdata to add to</param>
        /// <param name="addToRenderMesh">should the block also be added to the render mesh not just the collsion mesh</param>
        /// <returns>Given <paramref name="meshData"/> with this blocks data added to it</returns>
        /// <remarks>
        /// Only adds to the colision mesh as the model is handlled by the unity prefab system
        /// </remarks>
        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            if (myGameobject == null)
            {
                myGameobject = UnityEngine.Object.Instantiate(PrefabDictionary.GetPrefab("Apiary"), new THVector3(x, y, z) + chunk.chunkWorldPos, Quaternion.identity, chunk.transform);
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
            Serialization.Serialization.DeleteFile(myGameobject.GetComponent<ApiaryInventory>().inventoryName);
            UnityEngine.Object.Destroy(myGameobject);
            //* removes the collision mesh from the chunk
            base.BreakBlock(pos);
        }
        #endregion

        #region Overrides
        /// <summary>
        /// ID of the item
        /// </summary>
        /// <returns>3</returns>
        public override int GetHashCode()
        {
            return 3;
        }

        /// <summary>
        /// The item name and ID as a string
        /// </summary>
        /// <returns>A nicely formatted string</returns>
        public override string ToString()
        {
            return $"{itemName} \nID: {GetItemID()}";
        }
        #endregion

        /// <summary>
        /// Toggles the <see cref="ApiaryInventory"/> for the block
        /// </summary>
        /// <param name="inv"></param>
        /// <returns></returns>
        public override bool InteractWithBlock(Inventory.Inventory inv)
        {
            myGameobject.GetComponent<ApiaryInventory>().ToggleInventory(inv);
            return true;
        }
    }
}
