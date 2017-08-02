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
        private static Dictionary<string, Item> questDictionary = new Dictionary<string, Item>()
        {
            { $"Crafted: {Grass.ID}", new Dirt() }
        };

        public static void AddQuest(string quest, Item result)
        {
            if(!questDictionary.ContainsKey(quest))
            {
                questDictionary.Add(quest, result);
                return;
            }

            throw new QuestAlreadyExistsException($"Quest: {quest}, is already a quest");
        }

        public static Item ReturnCraftingTableQuest(int craftedItemID)
        {
            if (questDictionary.ContainsKey($"Crafted: {craftedItemID}"))
                return questDictionary[$"Crafted: {craftedItemID}"];

            return null;
        }

        public static Item ReturnBeeCraftingQuest(BeeSpecies primaryBeeSpecies)
        {
            if (questDictionary.ContainsKey($"BeeCrafted: {primaryBeeSpecies}"))
                return questDictionary[$"BeeCrafted: {primaryBeeSpecies}"];

            return null;
        }

        public static Item ReturnPureBreadBeeCraftingQuest(BeeSpecies primaryBeeSpecies, BeeSpecies secondaryBeeSpecies)
        {
            if (primaryBeeSpecies != secondaryBeeSpecies)
                return null;

            if (questDictionary.ContainsKey($"PureBredBee: {primaryBeeSpecies} {secondaryBeeSpecies}"))
                return questDictionary[$"PureBredBee: {primaryBeeSpecies} {secondaryBeeSpecies}"];

            return null;
        }
    }
}
