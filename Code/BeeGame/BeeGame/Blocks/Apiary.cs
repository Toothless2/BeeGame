using System;
using System.Linq;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Items;
using BeeGame.Inventory;
using BeeGame.Core.Enums;
using BeeGame.Terrain.Chunks;
using BeeGame.Core.Dictionarys;

namespace BeeGame.Blocks
{
    /// <summary>
    /// Apiary Block
    /// </summary>
    [Serializable]
    public class Apiary : Block
    {
        [NonSerialized]
        private GameObject myGameobject;

        public int mutationMultiplyer;

        public new static int ID => 10;

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Apiary() : base("Apiary")
        {
            usesGameObject = true;
        }
        #endregion

        #region Block Overrides
        /// <summary>
        /// Gets the game object for this apiary
        /// </summary>
        /// <returns>THe chest game object</returns>
        public override GameObject GetGameObject()
        {
            return PrefabDictionary.GetPrefab("Apiary");
        }

        /// <summary>
        /// Returns the texture for the apiary <see cref="Block"/>
        /// </summary>
        /// <param name="direction"><see cref="Direction"/> of thhe desired face</param>
        /// <returns><see cref="Tile"/> with the textture coordinates of the <see cref="Block"/> texture</returns>
        /// <remarks>
        /// Returns a trnasparent texture as the chest model already has a texture applied
        /// </remarks>
        public override Tile TexturePosition(Direction direction)
        {
            return new Tile() { x = 0, y = 9 };
        }

        /// <summary>
        /// The data that this block adds to the mesh
        /// </summary>
        /// <param name="chunk">Chunk the block is in</param>
        /// <param name="x">X pos of the block</param>
        /// <param name="y">Y pos of the block</param>
        /// <param name="z">Z pos of the block</param>
        /// <param name="meshData">meshdata to add to</param>
        /// <param name="addToRenderMesh">should the block also be added to the render mesh not just the collsion mesh</param>
        /// <returns>Given <paramref name="meshData"/> with this blocks data added to it</returns>
        /// <remarks>
        /// Only adds to the colision mesh as the model is handlled by the unity prefab system
        /// </remarks>
        public override MeshData BlockData(Chunk chunk, int x, int y, int z, MeshData meshData, bool addToRenderMesh = true)
        {
            if (myGameobject == null)
            {
                myGameobject = UnityEngine.Object.Instantiate(PrefabDictionary.GetPrefab("Apiary"), new THVector3(x, y, z) + chunk.chunkWorldPos, Quaternion.identity, chunk.transform);
                myGameobject.GetComponent<ChestInventory>().inventoryPosition = new THVector3(x, y, z) + chunk.chunkWorldPos;
                myGameobject.GetComponent<ChestInventory>().SetChestInventory();
            }
            return base.BlockData(chunk, x, y, z, meshData, true);
        }

        /// <summary>
        /// Breaks the block
        /// </summary>
        /// <param name="pos">Position of the block</param>
        public override void BreakBlock(THVector3 pos)
        {
            //* removes the blocks blocks inventory save file and destroys the game object
            Serialization.Serialization.DeleteFile(myGameobject.GetComponent<ApiaryInventory>().inventoryName);
            UnityEngine.Object.Destroy(myGameobject);
            //* removes the collision mesh from the chunk
            base.BreakBlock(pos);
        }

        public override Sprite GetItemSprite()
        {
            return SpriteDictionary.GetSprite("Apiary");
        }
        #endregion

        #region Overrides
        /// <summary>
        /// ID of the item
        /// </summary>
        /// <returns>3</returns>
        public override int GetHashCode()
        {
            return ID;
        }

        /// <summary>
        /// The item name and ID as a string
        /// </summary>
        /// <returns>A nicely formatted string</returns>
        public override string ToString()
        {
            return $"{itemName} \nID: {GetItemID()}";
        }
        #endregion

        /// <summary>
        /// Toggles the <see cref="ApiaryInventory"/> for the block
        /// </summary>
        /// <param name="inv"></param>
        /// <returns></returns>
        public override bool InteractWithBlock(Inventory.Inventory inv)
        {
            myGameobject.GetComponent<ApiaryInventory>().myblock = this;
            myGameobject.GetComponent<ApiaryInventory>().ToggleInventory(inv);
            return true;
        }

