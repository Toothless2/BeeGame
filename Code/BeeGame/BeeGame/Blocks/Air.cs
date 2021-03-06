﻿using System;
using BeeGame.Core.Enums;
using BeeGame.Terrain.Chunks;
using BeeGame.Core;

namespace BeeGame.Blocks
{
    /// <summary>
    /// Air <see cref="Block"/> is an empty block that does not render and has no collider
    /// </summary>
    [Serializable]
    public class Air : Block
    {
        public new static int ID => 0;

        public Air() : base("Air")
        {
        }

        /// <summary>
        /// No item should be made when air is broken
        /// </summary>
        /// <param name="pos">position to spawn the <see cref="Item"/></param>
        public override void BreakBlock(THVector3 pos)
        {
            return;
        }

        /// <summary>
        /// Returns the given <see cref="MeshData"/> as <see cref="Air"/> does not add anything to the mesh
        /// </summary>
        /// <returns>Given <see cref="MeshData"/></returns>
        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addRoRenderMesh = true)
        {
            return meshData;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"><see cref="Direction"/> wanted to check solid</param>
        /// <returns>false</returns>
        public override bool IsSolid(Direction direction)
        {
            return false;
        }

        /// <summary>
        /// Hashcode acts as the base ID for an item
        /// </summary>
        /// <returns>2</returns>
        public override int GetHashCode()
        {
            return ID;
        }

        /// <summary>
        /// Gets the item name and ID in a nice format
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{itemName} \nID: {GetItemID()}";
        }
    }
}
