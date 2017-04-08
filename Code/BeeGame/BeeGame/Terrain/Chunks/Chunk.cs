using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;
using BeeGame.Terrain.Blocks;
using BeeGame.Terrain.LandGeneration;

namespace BeeGame.Terrain.Chunks
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        public static int chunkSize = 16;

        public Block[,,] blocks = new Block[chunkSize, chunkSize, chunkSize];

        public bool update = false;
        public bool rendered;

        public World world;
        public ChunkWorldPos chunkWorldPos;

        private MeshData mesh;

        private MeshFilter filter;
        private MeshCollider meshCollider;

        void Start()
        {
            filter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
        }

        void Update()
        {
            if(update)
            {
                update = false;
                UpdateChunk();
            }
        }

        #region Get/Set Blocks
        public Block GetBlock(int x, int y, int z)
        {
            if (InRange(x) && InRange(y) && InRange(z))
            {
                return blocks[x, y, z];
            }
            return world.GetBlock(chunkWorldPos.x + x, chunkWorldPos.y + y, chunkWorldPos.z + z);
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            if (InRange(x) && InRange(y) && InRange(z))
            {
                blocks[x, y, z] = block;
                return;
            }
            world.SetBlock(chunkWorldPos.x + x, chunkWorldPos.y + y, chunkWorldPos.z + z, block);
        }

        public static bool InRange(int i)
        {
            if (i < 0 || i >= chunkSize)
                return false;
            return true;
        }
        #endregion

        public void SetBlocksUnmodified()
        {
            foreach (var block in blocks)
            {
                block.changed = false;
            }
        }

        void UpdateChunk()
        {
            rendered = true;
            mesh = new MeshData();

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        blocks[x, y, z].UpdateBlock(x, y, z, this);
                        mesh = blocks[x, y, z].BlockData(this, x, y, z, mesh);
                    }
                }
            }
            
            RenderMesh(mesh);
        }

        void RenderMesh(MeshData meshData)
        {
            filter.mesh.Clear();
            filter.mesh.name = "Render Mesh";
            filter.mesh.vertices = meshData.verts.ToArray();
            filter.mesh.triangles = meshData.tris.ToArray();

            filter.mesh.uv = meshData.uv.ToArray();

            filter.mesh.RecalculateNormals();

            if (meshData.shareMeshes)
            {
                meshCollider.sharedMesh = filter.mesh;
                return;
            }

            ColliderMesh(meshData);
        }

        void ColliderMesh(MeshData meshData)
        {
            Mesh mesh = new Mesh()
            {
                name = "Collider Mesh",
                vertices = meshData.colVerts.ToArray(),
                triangles = meshData.colTris.ToArray()
            };

            mesh.RecalculateNormals();

            meshCollider.sharedMesh = mesh;
        }
    }
}
