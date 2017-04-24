using UnityEngine;
using BeeGame.Blocks;
using BeeGame.Terrain.LandGeneration;
using System.Threading;

namespace BeeGame.Terrain.Chunks
{
    /// <summary>
    /// A section of land for the game, used so that land can be generated in parts and not all at once
    /// </summary>
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class Chunk : MonoBehaviour
    {
        #region Data
        /// <summary>
        /// Size of the <see cref="Chunk"/>
        /// </summary>
        /// <remarks>
        /// Same size for x, y, z \n
        /// Posibly some place has 16 hard coded as reduceing the number breaks things TODO: find
        /// </remarks>
        public static int chunkSize = 16;

        /// <summary>
        /// All of the <see cref="Block"/>s in the <see cref="Chunk"/>
        /// </summary>
        public Block[,,] blocks = new Block[chunkSize, chunkSize, chunkSize];

        /// <summary>
        /// Should the <see cref="Chunk"/> be updated?
        /// </summary>
        public bool update = true;
        /// <summary>
        /// Is the <see cref="Chunk"/> rendered?
        /// </summary>
        public bool rendered;

        /// <summary>
        /// Should the chunks collision mesh be updated?
        /// </summary>
        public bool updateCollsionMesh = false;
        /// <summary>
        /// Should the collision mesh be applied
        /// </summary>
        public bool applyCollisionMesh = false;

        /// <summary>
        /// <see cref="World"/> that this chunk is in as <see cref="MonoBehaviour"/>s cannot be static this is for convenicence
        /// </summary>
        public World world;
        /// <summary>
        /// <see cref="Chunk"/>s position in the world as a <see cref="ChunkWorldPos"/> (int verson of <see cref="Core.THVector3"/>)
        /// </summary>
        public ChunkWorldPos chunkWorldPos;

        /// <summary>
        /// <see cref="MeshData"/> of this chunk
        /// </summary>
        private MeshData mesh = new MeshData();

        /// <summary>
        /// This <see cref="Chunk"/>s mesh filter
        /// </summary>
        private MeshFilter filter;
        /// <summary>
        /// This <see cref="Chunk"/>s mesh colldier
        /// </summary>
        private MeshCollider meshCollider;
        #endregion

        #region Unity Methods
        /// <summary>
        /// Sets the <see cref="meshCollider"/> and <see cref="filter"/> variables
        /// </summary>
        void Start()
        {
            filter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();

            gameObject.isStatic = true;
        }

        /// <summary>
        /// Checks if the <see cref="Chunk"/> should be updated
        /// </summary>
        void Update()
        {
            lock(mesh)
            {
                if (update)
                {
                    update = false;
                    updateCollsionMesh = true;
                    mesh = new MeshData();
                    //* Enabling threading here works in editor but not in build?
                    //* ok whatever...
                    //* Thread thread = new Thread(UpdateChunk);

                    //* thread.Start();
                    UpdateChunk();
                }

                if (mesh.done && mesh != new MeshData())
                {
                    RenderMesh(mesh);
                }

                if (applyCollisionMesh)
                    ColliderMesh();
            }
        }
        #endregion

        #region Get/Set Blocks
        /// <summary>
        /// Returns the <see cref="Block"/> in the given x, y, z
        /// </summary>
        /// <param name="x">X pos if the <see cref="Block"/></param>
        /// <param name="y">Z pos if the <see cref="Block"/></param>
        /// <param name="z">Y pos if the <see cref="Block"/></param>
        /// <param name="checkNebouringChunks">Shoud this check nebouring chunks? Only set to false when chunk <see cref="mesh"/> is being built for performance</param>
        /// <returns><see cref="Block"/> at given x, y, z</returns>
        public Block GetBlock(int x, int y, int z, bool checkNebouringChunks = true)
        {
            //* checks that block is in the chunk
            if (InRange(x) && InRange(y) && InRange(z))
                return blocks[x, y, z];

            //* if the block is not in the chunk and we should check other chunks do that, otherwise return an air block (empty block)
            //if(checkNebouringChunks)
                return world.GetBlock(chunkWorldPos.x + x, chunkWorldPos.y + y, chunkWorldPos.z + z);

            //return new Air();
        }

        /// <summary>
        /// Sets a <see cref="Block"/> in the given position
        /// </summary>
        /// <param name="x">X pos of the <see cref="Block"/></param>
        /// <param name="y">Y pos of the <see cref="Block"/></param>
        /// <param name="z">Z pos of the <see cref="Block"/></param>
        /// <param name="block"><see cref="Block"/> to set</param>
        public void SetBlock(int x, int y, int z, Block block, bool checkNebouringChunks = true)
        {
            //* sets the block in the position if it is in the chunk, then return early
            if (InRange(x) && InRange(y) && InRange(z))
            {
                blocks[x, y, z] = block;
                return;
            }

            if (checkNebouringChunks)
                //* if the block is not in the chunk find its chunk and set it their
                world.SetBlock(chunkWorldPos.x + x, chunkWorldPos.y + y, chunkWorldPos.z + z, block);
        }

        /// <summary>
        /// Checks that a given value is within the <see cref="Chunk"/>
        /// </summary>
        /// <param name="i">Value to check</param>
        /// <returns>true if the value is in the <see cref="Chunk"/></returns>
        public static bool InRange(int i)
        {
            //* if the value is less then 0 or greater than 16 the value is outside the chunk
            if (i < 0 || i >= chunkSize)
                return false;
            return true;
        }
        #endregion

        #region Mesh
        /// <summary>
        /// Sets all of the <see cref="Block"/>s in the <see cref="blocks"/> array to unmodifed so that the whole chunk is not saved when it does not need to be
        /// </summary>
        /// <remarks>
        /// A modifed <see cref="Block"/> is a <see cref="Block"/> removed or added by the player
        /// </remarks>
        public void SetBlocksUnmodified()
        {
            foreach (var block in blocks)
            {
                block.changed = false;
            }
        }

        /// <summary>
        /// Updates the <see cref="mesh"/> for the <see cref="Chunk"/>
        /// </summary>
        void UpdateChunk()
        {
            //* says that this chunk is rendered and initialtes the mesh
            rendered = true;

            //* goes through every block in the blocks array getting their mesh data
            for (int x = 0; x < chunkSize; x ++)
            {
                for (int z = 0; z < chunkSize; z ++)
                {
                    for (int y = 0; y < chunkSize; y ++)
                    {
                        blocks[x, y, z]?.UpdateBlock(x, y, z, this);
                        mesh = blocks[x, y, z]?.BlockData(this, x, y, z, mesh) ?? mesh;
                    }
                }
            }
            mesh.done = true;
        }

        /// <summary>
        /// Renders the given <see cref="MeshData"/> into a unity <see cref="Mesh"/>
        /// </summary>
        /// <param name="meshData">Mesh data to render</param>
        void RenderMesh(MeshData meshData)
        {
            //* Applying the mesh takes the longest but nothing can be dont with the mesh class in a secondary thread...thanks unity

            mesh.done = false;
            //* clears the current chunk mesh
            filter.mesh.Clear();
            //* name for convenience
            filter.mesh.name = "Render Mesh";
            //* puts the tris and verts from the meshdata into the chunk mesh
            filter.mesh.vertices = meshData.verts.ToArray();
            filter.mesh.triangles = meshData.tris.ToArray();

            //* sets the uvs
            filter.mesh.uv = meshData.uv.ToArray();

            //* redoes the normals incase they got messed up
            filter.mesh.RecalculateNormals();
            //* is this necissary as it causes alsot of lag?
        }

        /// <summary>
        /// Makes a collision mesh from the <see cref="mesh"/>
        /// </summary>
        void ColliderMesh()
        {
            //* if the chunk has been told to update the collsions but the chunk has ne verts dont do it as their is no point
            if (this.mesh.verts.Count == 0)
                return;

            //* if the render and collision meshes should be shared set the render mesh to the collision mesh otherwise make a collision mesh
            if (this.mesh.shareMeshes)
            {
                world.chunkHasMadeCollisionMesh = true;
                applyCollisionMesh = false;
                meshCollider.sharedMesh = filter.mesh;
                return;
            }

            world.chunkHasMadeCollisionMesh = true;
            //* Applying the mesh takes the longest but nothing can be done with the mesh class in a secondary thread...thanks Unity

            //* makes a new mesh setting the name for convenience
            Mesh mesh = new Mesh()
            {
                name = "Collider Mesh",
                vertices = this.mesh.colVerts.ToArray(),
                triangles = this.mesh.colTris.ToArray()
            };
            
            //* recalcs the normals and applies the mesh
            mesh.RecalculateNormals();

            meshCollider.sharedMesh = mesh;

            applyCollisionMesh = false;
        }
        #endregion
    }
}
