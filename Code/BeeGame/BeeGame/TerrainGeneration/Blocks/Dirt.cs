using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Enums;

namespace BeeGame.TerrainGeneration.Blocks
{
    [Serializable]
    public class Dirt : Block
    {
        public Dirt() : base()
        {

        }

        public override Tile TexturePosition(BlockDirection direction)
        {
            return new Tile()
            {
                x = 2,
                y = 9
            };
        }
    }
}
