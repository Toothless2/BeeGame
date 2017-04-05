using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Enums;

namespace BeeGame.TerrainGeneration.Blocks
{
    [Serializable]
    public class Grass : Block
    {
        public Grass() : base()
        {

        }

        public override Tile TexturePosition(BlockDirection direction)
        {
            Tile tile = new Tile();

            switch (direction)
            {
                case BlockDirection.UP:
                    tile.x = 3;
                    tile.y = 9;
                    return tile;
                case BlockDirection.DOWN:
                    tile.x = 2;
                    tile.y = 9;
                    return tile;
                default:
                    tile.x = 4;
                    tile.y = 9;
                    return tile;
            }
        }
    }
}
