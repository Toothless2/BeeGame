using BeeGame.Inventory;
using BeeGame.Items;
using BeeGame.Enums;
using BeeGame.Core;
using System;
using UnityEngine.UI;
using UnityEngine;

namespace BeeGame.Bee
{
    /// <summary>
    /// \todo Add summarys to this
    /// </summary>
    public class Apiary : ChestInventory
    {
        private bool isCombining;
        private System.Random rand;
        private int mutationMultiplyer = 1;

        public Slider ticker;
        public BeeData combiningBee;
        public BeeData[] bees;
        public BeeData[] newBees;

        void Update()
        {
            UpdateChest();
            ReduceTimer();
        }

        void ReduceTimer()
        {
            if(isCombining)
            {
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
                    MakeItems(ReturnItems(newBees));
                    isCombining = false;
                }
            }
            else
            {
                isCombining = CheckForBees();
            }
        }

        bool CheckForBees()
        {
            bees = GetBees();

            if (CheckSlotsEmpty())
            {
                if (bees != null)
                {
                    if(bees[0].beeType == BeeType.QUEEN)
                    {
                        CombineBees(BeeType.QUEEN);
                        return true;
                    }
                    else if (bees[0].beeType == BeeType.PRINCESS && bees[1].beeType == BeeType.DRONE)
                    {
                        CombineBees(BeeType.PRINCESS);
                        return true;
                    }
                }
            }

            return false;
        }

        void CombineBees(BeeType type)
        {
            rand = new System.Random();

            switch (type)
            {
                case BeeType.QUEEN:
                    combiningBee = bees[0];
                    break;
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
            
            newBees = new BeeData[combiningBee.pFertility];

            for (int i = 0; i < newBees.Length; i++)
            {
                newBees[i] = NewBee(bees);
            }

            ticker.maxValue = (float)bees[0].pLifespan;
            ticker.value = ticker.maxValue;
        }

        void MakeItems(Item[] _items)
        {
            if (_items != null)
            {
                for (int i = 0; i < _items.Length; i++)
                {
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

            inventoryGUI[25].item = new Item();
        }

        Item[] ReturnItems(BeeData[] _bees)
        {
            _bees[0].beeType = BeeType.DRONE;
            _bees[1].beeType = BeeType.PRINCESS;

            for (int i = _bees.Length - 1; i >= 2; i--)
            {
                _bees[i].beeType = (BeeType)rand.Next(1, 3);
            }

            Item[] items = new Item[_bees.Length];

            for (int i = 0; i < items.Length; i++)
            {
                items[i] = new Item("1");

                items[i].UpdateBeeData(_bees[i]);
            }

            return items;
        }

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

        /// <summary>
        /// Makes a New Bee missing the bee type
        /// </summary>
        /// <param name="_bees"><see cref="BeeData"/> without a <see cref="BeeData.beeType"/></param>
        BeeData NewBee(BeeData[] _bees)
        {
            BeeData newBee = new BeeData();

            ///<summary>
            /// Setting the primary stats of the bee (Phenotype)
            ///</summary>
            newBee.pSpecies = ReturnBeeSpecies(combiningBee);
            newBee.pLifespan = (BeeLifeSpan)ReturnChange((int)combiningBee.sLifespan, (int)combiningBee.combiningData.sLifespan, (int)BeeLifeSpan.SEATURTLE);
            newBee.pFertility = (uint)ReturnChange((int)combiningBee.sFertility, (int)combiningBee.combiningData.sFertility, 5, 2);
            newBee.pEffect = (BeeEffect)ReturnChange((int)combiningBee.sEffect, (int)combiningBee.combiningData.sEffect, (int)BeeEffect.POSION);
            newBee.pProdSpeed = (BeeProductionSpeed)ReturnChange((int)combiningBee.sProdSpeed, (int)combiningBee.combiningData.sProdSpeed, (int)BeeProductionSpeed.FAST);

            ///<summary>
            /// Setting the secondary trait of the bee (Secondary)
            ///</summary>
            newBee.sSpecies = ReturnBeeSpecies(combiningBee);
            newBee.sLifespan = (BeeLifeSpan)ReturnChange((int)combiningBee.sLifespan, (int)combiningBee.combiningData.sLifespan, (int)BeeLifeSpan.SEATURTLE);
            newBee.sFertility = (uint)ReturnChange((int)combiningBee.sFertility, (int)combiningBee.combiningData.sFertility, 5, 2);
            newBee.sEffect = (BeeEffect)ReturnChange((int)combiningBee.sEffect, (int)combiningBee.combiningData.sEffect, (int)BeeEffect.POSION);
            newBee.sProdSpeed = (BeeProductionSpeed)ReturnChange((int)combiningBee.sProdSpeed, (int)combiningBee.combiningData.sProdSpeed, (int)BeeProductionSpeed.FAST);

            ///<summary>
            /// Sets the tempPref and tempTol of the new bee
            ///</summary>
            newBee.tempPref = (BeeTempPreferance)ReturnChange((int)combiningBee.tempPref, (int)combiningBee.combiningData.tempPref, (int)BeeTempPreferance.HELL);
            newBee.tempTol = TempTol(combiningBee);

            ///<summary>
            /// Sets the humidPref and humidTol of the new bee
            ///</summary>
            newBee.humidPref = (BeeHumidityPreferance)ReturnChange((int)combiningBee.humidPref, (int)combiningBee.combiningData.humidPref, (int)BeeHumidityPreferance.HUMID);
            newBee.humidTol = HumidityTol(combiningBee);

            newBee.nocturnal = ReturnNocturnal(combiningBee);
            newBee.flyer = ReturnFlyer(combiningBee);

            return newBee;
        }

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

        /// <summary>
        /// Returns the ammout of change compared to the old bee the new bees stats will be
        /// </summary>
        /// <param name="bee1"><see cref="BeeData"/>.<see cref="enum"/></param>
        /// <param name="bee2"><see cref="BeeData"/>.<see cref="enum"/></param>
        /// <param name="maxChange">The maximum ammout of chnage that a stat can have</param>
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

        int[] TempTol(BeeData bee)
        {
            BeeData[] _bees = new BeeData[2] { bee, bee.combiningData.ToBeeData() };
            return _bees[rand.Next(0, 2)].tempTol;
        }

        int[] HumidityTol(BeeData bee)
        {
            BeeData[] _bees = new BeeData[2] { bee, bee.combiningData.ToBeeData() };
            return _bees[rand.Next(0, 2)].humidTol;
        }

        /// <summary>
        /// Is the bee nocturnal. Both bees must not have <see cref="BeeData.nocturnal"/> is not null
        /// </summary>
        /// <param name="bees">2 bees both with <see cref="BeeData.nocturnal"/> not null</param>
        /// <returns><see cref="bool?"/></returns>
        bool? ReturnNocturnal(BeeData bee)
        {
            BeeData[] _bees = new BeeData[2] { bee, bee.combiningData.ToBeeData() };
            return _bees[rand.Next(0, 2)].nocturnal;
        }

        /// <summary>
        /// Is the bee a flyer. Both bees must not have <see cref="BeeData.flyer"/> is not null
        /// </summary>
        /// <param name="bees">2 bees both with <see cref="BeeData.flyer"/> not null</param>
        /// <returns><see cref="bool?"/></returns>
        bool? ReturnFlyer(BeeData bee)
        {
            BeeData[] _bees = new BeeData[2] { bee, bee.combiningData.ToBeeData() };
            return _bees[rand.Next(0, 2)].flyer;
        }
    }
}