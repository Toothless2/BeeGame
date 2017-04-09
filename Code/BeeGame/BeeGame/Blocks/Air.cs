using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Core.Enums;
using BeeGame.Terrain.Chunks;
using UnityEngine;

namespace BeeGame.Blocks
{
    [Serializable]
    public class Air : Block
    {
        public Air() : base() { }

        public override void BreakBlock(Vector3 pos)
        {
            return;
        }

        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addRoRenderMesh = true)
        {
            return meshData;
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }
    }
}
