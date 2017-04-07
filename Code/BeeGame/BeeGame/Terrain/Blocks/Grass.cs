using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Core.Enums;

namespace BeeGame.Terrain.Blocks
{
    [Serializable]
    public class Grass : Block
    {
        public Grass() : base() { }

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
    }
}
