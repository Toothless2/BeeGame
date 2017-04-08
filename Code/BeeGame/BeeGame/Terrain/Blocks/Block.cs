using UnityEngine;
using BeeGame.Terrain.Chunks;
using BeeGame.Core.Enums;

namespace BeeGame.Terrain.Blocks
{
    [System.Serializable]
    public class Block
    {
        public bool breakable = true;
        public bool changed = true;
        private const float tileSize = 0.1f;

        public Block() { }

        public virtual void UpdateBlock(int x, int y, int z, Chunk chunk) { }

        public virtual Tile TexturePosition(Direction direction)
        {
            return new Tile() {x = 1, y = 9};
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

        public virtual MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.DOWN))
            {
                meshData = FaceDataUp(chunk, x, y, z, meshData, addToRenderMesh);
            }

            if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.UP))
            {
                meshData = FaceDataDown(chunk, x, y, z, meshData, addToRenderMesh);
            }

            if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.SOUTH))
            {
                meshData = FaceDataNorth(chunk, x, y, z, meshData, addToRenderMesh);
            }

            if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.NORTH))
            {
                meshData = FaceDataSouth(chunk, x, y, z, meshData, addToRenderMesh);
            }

            if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.WEST))
            {
                meshData = FaceDataEast(chunk, x, y, z, meshData, addToRenderMesh);
            }

            if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.EAST))
            {
                meshData = FaceDataWest(chunk, x, y, z, meshData, addToRenderMesh);
            }

            return meshData;

        }

        protected virtual MeshData FaceDataUp(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            meshData.AddVertices(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), addToRenderMesh, Direction.UP);
            meshData.AddVertices(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), addToRenderMesh, Direction.UP);
            meshData.AddVertices(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), addToRenderMesh, Direction.UP);
            meshData.AddVertices(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), addToRenderMesh, Direction.UP);

            meshData.AddQuadTriangles(addToRenderMesh);

            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.UP));

            return meshData;
        }

        protected virtual MeshData FaceDataDown(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            meshData.AddVertices(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), addToRenderMesh);

            meshData.AddQuadTriangles(addToRenderMesh);

            if(addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.DOWN));

            return meshData;
        }

        protected virtual MeshData FaceDataNorth(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            meshData.AddVertices(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), addToRenderMesh);

            meshData.AddQuadTriangles(addToRenderMesh);

            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.NORTH));

            return meshData;
        }

        protected virtual MeshData FaceDataEast(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            meshData.AddVertices(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + 0.5f, y + 0.5f, z + 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + 0.5f, y - 0.5f, z + 0.5f), addToRenderMesh);

            meshData.AddQuadTriangles(addToRenderMesh);

            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.EAST));

            return meshData;
        }

        protected virtual MeshData FaceDataSouth(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            meshData.AddVertices(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + 0.5f, y + 0.5f, z - 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x + 0.5f, y - 0.5f, z - 0.5f), addToRenderMesh);

            meshData.AddQuadTriangles(addToRenderMesh);

            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.SOUTH));

            return meshData;
        }

        protected virtual MeshData FaceDataWest(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            meshData.AddVertices(new Vector3(x - 0.5f, y - 0.5f, z + 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - 0.5f, y + 0.5f, z + 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - 0.5f, y + 0.5f, z - 0.5f), addToRenderMesh);
            meshData.AddVertices(new Vector3(x - 0.5f, y - 0.5f, z - 0.5f), addToRenderMesh);

            meshData.AddQuadTriangles(addToRenderMesh);

            if (addToRenderMesh)
                meshData.uv.AddRange(FaceUVs(Direction.WEST));

            return meshData;
        }

        public virtual bool IsSolid(Direction direction)
        {
            switch (direction)
            {
                case Direction.NORTH:
                    return true;
                case Direction.EAST:
                    return true;
                case Direction.SOUTH:
                    return true;
                case Direction.WEST:
                    return true;
                case Direction.UP:
                    return true;
                case Direction.DOWN:
                    return true;
                default:
                    return false;
            }
        }
    }

    public struct Tile
    {
        public int x;
        public int y;
    }
}
