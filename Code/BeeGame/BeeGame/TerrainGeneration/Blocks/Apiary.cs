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

        public Apiary() : base()
        {
            mesh = GetGameOject();
        }

        public override GameObject GetGameOject()
        {
            changed = true;
            return PrefabDictionary.GetGameObjectItemFromDictionary("BlockTest");
        }

        public override MeshData BlockMeshData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRender = false)
        {
            return base.BlockMeshData(chunk, x, y, z, meshData, false);
        }

        public override bool IsSolid(BlockDirection direction)
        {
            return false;
        }
    }
}
