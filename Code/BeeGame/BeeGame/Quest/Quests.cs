using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Enums;
using BeeGame.Core;

namespace BeeGame.Quest
{
    [Serializable]
    public static class Quests
    {
        #region Data
        /// <summary>
        /// The Dictionary that contains all of the completed quests
        /// </summary>
        private static Dictionary<object, object> completedQuests = new Dictionary<object, object>();

        /// <summary>
        /// The Dictionary that contains all of the cirrently avaliable Quests
        /// </summary>
        private static Dictionary<object, object> avaliableQuests = new Dictionary<object, object>()
        {
            { new string[2] {BeeSpecies.FOREST.ToString(), BeeSpecies.MEADOWS.ToString() }, false }
        };

        /// <summary>
        /// Dinctionary that contains all of the currently locked quests
        /// </summary>
        private static Dictionary<object, object> lockedQuests = new Dictionary<object, object>()
        {
            {new string[2] {BeeSpecies.FOREST.ToString(), BeeSpecies.MEADOWS.ToString() }, BeeSpecies.COMMON.ToString() },
            {BeeSpecies.COMMON.ToString(), new object[2] { BeeSpecies.CULTIVATED.ToString(), new object[2] { BeeSpecies.DILIGENT.ToString(), null } } },
            {BeeSpecies.CULTIVATED.ToString(), new object[2] { BeeSpecies.UNWEARY.ToString(), new object[2] {BeeSpecies.INDUSTRIOUS.ToString(), new object[2] { BeeSpecies.MAJESTIC.ToString(), BeeSpecies.AGRARIAN.ToString() } } } }
        };
        #endregion

        #region Serialization
        /// <summary>
        /// Deserializes the quest dictionarys
        /// </summary>
        /// <param name="questDictionarys">raw deserialized data from the deserializer</param>
        public static void ApplyDeserializedDictionarys(object[] questDictionarys)
        {
            completedQuests = (Dictionary<object, object>)questDictionarys[0];
            avaliableQuests = (Dictionary<object, object>)questDictionarys[1];
            lockedQuests = (Dictionary<object, object>)questDictionarys[2];
        }

        /// <summary>
        /// Only used for serialization
        /// </summary>
        /// <returns>Returns all 3 quest dictionarys as a <see cref="object[]"/></returns>
        public static object[] ReturnQuestDictionarys()
        {
            return new object[3] { completedQuests, avaliableQuests, lockedQuests };
        }
        #endregion

        #region Initialization
        public static void SetQuestEvents()
        {
            QuestEvents.beeSpeciesMade += BeeSpeciesMade;
        }
        #endregion

        #region Quest Events

