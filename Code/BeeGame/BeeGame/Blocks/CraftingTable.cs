using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Core.Enums;
using BeeGame.Items;
using BeeGame.Terrain.Chunks;

namespace BeeGame.Blocks
{
    [Serializable]
    public class CraftingTable : Block
    {
        [NonSerialized]
        private GameObject myGameobject;

        public new static int ID => 9;

        public CraftingTable() : base("Crafting Table")
        {
            usesGameObject = true;
        }

        public override GameObject GetGameObject()
        {
            return PrefabDictionary.GetPrefab("CraftingTable");
        }

        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            if (myGameobject == null)
            {
                myGameobject = UnityEngine.Object.Instantiate(PrefabDictionary.GetPrefab("CraftingTable"), new THVector3(x, y, z) + chunk.chunkWorldPos, Quaternion.identity, chunk.transform);
            }
            return base.BlockData(chunk, x, y, z, meshData, true);
        }

        public override void BreakBlock(THVector3 pos)
        {
            UnityEngine.Object.Destroy(myGameobject);
            //* removes the collision mesh from the chunk
            base.BreakBlock(pos);
        }

        public override Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("TestSprite");
        }

        public override Tile TexturePosition(Direction direction)
        {
            return new Tile() { x = 0, y = 9 };
        }

        public override int GetHashCode()
        {
            return ID;
        }
    }
}
