using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Enums;

namespace BeeGame.TerrainGeneration
{
    [Serializable]
    public class Block
    {
        /// <summary>
        /// Number of siles in texture divided by 1 (1/4 as their is 4 textures)
        /// </summary>
        const float tileSize = 0.1f;

        public bool changed = true;

        public Block()
        {

        }

        public virtual GameObject GetGameOject()
        {
            return null;
        }

        public virtual Mesh GetMesh()
        {
            return null;
        }

        public virtual Tile TexturePosition(BlockDirection direction)
        {
            return new Tile() { x = 1, y = 9 };
        }

        public virtual THVector2[] FaceUVs(BlockDirection direction)
        {
            THVector2[] uVs = new THVector2[4];

            Tile tilepos = TexturePosition(direction);

            uVs[0] = new THVector2(tileSize * tilepos.x + tileSize - 0.01f, tileSize * tilepos.y + 0.01f);
            uVs[1] = new THVector2(tileSize * tilepos.x + tileSize - 0.01f, tileSize * tilepos.y + tileSize - 0.01f);
            uVs[2] = new THVector2(tileSize * tilepos.x + 0.01f, tileSize * tilepos.y + tileSize - 0.01f);
            uVs[3] = new THVector2(tileSize * tilepos.x + 0.01f, tileSize * tilepos.y + 0.01f); // - moves north

            return uVs;
        }

        /// <summary>
        /// Will retun the mesh data for all of the sides of the block that can bee seen
        /// </summary>
        /// <param name="chunk">the <see cref="Chunk"/> the block is in</param>
        /// <param name="x">x pos of the block</param>
        /// <param name="y">y pos of the block</param>
        /// <param name="z">z pos of the block</param>
        /// <param name="meshData">the <see cref="MeshData"/> that the block needs to add its data to</param>
        /// <returns><see cref="MeshData"/> given plus its data</returns>
        public virtual MeshData BlockMeshData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRender = true)
        {
            //does the block above me have a solid face
            if(!chunk.GetBlock(x, y + 1, z).IsSolid(BlockDirection.DOWN))
            {
                meshData = FaceDataUp(chunk, x, y, z, meshData, addToRender);
            }
            //does the block below me have a solid face
            if (!chunk.GetBlock(x, y - 1, z).IsSolid(BlockDirection.UP))
            {
                meshData = FaceDataDown(chunk, x, y, z, meshData, addToRender);
            }
            //does the block in front of me have a solid face
            if (!chunk.GetBlock(x, y, z + 1).IsSolid(BlockDirection.SOUTH))
            {
                meshData = FaceDataNorth(chunk, x, y, z, meshData, addToRender);
            }
            //does the block behind me have a solid face
            if (!chunk.GetBlock(x, y, z - 1).IsSolid(BlockDirection.NORTH))
            {
                meshData = FaceDataSouth(chunk, x, y, z, meshData, addToRender);
            }
            //does the block to me east have a solid face
            if (!chunk.GetBlock(x + 1, y, z).IsSolid(BlockDirection.WEST))
            {
                meshData = FaceDataEast(chunk, x, y, z, meshData, addToRender);
            }
            //does the block to me west have a solid face
            if (!chunk.GetBlock(x - 1, y, z).IsSolid(BlockDirection.EAST))
            {
                meshData = FaceDataWest(chunk, x, y, z, meshData, addToRender);
            }

            return meshData;
        }

        #region Faces to be Rendered
        protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRender)
        {
            //Added in a clockwise order
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), addToRender);

            meshData.AddQuadTriangles(addToRender);

            if(addToRender)
                meshData.uv.AddRange(FaceUVs(BlockDirection.UP).ToList());
            return meshData;
        }
        protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRender)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), addToRender);

            meshData.AddQuadTriangles(addToRender);

            if(addToRender)
                meshData.uv.AddRange(FaceUVs(BlockDirection.DOWN).ToList());
            return meshData;
        }
        protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRender)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), addToRender);

            meshData.AddQuadTriangles(addToRender);

            if(addToRender)
                meshData.uv.AddRange(FaceUVs(BlockDirection.NORTH).ToList());
            return meshData;
        }
        protected virtual MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRender)
        {
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), addToRender);

            meshData.AddQuadTriangles(addToRender);

            if(addToRender)
                meshData.uv.AddRange(FaceUVs(BlockDirection.EAST).ToList());
            return meshData;
        }
        protected virtual MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRender)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), addToRender);

            meshData.AddQuadTriangles(addToRender);

            if(addToRender)
                meshData.uv.AddRange(FaceUVs(BlockDirection.SOUTH).ToList());
            return meshData;
        }
        protected virtual MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRender)
        {
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), addToRender);
            meshData.AddVertex(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), addToRender);

            meshData.AddQuadTriangles(addToRender);

            if(addToRender)
                meshData.uv.AddRange(FaceUVs(BlockDirection.WEST).ToList());
            return meshData;
        }

        public virtual bool IsSolid(BlockDirection direction)
        {
            switch (direction)
            {
                case BlockDirection.NORTH:
                    return true;
                case BlockDirection.EAST:
                    return true;
                case BlockDirection.SOUTH:
                    return true;
                case BlockDirection.WEST:
                    return true;
                case BlockDirection.UP:
                    return true;
                case BlockDirection.DOWN:
                    return true;
                default:
                    return false;
            }
        }
        #endregion
    }

    public struct Tile {public int x; public int y; }
}