        #region Bee Combineing Stuff
        /// <summary>
        /// Will make new <see cref="Bee"/>/<see cref="Item"/>s from the given <see cref="BeeType.QUEEN"/> <see cref="Bee"/>
        /// </summary>
        /// <param name="queen">The <see cref="BeeType.QUEEN"/> to make the new <see cref="Bee"/>s from</param>
        /// <param name="inventory"><see cref="Inventory.Inventory"/> to put the new Bees/Items into</param>
        /// <remarks>
        /// Inventory is passed by reference to make it easier to modify the inventory. However is not necisseraly needed as a <see cref="class"/> array is being passed so a reference would be created anyway however so <see cref="ref"/> is their more for clarity due to the function modifying the invetory directly
        /// </remarks>
        public void MakeBees(Bee queen, ref Item[] inventory)
        {
            Item[] producedItems = new Item[9];

            //* will always return a new princess and drone
            producedItems[0] = MakeBee(BeeType.PRINCESS, queen.queenBee);
            producedItems[1] = MakeBee(BeeType.DRONE, queen.queenBee);

            var repeats = UnityEngine.Random.Range(0, queen.queenBee.queen.pFertility);

            //* produces as many other children as the bee staats will allow
            for (int i = 0; i < repeats; i++)
            {
                producedItems[i + 2] = MakeBee(queen.queenBee.queen.pFertility > 6 ? (BeeType)UnityEngine.Random.Range(1, 3) : BeeType.DRONE, queen.queenBee);

                if (producedItems[i + 2] is Bee b && b.beeType != BeeType.PRINCESS)
                    producedItems[i + 2].itemStackCount = UnityEngine.Random.Range(1, (int)queen.queenBee.queen.pFertility + 1);
            }

            //* gets the produced items
            var beeProduce = BeeDictionarys.GetBeeProduce(queen.queenBee.queen.pSpecies);

            //* chnages the stack count of the produced items to the correct number
            for (int i = 0; i < beeProduce.Length; i++)
            {
                beeProduce[i].itemStackCount += UnityEngine.Random.Range(1, (int)queen.queenBee.queen.sProdSpeed + 1);
            }

            //* adds the itmes that the bee species produces into the procued item array
            for (int i = (int)queen.queenBee.queen.pFertility + 2, prod = 0; prod < beeProduce.Length; i++, prod++)
            {
                producedItems[i] = beeProduce[prod];
            }

            //* puts the items into the inventory
            for (int i = 0; i < 9; i++)
            {
                if (inventory[i + 2] != null)
                {
                    //* if the slot has the same item in it and it wont be more than the max stack ount but the new item into it
                    if (producedItems[i] == inventory[i + 2] && inventory[i + 2].itemStackCount + 1 <= inventory[i + 2].maxStackCount)
                        inventory[i + 2].itemStackCount++;
                    else
                        //* otherwise find a new slot to put the item into
                        for (int j = i; j < (9 - i); j++)
                        {
                            if (inventory[j + 2] == null)
                            {
                                inventory[j + 2] = producedItems[i];
                                break;
                            }
                            else if (producedItems[i] == inventory[j + 2] && inventory[j + 2].itemStackCount + 1 <= inventory[j + 2].maxStackCount)
                            {
                                inventory[j + 2].itemStackCount++;
                                break;
                            }
                        }
                }
                //* if the slot is empty put the item into it
                else
                    inventory[i + 2] = producedItems[i];
            }
        }

        /// <summary>
        /// Nakes a new <see cref="Bee"/>
        /// </summary>
        /// <param name="beeType">The type of bee to make, <see cref="BeeType"/></param>
        /// <param name="queen">Th stats the new <see cref="Bee"/> should be made with, <see cref="QueenBee"/></param>
        /// <returns>A new <see cref="Bee"/></returns>
        public Bee MakeBee(BeeType beeType, QueenBee queen)
        {
            //* gives all of the primary and secondary stats to the bee
            NormalBee nb = new NormalBee()
            {
                pSpecies = CombineSpecies(queen.queen.sSpecies, queen.drone.sSpecies),
                sSpecies = CombineSpecies(queen.queen.sSpecies, queen.drone.sSpecies),

                pEffect = CombineEffect(queen.queen.sEffect, queen.drone.sEffect),
                sEffect = CombineEffect(queen.queen.sEffect, queen.drone.sEffect),

                pFertility = CombineFertility(queen.queen.sFertility, queen.drone.sFertility),
                sFertility = CombineFertility(queen.queen.sFertility, queen.drone.sFertility),

                pLifespan = CombineLifespan(queen.queen.sLifespan, queen.drone.sLifespan),
                sLifespan = CombineLifespan(queen.queen.sLifespan, queen.drone.sLifespan),

                pProdSpeed = CombineProductionSpeed(queen.queen.sProdSpeed, queen.drone.sProdSpeed),
                sProdSpeed = CombineProductionSpeed(queen.queen.sProdSpeed, queen.drone.sProdSpeed)
            };

            //* returns the new bee
            return new Bee(beeType, nb);
        }

        /// <summary>
        /// Returns a <see cref="BeeSpecies"/> depending on the given <see cref="BeeSpecies"/>
        /// </summary>
        /// <param name="s1">First <see cref="BeeSpecies"/></param>
        /// <param name="s2">Second <see cref="BeeSpecies"/></param>
        /// <returns>A new <see cref="BeeSpecies"/></returns>
        private BeeSpecies CombineSpecies(BeeSpecies s1, BeeSpecies s2)
        {
            BeeSpecies[] possibleSpecies = BeeDictionarys.GetCombinations(s1, s2);
            float[] weights = possibleSpecies.Length > 2 ? BeeDictionarys.GetWeights(possibleSpecies) : new float[] { 0.5f, 0.5f };

            var randomNum = Rand(weights);
            var weightsSum = 0f;

            //* when the rumber generated is less than the current sum of the weights return that bee
            for (int i = 0; i < weights.Length; i++)
            {
                if(randomNum <= weightsSum)
                {
                    return possibleSpecies[i];
                }

                weightsSum += weights[i];
            }

            //* if for some reason the weights cannot work return the first bee in the combination list
            return possibleSpecies[0];
        }

