using System;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Items;
using BeeGame.Core.Enums;
using BeeGame.Terrain.Chunks;
using BeeGame.Core.Dictionaries;

namespace BeeGame.Blocks
{
    /// <summary>
    /// The Workbench <see cref="Block"/> class
    /// </summary>
    [Serializable]
    public class CraftingTable : Block
    {
        #region Data
        /// <summary>
        /// The <see cref="GameObject"/> for this block
        /// </summary>
        [NonSerialized]
        private GameObject myGameobject;

        /// <summary>
        /// This block's ID
        /// </summary>
        public new static int ID => 9;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public CraftingTable() : base("Workbench")
        {
            usesGameObject = true;
        }
        #endregion

        #region Crafting
        /// <summary>
        /// Makes a shaped crafting recipe from the given items and return if it is a recipe
        /// </summary>
        /// <param name="items">Items to make the recipe from</param>
        /// <returns>A <see cref="Item"/> if the recipe exists</returns>
        public Item ReturnShapedRecipieItem(Item[] items)
        {
            var recipe = "";

            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    recipe += "0:";
                    continue;
                }

                recipe += $"{items[i].GetItemID()}:";
            }

            return ReturnShapedRecipieItem(recipe);
        }

        public virtual Item ReturnShapelessRecipieItem(Item[] items)
        {
            return CraftingRecipies.GetShaplessRecipieResult(items);
        }

        /// <summary>
        /// Returns a crafting recipe from a given recipe
        /// </summary>
        /// <param name="recipe"></param>
        /// <returns>A <see cref="Item"/> if the recipe exists</returns>
        /// <remarks>
        /// Virtual incase needs to be overriden by a different crafting system
        /// </remarks>
        public virtual Item ReturnShapedRecipieItem(string recipe)
        {
            return BeeGame.Core.Dictionaries.CraftingRecipies.GetShapedRecipeItem(recipe);
        }
        #endregion

        #region Block Overrides
        /// <summary>
        /// Toggles the <see cref="CraftingTableInventory"/> for the block
        /// </summary>
        /// <param name="inv"></param>
        /// <returns></returns>
        public override bool InteractWithBlock(Inventory.Inventory inv)
        {
            myGameobject.GetComponent<Inventory.BlockInventory.CraftingTableInventory>().myblock = this;
            myGameobject.GetComponent<Inventory.BlockInventory.CraftingTableInventory>().ToggleInventory(inv);
            return true;
        }

        /// <summary>
        /// Returns this <see cref="Block"/>s game object
        /// </summary>
        /// <returns></returns>
        public override GameObject GetGameObject()
        {
            return PrefabDictionary.GetPrefab("CraftingTable");
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
                myGameobject = UnityEngine.Object.Instantiate(PrefabDictionary.GetPrefab("CraftingTable"), new THVector3(x, y, z) + chunk.chunkWorldPos, Quaternion.identity, chunk.transform);
            }
            return base.BlockData(chunk, x, y, z, meshData, true);
        }

        /// <summary>
        /// Breaks the <see cref="Block"/>
        /// </summary>
        /// <param name="pos">Positon of the <see cref="Block"/></param>
        public override void BreakBlock(THVector3 pos)
        {
            //* removes the game object
            UnityEngine.Object.Destroy(myGameobject);
            //* removes the collision mesh from the chunk
            base.BreakBlock(pos);
        }

        /// <summary>
        /// Returns the sprite for the <see cref="Item"/>
        /// </summary>
        /// <returns><see cref="Sprite"/> for this <see cref="Item"/></returns>
        public override Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("CraftingTable");
        }

        /// <summary>
        /// Returns the texture for the apiary <see cref="Block"/>
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
        #endregion

        #region Overrides
        /// <summary>
        /// Returns the ID of the Item
        /// </summary>
        /// <returns><see cref="ID"/></returns>
        public override int GetHashCode()
        {
            return ID;
        }
        #endregion
    }
}