        public static void BeeSpeciesMade(string beeSpecies, EventArgs e)
        {
            foreach (var key in avaliableQuests.Keys)
            {
                //if the key contains the bee
                if (key is object[] keyArry && keyArry.Contains(beeSpecies))
                {
#if DEBUG
                    Debug.Log($"Bee speceies made: {beeSpecies}");
#endif
                    ShouldCompleteQuest(key, beeSpecies);
                    //break is hear is if it wasnt the keys in the dictionary would have changed whilst itterating and continuing to iterate would cause errors
                    break;
                }
                //if the bee is in a keys data array complate that bees creation for the quest
                else if((avaliableQuests[key] is object[] array && array.Contains(beeSpecies)))
                {
#if DEBUG
                    Debug.Log($"Bee speceies made: {beeSpecies}");
#endif
                    ShouldCompleteQuest(key, beeSpecies);
                    break;
                }
                else if (key is string)
                {
                    if (key.ToString() == beeSpecies)
                    {
#if DEBUG
                        Debug.Log($"Bee speceies made: {beeSpecies}");
#endif
                        ShouldCompleteQuest(key, beeSpecies);
                        //break is hear is if it wasnt the keys in the dictionary would have changed whilst itterating and continuing to iterate would cause errors
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Should the quest just triggered be completed?
        /// </summary>
        /// <param name="key">The key in the <see cref="avaliableQuests"/> dictionary</param>
        /// <param name="beeSpecies">If this method is called from <see cref="BeeSpeciesMade(string)"/> this is the bee that was just made</param>
        /// <param name="reduceNumberBy">The number to reduce a hand in quest by</param>
        static void ShouldCompleteQuest(object key, string beeSpecies = null, int? reduceNumberBy = null)
        {
            bool shouldComplete = true;

            //is the given key data an array?
            if (avaliableQuests[key] is object[] dictArrayData)
            {
                //Are we trying to complete a bee quest?
                if (beeSpecies != null)
                {
                    //if the given species is in the dictArrayData remove that bee from the quest line
                    if (dictArrayData.Contains(beeSpecies))
                    {
                        dictArrayData.SetValue(null, Array.IndexOf(dictArrayData, beeSpecies));
                    }
                }

                //If every part of the quest is completed (all indexes bar the first are null) this quest should be completed
                for (int i = 0; i < dictArrayData.Length; i++)
                {
                    if (dictArrayData[i] != null)
                    {
                        shouldComplete = false;
                    }
                }

                //If the quest should be completed do it
                if (shouldComplete)
                {
                    Complete(key);
                    return;
                }
            }
            //if the data is a single int a quest will require a certian number of items to be handed in so when this is 0 the quest can be completed
            else if (avaliableQuests[key] is int dictIntData)
            {
                if (reduceNumberBy != null)
                {
                    if (dictIntData - (int)reduceNumberBy < 0)
                    {
                        Complete(key);
                    }
                    else
                    {
                        dictIntData -= (int)reduceNumberBy;

                        avaliableQuests[key] = dictIntData;
                    }
                }
            }
            else
            {
                //It the value of the dioctionary is not an array or its is not an int all parts of the quest have been completed
                Complete(key);
            }
            
            /// This method will only ever be called from within <see cref="ShouldCompleteQuest(object, string)"/> so no reason not to make it a nested method
            void Complete(object _key)
            {
                completedQuests.Add(_key, null);
                avaliableQuests.Remove(_key);
                ShouldOpenNewQuest(_key);
#if DEBUG
                Debug.Log($"Quest Completed: {key}");
#endif
            }
        }

        /// <summary>
        /// Will move quests from the locked quests dictionary to the avaliable dictionary if a given key in within the locked quest dictionary
        /// </summary>
        /// <param name="key"><see cref="lockedQuests"/> key to look for</param>
        private static void ShouldOpenNewQuest(object key)
        {
            //If locked quests contains a given key
            if (DoesLockedDictionaryContainKey(key) != null)
            {
                // if the value at the key is an array mean thay more than 1 quest will be unlocked by this quest
                if (ReturnLockedDictionaryValue(key) is object[] values)
                {
                    //for each of the values in the vlaues array unlock its quest
                    foreach (var dictValue in values)
                    {
                        //if the element in the array is also an array make the forst element a key and the second element the quest params
                        if (dictValue is object[] dictValueArray)
                        {
                            avaliableQuests.Add(dictValueArray[0], dictValueArray[1]);
                        }
                        else
                        {
                            avaliableQuests.Add(dictValue, null);
                        }
                    }
                }
                // Otherwise just unlock the quest
                else
                {
                    avaliableQuests.Add(ReturnLockedDictionaryValue(key), null);
                }

                lockedQuests.Remove(DoesLockedDictionaryContainKey(key));
            }
        }
        #endregion
        
        #region Locked Dictionary Helper Methods
        /// <summary>
        /// Returns the key for a given key in the dictionary
        /// </summary>
        /// <param name="_key">dictionary key</param>
        /// <returns><see cref="lockedQuests"/> key</returns>
        /// <remarks>
        /// This is nessicary because some keys in dictionary are arrays this means that putting the assumed key into the dictionary of it is not an array even if it is the same will not find the data that is being searched for
        /// </remarks>
        static object DoesLockedDictionaryContainKey(object _key)
        {
            //array of keys in the lockedQuest dictionary
            var keys = lockedQuests.Keys;

            //if the given key is an array
            if(_key is object[])
            {
                //check against every key in the dictionary and of they match retirn the key in the lockedQuest dcionarys
                foreach (var key in keys)
                {
                    if (key is object[] keyArry && Enumerable.SequenceEqual(keyArry, (object[])_key))
                    {
                        return key;
                    }
                }
            }
            else
            {
                //if _key is not an array check if the dionary contains it and return the key
                if (lockedQuests.ContainsKey(_key))
                {
                    return _key;
                }
            }
            //If the dictionary does not contain the given key
            return null;
        }

        /// <summary>
        /// Returns the data for a given <see cref="lockedQuests"/> key
        /// </summary>
        /// <param name="_key"><see cref="lockedQuests"/> key</param>
        /// <returns><see cref="lockedQuests"/> data at given key</returns>
        /// <remarks>
        /// This should not be neciessary however due to keys possibly being arrays it is
        /// </remarks>
        static object ReturnLockedDictionaryValue(object _key)
        {
            //array of all of the lockedQuest keys
            var keys = lockedQuests.Keys;
            
            foreach (var key in keys)
            {
                //if the _key is an array and the goen key is an array check if they are the same
                if(_key is object[] _keyArray && key is object[] keyArray && Enumerable.SequenceEqual(keyArray, _keyArray))
                {
                    //if they are the same return the lockedQuest data for that key
                    return lockedQuests[key];
                }
                //if the _keys is not an array check if the locekd quest dictionary contains it and return the data
                else if(lockedQuests.ContainsKey(_key))
                {
                    return lockedQuests[_key];
                }
            }
            //if the given key is not in the dictioary return null (this should never happen)
            return null;
        }
        #endregion
    }
}
