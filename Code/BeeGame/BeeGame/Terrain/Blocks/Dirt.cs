using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Core.Enums;
using UnityEngine;

namespace BeeGame.Terrain.Blocks
{
    public class Dirt : Block
    {
        public Dirt() : base() { }

        public override Tile TexturePosition(Direction direction)
        {
            return new Tile { x = 2, y = 9 };
        }
    }
}
