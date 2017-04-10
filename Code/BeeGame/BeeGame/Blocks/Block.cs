using UnityEngine;
using BeeGame.Terrain.Chunks;
using BeeGame.Core.Enums;
using BeeGame.Items;
using BeeGame.Core;

namespace BeeGame.Blocks
{
    [System.Serializable]
    public class Block : Item
    {
        public bool breakable = true;
        public bool changed = true;

        public Block()
        {
            placeable = true;
        }

        public Block(Block block)
        {

        }

        public virtual void BreakBlock(THVector3 pos)
        {
            GameObject go = Object.Instantiate(Resources.Load("Prefabs/ItemGameObject") as GameObject, pos, Quaternion.identity) as GameObject;
            go.GetComponent<ItemGameObject>().item = this;
        }

        public virtual void UpdateBlock(int x, int y, int z, Chunk chunk) { }

        public virtual MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            if (!chunk.GetBlock(x, y + 1, z).IsSolid(Direction.DOWN))
            {
                meshData = FaceDataUp(x, y, z, meshData, addToRenderMesh);
            }

            if (!chunk.GetBlock(x, y - 1, z).IsSolid(Direction.UP))
            {
                meshData = FaceDataDown(x, y, z, meshData, addToRenderMesh);
            }

            if (!chunk.GetBlock(x, y, z + 1).IsSolid(Direction.SOUTH))
            {
                meshData = FaceDataNorth(x, y, z, meshData, addToRenderMesh);
            }

            if (!chunk.GetBlock(x, y, z - 1).IsSolid(Direction.NORTH))
            {
                meshData = FaceDataSouth(x, y, z, meshData, addToRenderMesh);
            }

            if (!chunk.GetBlock(x + 1, y, z).IsSolid(Direction.WEST))
            {
                meshData = FaceDataEast(x, y, z, meshData, addToRenderMesh);
            }

            if (!chunk.GetBlock(x - 1, y, z).IsSolid(Direction.EAST))
            {
                meshData = FaceDataWest(x, y, z, meshData, addToRenderMesh);
            }

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
}
