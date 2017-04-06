using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Enums;

namespace BeeGame.TerrainGeneration.Blocks
{
    [Serializable]
    public class Apiary : Block
    {
        [NonSerialized]
        public GameObject mesh;

        /// <summary>
        /// Gets the gameobject and starts and calls the base constructor
        /// </summary>
        public Apiary() : base()
        {
            mesh = GetGameOject();
        }

        /// <summary>
        /// Returns the game object for the apiary
        /// </summary>
        /// <returns></returns>
        public override GameObject GetGameOject()
        {
            changed = true;
            return PrefabDictionary.GetGameObjectItemFromDictionary("BlockTest");
        }

        /// <summary>
        /// Overrides the normal function as the apiary should not be rendered like a normal block
        /// </summary>
        /// <param name="chunk"><see cref="Chunk"/> that the apiary should be in</param>
        /// <param name="x">X pos if the Apiary</param>
        /// <param name="y">Y pos of the Apiary</param>
        /// <param name="z">Z pos if the Apiary</param>
        /// <param name="meshData"><see cref="MeshData"/> to add its data to</param>
        /// <param name="addToRender">Should the apiary be added to the render? (default = false)</param>
        /// <returns><see cref="MeshData"/> with the Apiary data added to it</returns>
        public override MeshData BlockMeshData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRender = false)
        {
            meshData.useRenderForColData = false;
            return base.BlockMeshData(chunk, x, y, z, meshData, false);
        }

        /// <summary>
        /// Overrides the normal <see cref="Block.IsSolid(BlockDirection)"/> method as the apiary has sides with can bee seen through
        /// </summary>
        /// <param name="direction"><see cref="BlockDirection"/></param>
        /// <returns><see cref="false"/></returns>
        public override bool IsSolid(BlockDirection direction)
        {
            return false;
        }
    }
}
