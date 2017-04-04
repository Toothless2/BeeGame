using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Enums;

namespace BeeGame.TerrainGeneration.Blocks
{
    public class Apiary : Block
    {
        Mesh mesh;

        public Apiary() : base()
        {
            mesh = PrefabDictionary.GetGameObjectItemFromDictionary("BlockTest").gameObject.GetComponent<MeshFilter>().sharedMesh;
        }

        public override Mesh GetMesh()
        {
            return mesh;
        }

        public override MeshData BlockMeshData(Chunk chunk, int x, int y, int z, MeshData meshData)
        {
            meshData.AddCompliexMesh(mesh, x, y, z, 1);

            return meshData;
        }

        public override bool IsSolid(BlockDirection direction)
        {
            return false;
        }
    }
}
