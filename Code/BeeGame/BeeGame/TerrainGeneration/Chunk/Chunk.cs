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
        #region Data
        /// <summary>
        /// How many blocks in each direction should the chunk have
        /// </summary>
        public static int chunkSize = 16;
        /// <summary>
        /// 3D array of all of the <see cref="Block"/>s in the chunk
        /// </summary>
        public Block[,,] blocks = new Block[chunkSize, chunkSize, chunkSize];
        /// <summary>
        /// Should the chunk be updated (if the chunk has been changed set to true)
        /// </summary>
        public bool update = false;
        
        /// <summary>
        /// The world that the chunk is in
        /// </summary>
        public World world;
        /// <summary>
        /// The position in the world that the chunk is in
        /// </summary>
        public THVector3 worldPos;

        /// <summary>
        /// Should the chunk be rendered
        /// </summary>
        public bool rendered;
        
        /// <summary>
        /// Mesh filter for the chunk
        /// </summary>
        private MeshFilter filter;
        /// <summary>
        /// Mesh Collider for the chunk
        /// </summary>
        private MeshCollider chunkCollider;
        #endregion

        /// <summary>
        /// Gets the mush collider and filter
        /// </summary>
        void Awake()
        {
            filter = gameObject.GetComponent<MeshFilter>();
            chunkCollider = gameObject.GetComponent<MeshCollider>();
        }

        /// <summary>
        /// Checks if the chunk should be updated
        /// </summary>
        void Update()
        {
            rendered = true;

            if (update)
            {
                update = false;
                UpdateChunk();
            }
        }

        /// <summary>
        /// Sets all of the blocks in the chunk to unmodified
        /// </summary>
        public void SetBlocksUnmodified()
        {
            foreach (Block block in blocks)
            {
                block.changed = false;
            }
        }

        #region Get/Set Blocks
        /// <summary>
        /// Sets a block in the chunk
        /// </summary>
        /// <param name="x">X position of the <see cref="Block"/></param>
        /// <param name="y">Y position of the <see cref="Block"/></param>
        /// <param name="z">Z position of the <see cref="Block"/></param>
        /// <param name="block"><see cref="Block"/> that shoudl be set</param>
        public void SetBlock(int x, int y, int z, Block block)
        {
            //if the block is within this chunk then set it
            if(InRange(x) && InRange(y) && InRange(z))
            {
                blocks[x, y, z] = block;
                blocks[x, y, z].changed = true;
            }
            //if the block is not in this chunk find the chunk it is supposed to be in and set it in that chunk
            else
            {
                world.SetBlock((int)worldPos.x + x, (int)worldPos.y + y, (int)worldPos.z + z, block);
            }
        }

        /// <summary>
        /// Gets a <see cref="Block"/> in the chunk given its positon (if position is outside chunk will return <see cref="new"/> <see cref="Air"/> block)
        /// </summary>
        /// <param name="x">X location</param>
        /// <param name="y">Y location</param>
        /// <param name="z">Z location</param>
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
        #endregion

        #region Mesh Stuff
        /// <summary>
        /// Updates the chunk, and builds the <see cref="MeshData"/>
        /// </summary>
        public void UpdateChunk()
        {
            MeshData mesh = new MeshData() { useRenderForColData = true };

            //builds the chunk meshes
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
        }

        /// <summary>
        /// Renders the given mesh
        /// </summary>
        void RenderMesh(MeshData mesh)
        {
            //destroys all of the child gameobjects so the do not get duplicated
            foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }

            //applies the mesh
            filter.mesh.Clear();
            filter.mesh.name = "Chunk Render Mesh";
            filter.mesh.vertices = mesh.verts.ToArray().ToUnityVector3Array();
            filter.mesh.triangles = mesh.tris.ToArray();
            
            filter.mesh.uv = mesh.uv.ToArray().ToUnityVector2Array();


            //recalculates the meshes normals
            filter.mesh.RecalculateNormals();

            //if the same mesh should be used for the collider and the render apply it here otherwise make and apply the collider mesh
            if (mesh.useRenderForColData)
            {
                chunkCollider.sharedMesh = filter.mesh;
                return;
            }

            ColliderMesh(mesh);

            //makes the blocks that are not made in the normal way
            //made after makeing the mesh filter as this takes time and may not be necissary
            for (int x = 0; x < chunkSize; x++)
            {
                for (int y = 0; y < chunkSize; y++)
                {
                    for (int z = 0; z < chunkSize; z++)
                    {
                        if(blocks[x, y, z].GetGameOject() != null)
                        {
                            Instantiate(blocks[x, y, z].GetGameOject(), new THVector3(x, y, z) + worldPos, Quaternion.identity, transform);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applys the given <see cref="MeshData"/> to the collider
        /// </summary>
        /// <param name="mesh"><see cref="MeshData"/> with collider data</param>
        void ColliderMesh(MeshData mesh)
        {
            Mesh colliderMesh = new Mesh()
            {
                name = "Chunk Collider Mesh",
                vertices = mesh.colVerts.ToArray().ToUnityVector3Array(),
                triangles = mesh.colTris.ToArray()
            };

            chunkCollider.sharedMesh = colliderMesh;
        }
        #endregion
    }
}
