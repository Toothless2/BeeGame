using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeGame.Core.Enums;

namespace BeeGame.Quest
{
    public class QuestEvents
    {
        public delegate void QuestEventHandler (int quest);
        private static event QuestEventHandler itemCraftedEvent = Quests.ReturnCraftingTableQuest;
        private static event QuestEventHandler itemPickupEvent = Quests.ReturnPickupQuest;

        private delegate void BeeQuestEventHandler(BeeSpecies species);
        private static event BeeQuestEventHandler beeCraftedEvent = Quests.ReturnBeeCraftingQuest;

        private delegate void PureBeeQuestEventHandler(BeeSpecies species1, BeeSpecies species2);
        private static event PureBeeQuestEventHandler pureBeeCraftedEvent = Quests.ReturnPureBreadBeeCraftingQuest;

        public static void CallItemPickupEvent(int id)
        {
            itemPickupEvent(id);
        }

        public static void CallItemCraftedEvent(int id)
        {
            itemCraftedEvent(id);
        }

        public static void CallBeeCraftedEvent(BeeSpecies species)
        {
            beeCraftedEvent(species);
        }

        public static void CallPureBeeCraftedEvent(BeeSpecies species1, BeeSpecies species2)
        {
            pureBeeCraftedEvent(species1, species2);
        }
    }
}
