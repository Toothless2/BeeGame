using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Core.Enums;

namespace BeeGame.Blocks
{
    [Serializable]
    public class Bedrock : Block
    {
        public Bedrock() : base()
        {
            breakable = false;
        }

        public override Tile TexturePosition(Direction direction)
        {
            return new Tile() { x = 0, y = 0};
        }
    }
}
