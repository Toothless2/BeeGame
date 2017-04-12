using System;
using BeeGame.Core.Enums;
using BeeGame.Terrain.Chunks;
using BeeGame.Items;

namespace BeeGame.Blocks
{
    [Serializable]
    public class Grass : Block
    {
        public Grass() : base() { }

        public override void UpdateBlock(int x, int y, int z, Chunk chunk)
        {
            if (chunk.GetBlock(x, y + 1, z).IsSolid(Direction.DOWN))
                chunk.blocks[x, y, z] = new Dirt() { changed = changed };
        }

        public override Tile TexturePosition(Direction direction)
        {
            Tile tile = new Tile()
            {
                y = 9
            };

            switch (direction)
            {
                case Direction.UP:
                    tile.x = 3;
                    return tile;
                case Direction.DOWN:
                    tile.x = 2;
                    return tile;
                default:
                    tile.x = 4;
                    return tile;
            }
        }

        public override int GetHashCode()
        {
            return 4;
        }

        public override string ToString()
        {
            return $"Grass ID: {GetHashCode()}";
        }
    }
}
