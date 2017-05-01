using System;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Terrain.Chunks;
using BeeGame.Core.Enums;
using BeeGame.Items;
using BeeGame.Inventory;

namespace BeeGame.Blocks
{
    [Serializable]
    public class Chest : Block
    {
        [NonSerialized]
        private GameObject myGameobject;

        public Chest() : base("Chest")
        {
            usesGameObject = true;
        }

        public override GameObject GetGameObject()
        {

            return PrefabDictionary.GetPrefab("Chest");
        }

        public override Tile TexturePosition(Direction direction)
        {
            return new Tile() { x = 0, y = 9 };
        }

        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            if (myGameobject == null)
            {
                myGameobject = UnityEngine.Object.Instantiate(PrefabDictionary.GetPrefab("Chest"), new THVector3(x, y, z) + chunk.chunkWorldPos, Quaternion.identity, chunk.transform);
                myGameobject.GetComponent<ChestInventory>().inventoryPosition = new THVector3(x, y, z) + chunk.chunkWorldPos;
                myGameobject.GetComponent<ChestInventory>().SetChestInventory(); 
            }
            return base.BlockData(chunk, x, y, z, meshData, true);
        }

        public override bool InteractWithBlock(BeeGame.Inventory.Inventory inv)
        {
            myGameobject.GetComponent<ChestInventory>().ToggleInventory(inv);
             return true;
        }

        public override void BreakBlock(THVector3 pos)
        {
            Serialization.Serialization.DeleteFile(myGameobject.GetComponent<ChestInventory>().inventoryName);
            UnityEngine.Object.Destroy(myGameobject);
            base.BreakBlock(pos);
        }

        public override int GetHashCode()
        {
            return 8;
        }

        public override string ToString()
        {
            return $"{itemName}\nID{GetItemID()}";
        }
    }
}
