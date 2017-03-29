using BeeGame.Inventory;
using BeeGame.Items;
using BeeGame.Enums;
using BeeGame.Core;
using System;
using UnityEngine.UI;

namespace BeeGame.Bee
{
    /// <summary>
    /// The Apiary. Where bees are combined and such things
    /// </summary>
    /// <remarks>
    /// <see cref="InventoryBase.inventoryGUI"/> in index 25 is where the queen bee is when it is combining with a drone bee
    /// </remarks>
    public class Apiary : ChestInventory
    {
        #region Data
        /// <summary>
        /// Is the Apiary Makeing bees?
        /// </summary>
        private bool isCombining;
        /// <summary>
        /// A new random numeber maker
        /// </summary>
        private System.Random rand;
        /// <summary>
        /// Affects how likely the bee is to mutate when it is combineing
        /// </summary>
        /// <remarks>
        /// Currently Unused
        /// </remarks>
        private int mutationMultiplyer = 1;

        /// <summary>
        /// The ticker slider
        /// </summary>
        public Slider ticker;
        /// <summary>
        /// The bee currently combineing
        /// </summary>
        public BeeData combiningBee;
        /// <summary>
        /// bees currently in the combeination slots
        /// </summary>
        public BeeData[] bees;
        /// <summary>
        /// bees to be made into items
        /// </summary>
        public BeeData[] newBees;
        #endregion

        #region UI Suff
        void Update()
        {
            UpdateChest();
            ReduceTimer();
        }
        
        /// <summary>
        /// Reduces the combination timer in the apiary UI. Speed of timer depends on bee lifespan
        /// </summary>
        void ReduceTimer()
        {
            //if the apiary is combining bees reduce the timer
            if(isCombining)
            {
                //if the queen bee is removed from the apiary stop the timer and also make it so that new bees will not be made
                if(inventoryGUI[25].item.itemId == null)
                {
                    isCombining = false;
                    newBees = null;
                }
                else if(ticker.value > 0)
                {
                    ticker.value -= (float)bees[0].pLifespan / 100f;
                }
                else
                {
                    //if the timer has reached 0 and the bees are still their make their items as set isCombining to false as the apiary is not makeing new bees at the moment
                    MakeItems(ReturnItems(newBees));
                    isCombining = false;
                }
            }
            else
            {
                //if the apiary is not combineing chack for bees and if they are cound start the combination process
                isCombining = CheckForBees();
            }
        }
        #endregion

        #region Return Made Items/Bees to Inventory
        /// <summary>
        /// Puts the items made by the apiary into the spot where they are supposed to be (in the inventory)
        /// </summary>
        /// <param name="_items"><see cref="Item"/></param>
        void MakeItems(Item[] _items)
        {
            //If items have been made (should never be null when this is called)
            if (_items != null)
            {
                for (int i = 0; i < _items.Length; i++)
                {
                    //if their is space in the inventory for the item put it in
                    for (int h = inventoryGUI.Length - 1; h >= 27; h--)
                    {
                        if (inventoryGUI[h].item.itemId == null)
                        {
                            inventoryGUI[h].item = _items[i];
                            break;
                        }
                    }
                }
            }

            //Removes the queen from the inventory slot
            inventoryGUI[25].item = new Item(); //should this happen before makeing the resulting items or is this fine?
        }

        /// <summary>
        /// Ite items that each of the bee species should make will be returned here
        /// </summary>
        /// <param name="_bees"></param>
        /// <returns><see cref="Item[]"/> so that they cam be put into the apiary inventory</returns>
        /// <remarks>
        /// other items made by the bee are not being returned (eg. HoneyCombs)
        /// </remarks>
        Item[] ReturnItems(BeeData[] _bees)
        {
            //the first and second bees made will always bee a Drone and a Princess
            _bees[0].beeType = BeeType.DRONE;
            _bees[1].beeType = BeeType.PRINCESS;

            //all other bees will be a random type
            for (int i = 2; i < _bees.Length; i++)
            {
                _bees[i].beeType = (BeeType)rand.Next(1, 3);
            }

            //make a new item arry for each of the bees
            Item[] items = new Item[_bees.Length];

            //updates the bee data for all of the bees produced
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new Item("1");

                items[i].UpdateBeeData(_bees[i]);
            }

            return items;
        }
        #endregion

