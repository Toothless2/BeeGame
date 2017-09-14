using System;
using UnityEngine;
using BeeGame.Core.Dictionaries;
using BeeGame.Core.Enums;
using BeeGame.Items;
using BeeGame.Core;

namespace BeeGame.Blocks
{
    [Serializable]
    public class Leaves : Block
    {
        public new static int ID => 6;

        public Leaves() : base("Leaves")
        {

        }
        
        #region Item Stuff
        public override Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("Leaves");
        }
        #endregion

        public override Tile TexturePosition(Direction direction)
        {
            return new Tile() { x = 5, y = 9 };
        }

        public override bool IsSolid(Direction direction)
        {
            return false;
        }

        public override void BreakBlock(THVector3 pos)
        {
            var temp = new UnityEngine.Random();
            
            if(UnityEngine.Random.Range(0, 100) <= 25)
            {
                GameObject go = UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, pos, Quaternion.identity) as GameObject;

                var species = Bee.ReturnRandomBaseSpecies();
                var type = Bee.ReturnRandomType();

                go.GetComponent<ItemGameObject>().item = new Bee(type, new NormalBee(species));
            }
            else
            {
                base.BreakBlock(pos);
            }
        }

        #region Overrides
        /// <summary>
        /// Base ID of the block
        /// </summary>
        /// <returns>5</returns>
        public override int GetHashCode()
        {
            return ID;
        }

        /// <summary>
        /// Returns the name and ID of the block as a string
        /// </summary>
        /// <returns>A nicely formatted string</returns>
        public override string ToString()
        {
            return $"{itemName} \nID: {GetItemID()}";
        }
        #endregion
    }
}
