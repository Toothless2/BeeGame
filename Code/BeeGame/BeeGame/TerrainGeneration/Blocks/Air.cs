using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Enums;
using UnityEngine;

namespace BeeGame.TerrainGeneration.Blocks
{
    public class Air : Block
    {
        /// <summary>
        /// Makes the Air constructor and the base constructor
        /// </summary>
        public Air() : base()
        {
        }

        public override MeshData BlockMeshData(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            return meshData;
        }

        /// <summary>
        /// Air is not solid so will return false for all directions
        /// </summary>
        /// <param name="direction"><see cref="BlockDirection"/></param>
        /// <returns><see cref="false"/></returns>
        public override bool IsSolid(BlockDirection direction)
        {
            return false;
        }
    }
}