        #region Combine The Bees
        /// <summary>
        /// Checks if their is bees in the Apiary
        /// </summary>
        /// <returns><see cref="bool"/></returns>
        bool CheckForBees()
        {
            bees = GetBees();

            if (CheckSlotsEmpty())
            {
                if (bees != null)
                {
                    //if their are bees and the bee in the top slot is a queen continue its combineing
                    if(bees[0].beeType == BeeType.QUEEN)
                    {
                        CombineBees(BeeType.QUEEN);
                        return true;
                    }
                    //if the bee in the top slot is a princess and the bee in the bottom slot is a drone begin the combination process
                    else if (bees[0].beeType == BeeType.PRINCESS && bees[1].beeType == BeeType.DRONE)
                    {
                        CombineBees(BeeType.PRINCESS);
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Combines the 2 bees pout into the apiary
        /// </summary>
        /// <param name="type">is the bee a <see cref="BeeType.QUEEN"/> or a <see cref="BeeType.PRINCESS"/></param>
        void CombineBees(BeeType type)
        {
            rand = new System.Random();

            switch (type)
            {
                //if the bee is a queen then nothing needs to be done to it and just passes its data along
                case BeeType.QUEEN:
                    combiningBee = bees[0];
                    break;
                //if the bee is a princess then it should now bee a queen and all the data requred from the drone bee is given to the new queen and the old drone is removed
                case BeeType.PRINCESS:
                    combiningBee = bees[0];
                    combiningBee.combiningData.ToCombiningBeeData(bees[1]);
                    inventoryGUI[25].item.beeItem = combiningBee;
                    inventoryGUI[25].item.SetBeeType(BeeType.QUEEN);
                    inventoryGUI[26].item = new Item();
                    break;
                default:
                    break;
            }
            
            //makes a new bee array, size of array depends on the fertility of the bee
            newBees = new BeeData[combiningBee.pFertility];

            for (int i = 0; i < newBees.Length; i++)
            {
                newBees[i] = NewBee(bees);
            }

            //sets the ticker mas value and current value to the bees lifespan as a float
            ticker.maxValue = (float)bees[0].pLifespan;
            ticker.value = ticker.maxValue;
        }
        
        #region Bee Getters
        /// <summary>
        /// Checks if the slots that the new items will be put into are empty
        /// </summary>
        /// <returns><see cref="true"/> if the slots are empty</returns>
        bool CheckSlotsEmpty()
        {
            for (int i = inventoryGUI.Length - 1; i >= 27; i--)
            {
                if(inventoryGUI[i].item.itemId != null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Gets the Items in <see cref="inventoryGUI"/> positons 25 - 26
        /// </summary>
        /// <returns><see cref="BeeData[]"/> of the bees in <see cref="inventoryGUI"/> positons 25 - 26</returns>
        BeeData[] GetBees()
        {
            BeeData bee1 = new BeeData();
            BeeData bee2 = new BeeData();

            bool returnNull = true;

            if (inventoryGUI[25].item.itemId != null)
            {
                if (inventoryGUI[25].item.itemType == ItemType.BEE)
                {
                    bee1 = inventoryGUI[25].item.ReturnBeeData();
                    returnNull = false;
                }
            }
            if (inventoryGUI[26].item.itemId != null)
            {
                if (inventoryGUI[26].item.itemType == ItemType.BEE)
                {
                    bee2 = inventoryGUI[26].item.ReturnBeeData();
                }
            }

            if(!returnNull)
            {
                return new BeeData[2] { bee1, bee2 };
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Resulting Bee Makers
        /// <summary>
        /// Makes a New Bee missing the bee type
        /// </summary>
        /// <param name="_bees"><see cref="BeeData"/> without a <see cref="BeeData.beeType"/></param>
        BeeData NewBee(BeeData[] _bees)
        {
            BeeData newBee = new BeeData();

            // Setting the primary stats of the bee (Phenotype)
            newBee.pSpecies = ReturnBeeSpecies(combiningBee);
            newBee.pLifespan = (BeeLifeSpan)ReturnChange((int)combiningBee.sLifespan, (int)combiningBee.combiningData.sLifespan, (int)BeeLifeSpan.SEATURTLE);
            newBee.pFertility = (uint)ReturnChange((int)combiningBee.sFertility, (int)combiningBee.combiningData.sFertility, 5, 2);
            newBee.pEffect = (BeeEffect)ReturnChange((int)combiningBee.sEffect, (int)combiningBee.combiningData.sEffect, (int)BeeEffect.POSION);
            newBee.pProdSpeed = (BeeProductionSpeed)ReturnChange((int)combiningBee.sProdSpeed, (int)combiningBee.combiningData.sProdSpeed, (int)BeeProductionSpeed.FAST);
            
            // Setting the secondary trait of the bee (Secondary)
            newBee.sSpecies = ReturnBeeSpecies(combiningBee);
            newBee.sLifespan = (BeeLifeSpan)ReturnChange((int)combiningBee.sLifespan, (int)combiningBee.combiningData.sLifespan, (int)BeeLifeSpan.SEATURTLE);
            newBee.sFertility = (uint)ReturnChange((int)combiningBee.sFertility, (int)combiningBee.combiningData.sFertility, 5, 2);
            newBee.sEffect = (BeeEffect)ReturnChange((int)combiningBee.sEffect, (int)combiningBee.combiningData.sEffect, (int)BeeEffect.POSION);
            newBee.sProdSpeed = (BeeProductionSpeed)ReturnChange((int)combiningBee.sProdSpeed, (int)combiningBee.combiningData.sProdSpeed, (int)BeeProductionSpeed.FAST);
            
            // Sets the tempPref and tempTol of the new bee
            newBee.tempPref = (BeeTempPreferance)ReturnChange((int)combiningBee.tempPref, (int)combiningBee.combiningData.tempPref, (int)BeeTempPreferance.HELL);
            newBee.tempTol = TempTol(combiningBee);
            
            // Sets the humidPref and humidTol of the new bee
            newBee.humidPref = (BeeHumidityPreferance)ReturnChange((int)combiningBee.humidPref, (int)combiningBee.combiningData.humidPref, (int)BeeHumidityPreferance.HUMID);
            newBee.humidTol = HumidityTol(combiningBee);

            newBee.nocturnal = ReturnNocturnal(combiningBee);
            newBee.flyer = ReturnFlyer(combiningBee);

            return newBee;
        }

        #region Resulting Species
        /// <summary>
        /// Returns the new bee species that the bees allel is based on the combination weights see: <see cref="BeeDictionarys.beeMutationChance"/>
        /// </summary>
        /// <param name="bee1"><see cref="BeeData"/></param>
        /// <param name="bee2"><see cref="BeeData"/></param>
        /// <returns><see cref="BeeSpecies"/></returns>
        BeeSpecies ReturnBeeSpecies(BeeData bee)
        {
            BeeSpecies[] possibleCombinationTypes = BeeDictionarys.GetCombination(bee.sSpecies, bee.combiningData.sSpecies);
            BeeSpecies[] possibleTypes;
            float[] weights;

            if (possibleCombinationTypes != null)
            {
                possibleTypes = new BeeSpecies[possibleCombinationTypes.Length + 2];
                weights = GetWeights(possibleCombinationTypes);
                
                for (int i = 0; i < possibleCombinationTypes.Length; i++)
                {
                    possibleTypes[i] = possibleCombinationTypes[i];
                }
            }
            else
            {
                possibleTypes = new BeeSpecies[2];
                weights = new float[2] { 0.5f, 0.5f };
            }
            possibleTypes[possibleTypes.Length - 2] = bee.sSpecies;
            possibleTypes[possibleTypes.Length - 1] = bee.combiningData.sSpecies;

            var randomNum = Rand(weights);
            var weightsSum = 0f;

            for (int i = 0; i < weights.Length; i++)
            {
                if(randomNum <= weightsSum)
                {
                    return possibleTypes[i];
                }

                weightsSum += weights[i];
            }

            return 0;
        }

        #region Weighted Randomnes
        /// <summary>
        /// Will generate a random number between 0 and the total for the given weights
        /// </summary>
        /// <param name="weights">The chance of the the bee conveting into that species</param>
        /// <returns>Float between 0 - weight total</returns>
        float Rand(float[] weights)
        {
            var totalWeights = 0f;

            for (int i = 0; i < weights.Length; i++)
            {
                totalWeights += weights[i];
            }

            return (float)Math.Round(UnityEngine.Random.Range(0, totalWeights), 2);
        }

        /// <summary>
        /// Gets the weights for the given bee species
        /// </summary>
        /// <param name="types"><see cref="BeeSpecies[]"/></param>
        /// <returns><see cref="float[]"> for the <see cref="BeeSpecies"/> weights</returns>
        float[] GetWeights(BeeSpecies[] types)
        {
            float[] weights = new float[types.Length + 2];
            var totalWeight = 0f;

            for (int i = 0; i < types.Length; i++)
            {
                var weight = BeeDictionarys.GetMutationChance(types[i]);
                weights[i] = weight;
                totalWeight += weight;
            }

            var half = (1f - totalWeight) / 2;

            weights[weights.Length - 2] = half;
            weights[weights.Length - 1] = half;

            return weights;
        }
        #endregion
        #endregion

        /// <summary>
        /// Returns the ammout of change compared to the old bee the new bees stats will be
        /// </summary>
        /// <param name="bee1"><see cref="BeeData"/>.<see cref="enum"/></param>
        /// <param name="bee2"><see cref="BeeData"/>.<see cref="enum"/></param>
        /// <param name="maxChange">The maximum ammout of chnage that a stat can have</param>
        /// <param name="minChange">The minimum ammout of chnage a stat can have</param>
        /// <returns><see cref="int"/> that reperesents a <see cref="enum"/> value</returns>
        int ReturnChange(int bee1, int bee2, int maxChange, int minChange = 0)
        {
            var change = 0;
            if (bee1 < bee2)
            {
                change = rand.Next(bee1, bee2 + 1);
            }
            else
            {
                change = rand.Next(bee2, bee1 + 1);
            }

            change += rand.Next(-mutationMultiplyer, mutationMultiplyer);

            if (change < minChange)
            {
                return minChange;
            }
            else if (change > maxChange)
            {
                return maxChange;
            }
            return change;
        }

        #region Combination Helper
        /// <summary>
        /// REturms a random humidity tolerance of one of the parents
        /// </summary>
        /// <param name="bee">a <see cref="BeeData.tempTol"/> with the <see cref="BeeData.combiningData"/> both with a non null <see cref="BeeData.tempTol"/></param>
        /// <returns></returns>
        int[] TempTol(BeeData bee)
        {
            BeeData[] _bees = new BeeData[2] { bee, bee.combiningData.ToBeeData() };
            return _bees[rand.Next(0, 2)].tempTol;
        }

        /// <summary>
        /// Combines the <see cref="BeeData.humidTol"/>
        /// </summary>
        /// <param name="bee">a <see cref="BeeData.humidTol"/> with the <see cref="BeeData.combiningData"/> both with a non null <see cref="BeeData.humidTol"/></param>
        /// <returns>one of the bees humidity tolerances</returns>
        /// <remarks>
        /// Useing this means the himud tol can never chnage via random mutation.
        /// Do I realy want this?
        /// </remarks>
        int[] HumidityTol(BeeData bee)
        {
            BeeData[] _bees = new BeeData[2] { bee, bee.combiningData.ToBeeData() };
            return _bees[rand.Next(0, 2)].humidTol;
        }

        /// <summary>
        /// Is the bee nocturnal. Both bees must not have <see cref="BeeData.nocturnal"/> is not null
        /// </summary>
        /// <param name="bee">a <see cref="BeeData.nocturnal"/> with the <see cref="BeeData.combiningData"/> both with a non null <see cref="BeeData.nocturnal"/></param>
        /// <returns><see cref="bool?"/></returns>
        /// <remarks>
        /// Means that a random mutation cannot occur giving a <see cref="BeeData.nocturnal"/> value different to the parents
        /// </remarks>
        bool? ReturnNocturnal(BeeData bee)
        {
            BeeData[] _bees = new BeeData[2] { bee, bee.combiningData.ToBeeData() };
            return _bees[rand.Next(0, 2)].nocturnal;
        }

        /// <summary>
        /// Is the bee a flyer. Both bees must not have <see cref="BeeData.flyer"/> is not null
        /// </summary>
        /// <param name="bees">a <see cref="BeeData.flyer"/> with the <see cref="BeeData.combiningData"/> both with a non null <see cref="BeeData.flyer"/></param>
        /// <returns><see cref="bool?"/></returns>
        /// <remarks>
        /// Same remarks as for <see cref="ReturnNocturnal(BeeData)"/> but replace <see cref="BeeData.nocturnal"/> with <see cref="BeeData.flyer"/>
        /// </remarks>
        bool? ReturnFlyer(BeeData bee)
        {
            BeeData[] _bees = new BeeData[2] { bee, bee.combiningData.ToBeeData() };
            return _bees[rand.Next(0, 2)].flyer;
        }
        #endregion
        #endregion
        #endregion
    }
}