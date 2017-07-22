using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Items;
using BeeGame.Blocks;

namespace BeeGame.Core
{
    public static class Events
    {
        public delegate void ItemCraftedEvent(Item item);
        public static ItemCraftedEvent shapedRecipieCrafted;
        public static ItemCraftedEvent shaplessRecipieCrafted;
        public static ItemCraftedEvent beeCraftedEvent;

        public static void CallShapedRecipieCraftedEvent(Item item) => shapedRecipieCrafted?.Invoke(item);
        public static void CallShaplessRecipirCraftedEvent(Item item) => shaplessRecipieCrafted?.Invoke(item);
        public static void CallBeeCraftedEvent(Item item) => beeCraftedEvent?.Invoke(item);
    }
}
