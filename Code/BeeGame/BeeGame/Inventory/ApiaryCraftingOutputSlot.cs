using UnityEngine.EventSystems;
using BeeGame.Items;
using BeeGame.Inventory.BlockInventory;
using BeeGame.Core;
using BeeGame.Quest;
using System;

namespace BeeGame.Inventory
{
    /// <summary>
    /// Overrides the 
    /// </summary>
    public class ApiaryCraftingOutputSlot : InventorySlot, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// Updates the base slot things
        /// </summary>
        protected new void Update()
        {
            CheckItem();
            UpdateIcon();
        }

        /// <summary>
        /// Gives extra functionality to the base slot
        /// </summary>
        /// <param name="eventData"></param>
        public override void OnPointerClick(PointerEventData eventData)
        {
            //* recored what item was in the slot before it is moved
            Item before = item;

            base.OnPointerClick(eventData);

            //* if the item is different now then the crafting result must have been removed so call the event
            if (before != item && before != null)
                ((CraftingTableInventory)myInventory).craftingResultRemoved.Invoke();

            if (before is Bee b)
                QuestEvents.CallBeeCraftedEvent(b.normalBee?.pSpecies ?? b.queenBee.queen.pSpecies);
            else
                QuestEvents.CallItemCraftedEvent(before.GetHashCode());
        }
    }
}