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
        /// How many blocks in each direction should the chunk have
        /// </summary>
        public static int chunkSize = 16;
        /// <summary>
        /// 3D arry of all of the <see cref="Block"/>s in the chunk
        /// </summary>
        public Block[,,] blocks = new Block[chunkSize, chunkSize, chunkSize];
        /// <summary>
        /// Should the chunk be updated (if the chunk has been changed set to true)
        /// </summary>
        public bool update = false;
        
        public World world;
        public THVector3 worldPos;

        public bool rendered;
        
        private MeshFilter filter;
        //private MeshCollider collider;

        void Awake()
        {
            filter = gameObject.GetComponent<MeshFilter>();
            //collider = gameObject.GetComponent<MeshCollider>();
        }

        void Update()
        {
            rendered = true;

            if (update)
            {
                update = false;
                UpdateChunk();
            }
        }

        public void SetBlocksUnmodified()
        {
            foreach (Block block in blocks)
            {
                block.changed = false;
            }
        }

        public void SetBlock(int x, int y, int z, Block block, bool updateChunk = false)
        {
            if(InRange(x) && InRange(y) && InRange(z))
            {
                blocks[x, y, z] = block;
                blocks[x, y, z].changed = true;
            }
            else
            {
                world.SetBlock((int)worldPos.x + x, (int)worldPos.y + y, (int)worldPos.z + z, block);
            }
        }

        /// <summary>
        /// Gets a <see cref="Block"/> in the chunk given its positon (if position is outside chunk will return <see cref="new"/> <see cref="Air"/> block)
        /// </summary>
        /// <param name="x">x location</param>
        /// <param name="y">y location</param>
        /// <param name="z">z location</param>
        /// <returns><see cref="Block"/> or some child of <see cref="Block"/></returns>
        public Block GetBlock(int x, int y, int z)
        {
            if(InRange(x) && InRange(y) && InRange(z))
            {
                return blocks[x, y, z];
            }
            else
            {
                // could in also look at the neboring chunk to see if the block should be rendered (look into if performance is bad)
                //return world.GetBlock((int)worldPos.x + x, (int)worldPos.y + y, (int)worldPos.z + z);
                return new Air();
            }
        }

        /// <summary>
        /// Checks of the gien index, i, is in this chunk
        /// </summary>
        /// <param name="i">x, y, or z index of a <see cref="Block"/></param>
        /// <returns><see cref="true"/> if <see cref="Block"/> is in this chunk</returns>
        bool InRange(int i)
        {
            if(i >= 0 && i < chunkSize)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Updates the chunk, and builds the <see cref="MeshData"/>
        /// </summary>
        public void UpdateChunk()
        {
            MeshData mesh = new MeshData();

            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        mesh = blocks[x, y, z].BlockMeshData(this, x, y, z, mesh);
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
                        switch (blocks[x, y, z])
                        {
                            case Apiary a:
                                Instantiate(a.mesh, new THVector3(x, y, z) + worldPos, Quaternion.identity, transform);
                                break;
                            default:
                                break;
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
