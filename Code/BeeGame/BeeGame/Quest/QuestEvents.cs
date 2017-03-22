using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Enums;

namespace BeeGame.Quest
{
    public static class QuestEvents
    {
        public delegate void CheckQuestComplete(string sender, EventArgs e);
        
        public static event CheckQuestComplete beeSpeciesMade;

        public static void BeeSpeciesMade(BeeSpecies beeSpecies, EventArgs e)
        {
            beeSpeciesMade?.Invoke(beeSpecies.ToString(), e);
        }
    }
}
