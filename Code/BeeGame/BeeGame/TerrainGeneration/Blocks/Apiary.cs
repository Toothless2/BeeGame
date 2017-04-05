using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Enums;

namespace BeeGame.TerrainGeneration.Blocks
{
    [Serializable]
    public class Apiary : Block
    {
        [NonSerialized]
        public GameObject mesh;

        public Apiary() : base()
        {
            mesh = PrefabDictionary.GetGameObjectItemFromDictionary("BlockTest");
        }

        public override Tile TexturePosition(BlockDirection direction)
        {
            return new Tile();
            return new Tile { x = 0, y = 3 };
        }

        public override bool IsSolid(BlockDirection direction)
        {
            return false;
        }
    }
}
