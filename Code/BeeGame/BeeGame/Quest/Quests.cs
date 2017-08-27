using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Items;
using BeeGame.Blocks;
using BeeGame.Core.Enums;
using BeeGame.Exceptions;

namespace BeeGame.Quest
{
    public static class Quests
    {
        private static Dictionary<string, Item> compleatedQuests = new Dictionary<string, Item>();

        private static Dictionary<string, object[]> currentQuests = new Dictionary<string, object[]>()
        {
            { $"Pickup: {Wood.ID}", new object[] {new CraftingTable(), $"Crafted: {Grass.ID}" } }
        };

        private static Dictionary<string, object[]> lockedQuests = new Dictionary<string, object[]>()
        {
            { $"Crafted: {Grass.ID}", new object[] {new Dirt(), "nothign" } }
        };

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
                currentQuests.Add(currentQuests[$"Pickup: {pickupID}"][1] as string, lockedQuests[(string)currentQuests[$"Pickup: {pickupID}"][1]]);
                lockedQuests.Remove($"Pickup: {pickupID}");

                compleatedQuests.Add($"Pickup: {pickupID}", currentQuests[$"Pickup: {pickupID}"][0] as Item);
            }
        }

        public static void ReturnCraftingTableQuest(int craftedItemID)
        {
            if (currentQuests.ContainsKey($"Crafted: {craftedItemID}"))
            {
                currentQuests.Add(currentQuests[$"Crafted: {craftedItemID}"][1] as string, lockedQuests[(string)currentQuests[$"Pickup: {craftedItemID}"][1]]);
                lockedQuests.Remove($"Crafted: {craftedItemID}");

                compleatedQuests.Add($"Crafted: {craftedItemID}", currentQuests[$"Crafted: {craftedItemID}"][0] as Item);
            }
        }

        public static void ReturnBeeCraftingQuest(BeeSpecies primaryBeeSpecies)
        {
            if (currentQuests.ContainsKey($"BeeCrafted: {primaryBeeSpecies}"))
            {
                currentQuests.Add(currentQuests[$"BeeCrafted: {primaryBeeSpecies}"][1] as string, lockedQuests[(string)currentQuests[$"BeeCrafted: {primaryBeeSpecies}"][1]]);
                lockedQuests.Remove($"BeeCrafted: {primaryBeeSpecies}");

                compleatedQuests.Add($"BeeCrafted: {primaryBeeSpecies}", currentQuests[$"BeeCrafted: {primaryBeeSpecies}"][0] as Item);
            }
        }

        public static void ReturnPureBreadBeeCraftingQuest(BeeSpecies primaryBeeSpecies, BeeSpecies secondaryBeeSpecies)
        {
            if (currentQuests.ContainsKey($"PureBredBee: {primaryBeeSpecies} {secondaryBeeSpecies}"))
            {
                currentQuests.Add(currentQuests[$"PureBredBee: {primaryBeeSpecies} {secondaryBeeSpecies}"][1] as string, lockedQuests[(string)currentQuests[$"PureBredBee: {primaryBeeSpecies} {secondaryBeeSpecies}"][1]]);
                lockedQuests.Remove($"PureBredBee: {primaryBeeSpecies} {secondaryBeeSpecies}");

                compleatedQuests.Add($"PureBredBee: {primaryBeeSpecies} {secondaryBeeSpecies}", currentQuests[$"PureBredBee: {primaryBeeSpecies} {secondaryBeeSpecies}"][0] as Item);
            }
        }
    }
}
