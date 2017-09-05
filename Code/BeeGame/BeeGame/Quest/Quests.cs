using System.Collections.Generic;
using BeeGame.Items;
using BeeGame.Blocks;
using BeeGame.Core.Enums;
using BeeGame.Exceptions;

namespace BeeGame.Quest
{
    [System.Serializable]
    public static class Quests
    {
        /// <summary>
        /// Quests that the player has compleated
        /// </summary>
        private static Dictionary<string, object[]> compleatedQuests = new Dictionary<string, object[]>();

        /// <summary>
        /// Quests that have been compleated but the rewards have not been claimed
        /// </summary>
        private static Dictionary<string, object[]> compleatedUnclaimedQuests = new Dictionary<string, object[]>();

        /// <summary>
        /// Quests the player is currently finishing
        /// </summary>
        private static Dictionary<string, object[]> currentQuests = new Dictionary<string, object[]>()
        {
            { $"Pickup: {Wood.ID}", new object[] {new CraftingTable(), $"Crafted: {Grass.ID}", "Pickup A Wood Block" } },
            {$"BeeCrafted: {BeeSpecies.COMMON}", new object[] {new CraftingTable(), "Make a Common Bee"} }
        };

        /// <summary>
        /// Quests that the player does not have accest to yet
        /// </summary>
        private static Dictionary<string, object[]> lockedQuests = new Dictionary<string, object[]>()
        {
            { $"Crafted: {Grass.ID}", new object[] {new Dirt(), "nothing", "Use some Dirt in the Workbench" } }
        };

        public static Dictionary<string, object[]> ReturnCompleatedQuests()
        {
            return compleatedUnclaimedQuests;
        }

        public static Dictionary<string, object[]> ReturnCompleatedClaimedQuests()
        {
            return compleatedQuests;
        }

        public static Dictionary<string, object[]> ReturnCurrentQuests()
        {
            return currentQuests;
        }
        
        public static Dictionary<string, object[]> ReturnLockedQuests()
        {
            return lockedQuests;
        }

        public static void LoadQuests(Dictionary<string, object[]> compleated, Dictionary<string, object[]> compleatedNotCollected, Dictionary<string, object[]> inProgress, Dictionary<string, object[]> locked)
        {
            compleatedQuests = compleated;
            compleatedUnclaimedQuests = compleatedNotCollected;
            currentQuests = inProgress;
            lockedQuests = locked;
        }

        public static void ClaimQuest(string key)
        {
            var item = compleatedUnclaimedQuests[key];
            compleatedQuests.Add(key, compleatedUnclaimedQuests[key]);
            compleatedUnclaimedQuests.Remove(key);

            var temp = UnityEngine.Object.Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as UnityEngine.GameObject, UnityEngine.Object.FindObjectOfType<Player.PlayerMove>().transform.position + UnityEngine.Vector3.one, UnityEngine.Quaternion.identity);
            temp.GetComponent<ItemGameObject>().item = (Item)item[0];
        }

        public static void AddQuest(string quest, Item result, string nextQuest)
        {
            if(!lockedQuests.ContainsKey(quest))
            {
                lockedQuests.Add(quest, new object[] { result, nextQuest});
                return;
            }
            else
            {
                lockedQuests[quest] = new object[] { result, nextQuest };

                throw new QuestAlreadyExistsException($"Warning Quest: {quest}, is already a quest and the old has been overriden!");
            }
        }

        public static void ReturnPickupQuest(int pickupID)
        {
            if (currentQuests.ContainsKey($"Pickup: {pickupID}"))
            {
                UnlockQuests($"Pickup: {pickupID}");
            }
        }

        public static void ReturnCraftingTableQuest(int craftedItemID)
        {
            if (currentQuests.ContainsKey($"Crafted: {craftedItemID}"))
            {
                UnlockQuests($"Crafted: {craftedItemID}");
            }
        }

        public static void ReturnBeeCraftingQuest(BeeSpecies primaryBeeSpecies)
        {
            if (currentQuests.ContainsKey($"BeeCrafted: {primaryBeeSpecies}"))
            {
                UnlockQuests($"BeeCrafted: {primaryBeeSpecies}");
            }
        }

        public static void ReturnPureBreadBeeCraftingQuest(BeeSpecies primaryBeeSpecies, BeeSpecies secondaryBeeSpecies)
        {
            if (currentQuests.ContainsKey($"PureBredBee: {primaryBeeSpecies} {secondaryBeeSpecies}"))
            {
                UnlockQuests($"PureBredBee: {primaryBeeSpecies} {secondaryBeeSpecies}");
            }
        }

        private static void UnlockQuests(string key)
        {
            compleatedUnclaimedQuests.Add(key, currentQuests[key]);

            var objArray = currentQuests[key];

            if(objArray.Length > 2)
            {
                var next = objArray[1];

                if (next is string[] sa)
                {
                    foreach (var q in sa)
                    {
                        currentQuests.Add(q, lockedQuests[q]);
                        lockedQuests.Remove(q);
                    }
                }
                else if (next is string ss)
                {
                    if (lockedQuests.ContainsKey(ss))
                    {
                        currentQuests.Add(ss, lockedQuests[ss]);
                        lockedQuests.Remove(ss);
                    }
                }
            }

            currentQuests.Remove(key);

            Serialization.Serialization.SaveQuests();
        }
    }
}
