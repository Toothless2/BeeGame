using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Core;
using BeeGame.TerrainGeneration.Blocks;

namespace BeeGame.TerrainGeneration
{
    //components requred to be on the game object for the script to work (when the script is added theise 3 things will be added automaticaly)
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        /// <summary>
        /// 3D arry of all of the <see cref="Block"/>s in the chunk
        /// </summary>
        private Block[,,] blocks;
        /// <summary>
        /// How many blocks in each direction should the chunk have
        /// </summary>
        public static int chunkSize = 16;
        /// <summary>
        /// Should the chunk be updated (if the chunk has been changed set to true)
        /// </summary>
        public bool update = true;

        private MeshFilter filter;
        private MeshCollider collider;

        void Start()
        {
            filter = gameObject.GetComponent<MeshFilter>();
            collider = gameObject.GetComponent<MeshCollider>() as MeshCollider;

            blocks = new Block[chunkSize, chunkSize, chunkSize];

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        blocks[x, y, z] = new Blocks.Air();
                    }
                }
            }

            blocks[2, 3, 1] = new Grass();
            blocks[1, 3, 1] = new Apiary();
            blocks[2, 2, 1] = new Block();

            UpdateChunk();
        }
        
        void Update()
        {

        }

        public Block GetBlock(int x, int y, int z)
        {
            if(x >= 0 && y >= 0 && z >= 0)
            {
                return blocks[x, y, z];
            }
            else
            {
                return new Air();
            }
        }

        /// <summary>
        /// Updates the chunk, and builds the <see cref="MeshData"/>
        /// </summary>
        void UpdateChunk()
        {
            MeshData mesh = new MeshData();

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        mesh= blocks[x, y, z].BlockMeshData(this, x, y, z, mesh);
                    }
                }
            }

            RenderMesh(mesh);
            ColliderMesh(mesh);
        }

        /// <summary>
        /// Renders the given mesh
        /// </summary>
        void RenderMesh(MeshData mesh)
        {
            filter.mesh.Clear();
            filter.mesh.vertices = mesh.verts.ToArray().ToUnityVector3Array();
            filter.mesh.triangles = mesh.tris.ToArray();
            
            filter.mesh.uv = mesh.uv.ToArray().ToUnityVector2Array();

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        if(blocks[x, y, z].GetMesh() != null)
                        {
                            filter.mesh.subMeshCount = 2;

                            switch (blocks[x, y, z])
                            {
                                case Apiary a:
                                    filter.mesh.SetTriangles(mesh.GetCorrectedTriangles(a.GetMesh(), x, y, z), 1);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
            filter.mesh.RecalculateNormals();
        }

        void ColliderMesh(MeshData mesh)
        {
            gameObject.GetComponent<MeshCollider>().sharedMesh = filter.mesh;
        }
    }
}