        /// <summary>
        /// Returns a random float bewteen 0 and the sum of <paramref name="weights"/> rounded to 2dp
        /// </summary>
        /// <param name="weights">The weights</param>
        /// <returns><see cref="float"/> bewteen 0 and the sum of <paramref name="weights"/> rounded to 2dp</returns>
        private float Rand(float[] weights)
        {
            var totalWeights = 0f;

            //* sums the weights
            for (int i = 0; i < weights.Length; i++)
            {
                totalWeights += weights[i];
            }

            return (float)Math.Round(UnityEngine.Random.Range(0, totalWeights), 2);
        }

        /// <summary>
        /// Combines the <see cref="BeeLifeSpan"/> of the given <see cref="BeeLifeSpan"/>
        /// </summary>
        /// <param name="b1">Fist <see cref="BeeLifeSpan"/></param>
        /// <param name="b2">Second <see cref="BeeLifeSpan"/></param>
        /// <returns>A new <see cref="BeeLifeSpan"/></returns>
        private BeeLifeSpan CombineLifespan(BeeLifeSpan b1, BeeLifeSpan b2)
        {
            return (BeeLifeSpan)ReturnChange((int)b1, (int)b2, (int)BeeLifeSpan.SEATURTLE);
        }

        /// <summary>
        /// Combines the fertility of the given fertility
        /// </summary>
        /// <param name="b1">Fist <see cref="Bee"/>s fertility</param>
        /// <param name="b2">Second <see cref="Bee"/>s fertility</param>
        /// <returns>A new fertility, <see cref="uint"/></returns>
        private uint CombineFertility(uint b1, uint b2)
        {
            return (uint)ReturnChange((int)b1, (int)b2, 5, 1);
        }

        /// <summary>
        /// Combines the <see cref="BeeEffect"/> of the given <see cref="BeeEffect"/>
        /// </summary>
        /// <param name="b1">Fist <see cref="BeeEffect"/></param>
        /// <param name="b2">Second <see cref="BeeEffect"/></param>
        /// <returns>A new <see cref="BeeEffect"/></returns>
        private BeeEffect CombineEffect(BeeEffect b1, BeeEffect b2)
        {
            return (BeeEffect)ReturnChange((int)b1, (int)b2, (int)BeeEffect.POISON);
        }

        /// <summary>
        /// Combines the <see cref="BeeProductionSpeed"/> of the given <see cref="BeeProductionSpeed"/>
        /// </summary>
        /// <param name="b1">Fist <see cref="BeeProductionSpeed"/></param>
        /// <param name="b2">Second <see cref="BeeProductionSpeed"/></param>
        /// <returns>A new <see cref="BeeProductionSpeed"/></returns>
        public  BeeProductionSpeed CombineProductionSpeed(BeeProductionSpeed b1, BeeProductionSpeed b2)
        {
            return (BeeProductionSpeed)ReturnChange((int)b1, (int)b2, (int)BeeProductionSpeed.FAST);
        }

        /// <summary>
        /// Returns a number between <paramref name="maxChange"/> and <paramref name="minChange"/> based of <paramref name="b1"/> and <paramref name="b2"/>
        /// </summary>
        /// <param name="b1">First number</param>
        /// <param name="b2">Second number</param>
        /// <param name="maxChange">Max return value</param>
        /// <param name="minChange">Min return value</param>
        /// <returns>A number between <paramref name="maxChange"/> and <paramref name="minChange"/></returns>
        /// <remarks>
        /// If <paramref name="b1"/> and <paramref name="b2"/> are the same their is still a chance of change due to this function also takeing <see cref="mutationMultiplyer"/>, the value of wich is dictated by the apairy
        /// </remarks>
        private int ReturnChange(int b1, int b2, int maxChange, int minChange = 0)
        {
            //* b1 and b2 are checked for which one is bigger than the other here as the 
            //* queen my have a lower stat the an the drone and the drone is always passed in second
            var change = UnityEngine.Random.Range(b1 < b2 ? b1 : b2, (b2 > b1 ? b2 : b1) + 2);

            //* this will make it possible for the bees to mutate during combination of the stats are the same
            //* it will also cause more random mutation more mimicing nature
            change += UnityEngine.Random.Range(-mutationMultiplyer, mutationMultiplyer);

            //* as all but on ef the stats are enums they have a min/max value so need to check that this is not exceded
            if (change > maxChange)
                change = maxChange;
            else if (minChange > change)
                change = minChange;

            return change;

        }
        #endregion
    }
}
