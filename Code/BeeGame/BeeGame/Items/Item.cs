using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Terrain.Chunks;
using BeeGame.Core.Enums;

namespace BeeGame.Items
{
    [Serializable]
    public class Item
    {
        public bool placeable = false;
        public bool usesGameObject = false;
        private const float tileSize = 0.1f;

        public virtual GameObject GetGameObject() { return null; }

        #region Item Mesh
        public virtual Tile TexturePosition(Direction direction)
        {
            return new Tile() { x = 1, y = 9 };
        }

        public virtual MeshData ItemMesh(int x, int y, int z, MeshData meshData)
        {
            meshData = FaceDataUp(x, y, z, meshData, true, 0.25f);
            meshData = FaceDataDown(x, y, z, meshData, true, 0.25f);
            meshData = FaceDataNorth(x, y, z, meshData, true, 0.25f);
            meshData = FaceDataEast(x, y, z, meshData, true, 0.25f);
            meshData = FaceDataSouth(x, y, z, meshData, true, 0.25f);
            meshData = FaceDataWest(x, y, z, meshData, true, 0.25f);

            return meshData;
        }

        public virtual Vector2[] FaceUVs(Direction direction)
        {
            Vector2[] UVs = new Vector2[4];
            Tile tilePos = TexturePosition(direction);

            UVs[0] = new Vector2(tileSize * tilePos.x + tileSize - 0.01f, tileSize * tilePos.y + 0.01f);
            UVs[1] = new Vector2(tileSize * tilePos.x + tileSize - 0.01f, tileSize * tilePos.y + tileSize - 0.01f);
            UVs[2] = new Vector2(tileSize * tilePos.x + 0.01f, tileSize * tilePos.y + tileSize - 0.01f);
            UVs[3] = new Vector2(tileSize * tilePos.x + 0.01f, tileSize * tilePos.y + 0.01f); // - moves north

            return UVs;
        }

        protected virtual MeshData FaceDataUp(int x, int y, int z, MeshData meshData, bool addToRenderMesh = true, float blockSize = 0.5f)
        {
            meshData.AddVertices(new Vector3(x - blockSize, y + blockSize, z + blockSize), addToRenderMesh, Direction.UP);
            meshData.AddVertices(new Vector3(x + blockSize, y + blockSize, z + blockSize), addToRenderMesh, Direction.UP);
            meshData.AddVertices(new Vector3(x + blockSize, y + blockSize, z - blockSize), addToRenderMesh, Direction.UP);
            meshData.AddVertices(new Vector3(x - blockSize, y + blockSize, z - blockSize), addToRenderMesh, Direction.UP);

            meshData.AddQuadTriangles(addToRenderMesh);

            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.UP));

            return meshData;
        }

        protected virtual MeshData FaceDataDown(int x, int y, int z, MeshData meshData, bool addToRenderMesh = true, float blockSize = 0.5f)
        {
            meshData.AddVertices(new Vector3(x - blockSize, y - blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + blockSize, y - blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + blockSize, y - blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - blockSize, y - blockSize, z + blockSize), addToRenderMesh);

            meshData.AddQuadTriangles(addToRenderMesh);

            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.DOWN));

            return meshData;
        }

        protected virtual MeshData FaceDataNorth(int x, int y, int z, MeshData meshData, bool addToRenderMesh = true, float blockSize = 0.5f)
        {
            meshData.AddVertices(new Vector3(x + blockSize, y - blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + blockSize, y + blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - blockSize, y + blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - blockSize, y - blockSize, z + blockSize), addToRenderMesh);

            meshData.AddQuadTriangles(addToRenderMesh);

            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.NORTH));

            return meshData;
        }

        protected virtual MeshData FaceDataEast(int x, int y, int z, MeshData meshData, bool addToRenderMesh = true, float blockSize = 0.5f)
        {
            meshData.AddVertices(new Vector3(x + blockSize, y - blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + blockSize, y + blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + blockSize, y + blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + blockSize, y - blockSize, z + blockSize), addToRenderMesh);

            meshData.AddQuadTriangles(addToRenderMesh);

            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.EAST));

            return meshData;
        }

        protected virtual MeshData FaceDataSouth(int x, int y, int z, MeshData meshData, bool addToRenderMesh = true, float blockSize = 0.5f)
        {
            meshData.AddVertices(new Vector3(x - blockSize, y - blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - blockSize, y + blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + blockSize, y + blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + blockSize, y - blockSize, z - blockSize), addToRenderMesh);

            meshData.AddQuadTriangles(addToRenderMesh);

            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.SOUTH));

            return meshData;
        }

        protected virtual MeshData FaceDataWest(int x, int y, int z, MeshData meshData, bool addToRenderMesh = true, float blockSize = 0.5f)
        {
            meshData.AddVertices(new Vector3(x - blockSize, y - blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - blockSize, y + blockSize, z + blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - blockSize, y + blockSize, z - blockSize), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - blockSize, y - blockSize, z - blockSize), addToRenderMesh);

            meshData.AddQuadTriangles(addToRenderMesh);

            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.WEST));

            return meshData;
        }
        #endregion
    }



    public struct Tile
    {
        public int x;
        public int y;
    }
}
