using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BeeGame.Items;
using BeeGame.Core;

namespace BeeGame.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Data
        /// <summary>
        /// The slot in the inventory this is
        /// </summary>
        internal int slotIndex;
        /// <summary>
        /// The item this slot has in it
        /// </summary>
        public Item item;
        /// <summary>
        /// The <see cref="Inventory"/> this slot is in
        /// </summary>
        public Inventory myInventory;
        /// <summary>
        /// If the slot currently has the item text object made this will be not null otherwise it is null
        /// </summary>
        public GameObject itemText;
        /// <summary>
        /// Is this slot currently the selected slot in the hotbar?
        /// </summary>
        public bool selectedSlot = false;
        /// <summary>
        /// Can items be inserted into this slot by the player
        /// </summary>
        public bool itemsCanBeInserted = true;
        #endregion

        /// <summary>
        /// Updates the slot
        /// </summary>
        private void Update()
        {
            CheckItem();
            UpdateIcon();
        }


        /// <summary>
        /// Applies the correct icon to the slot depending on what is in the slot
        /// </summary>
        void UpdateIcon()
        {
            if(item == null)
            {
                GetComponent<Image>().sprite = null;
            }
            else
            {
                if(!item.Equals(new Item()))
                    GetComponent<Image>().sprite = item.GetItemSprite();
            }

            //* if the slot is selected in the hotbar give the player some indication by colouring it grey
            if (selectedSlot)
            {
                GetComponent<Image>().color = Color.gray;
            }
            else
            {
                GetComponent<Image>().color = Color.white;
            }
        }

        #region Interact With Slot
        /// <summary>
        /// Allows the player to interact with the item slot
        /// </summary>
        /// <param name="eventData">Right or Left click</param>
        /// <remarks>
        /// Called by the unity event handler when the slot is clicked on
        /// </remarks>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (myInventory.floatingItem != null)
            {
                //* Left click moves whole stacks of items
                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    //* If the item in the slot is empty put the floating item into it then clear it and the slot can have items inserted
                    if (item == null && itemsCanBeInserted)
                    {
                        item = myInventory.floatingItem;
                        myInventory.floatingItem = null;
                        myInventory.AddItemToSlots(slotIndex, item);
                        return;
                    }
                    //* if the items are the same
                    if(myInventory.floatingItem == item && itemsCanBeInserted)
                    {
                        //* if the item in the inventoys stack count + the floating items stack count is less than the max stack count 
                        if (myInventory.floatingItem.itemStackCount + item.itemStackCount <= item.maxStackCount)
                        {
                            AddToSlot(myInventory.floatingItem.itemStackCount);
                            return;
                        }
                        //* if the item stack added is larger than the max count add as many as you can and move on
                        else
                        {
                            AddToSlot(item.maxStackCount - item.itemStackCount);
                            return;
                        }
                    }
                    //* if the tiems are the same but items cannot be inserted into the slot add as many items as you 
                    //* can from the slot to the floating item
                    else if(myInventory.floatingItem == item && !itemsCanBeInserted)
                    {
                        AddToFloatingItem();
                        return;
                    }
                    //* If the items were not == swap them
                    else
                    {
                        //* only if items can be inserted into the slot
                        if(itemsCanBeInserted)
                            SwapItems();
                        return;
                    }
                }
                else if(eventData.button == PointerEventData.InputButton.Right)
                {
                    //* if the item in slot is null add 1 from the floating item to it
                    if(item == null && itemsCanBeInserted)
                    {
                        AddToSlot(1);
                        return;
                    }
                    //* if the items are the same add 1 from the floating item to this item
                    else if(item == myInventory.floatingItem && itemsCanBeInserted)
                    {
                        AddToSlot(1);
                        return;
                    }
                }
            }
            //* if the floating item is null
            else
            {
                //* add 1/2 of the stack into the floating item if right click was pressed
                if(eventData.button == PointerEventData.InputButton.Right)
                {
                    SplitStack();
                    return;
                }

                //* otherwie add the items into the floating item slot
                SwapItems();
                //* ^ does not need to check that the slot cannot be inserted into as null be being inserted because the floating item is null
                return;
            }

        }

        /// <summary>
        /// Add items from the slot to the <see cref="Inventory.floatingItem"/>
        /// </summary>
        void AddToFloatingItem()
        {
            //* if the whole stack can be added do it and move on
            if(myInventory.floatingItem.itemStackCount + item.itemStackCount <= item.maxStackCount)
            {
                myInventory.floatingItem.itemStackCount += item.itemStackCount;

                item = null;

                myInventory.AddItemToSlots(slotIndex, item);

                return;
            }

            //* if the whole stack cannot be added calculate how many need to be removed from the slots item stack
            item.itemStackCount -= (item.maxStackCount - myInventory.floatingItem.itemStackCount);
            //* set the floating item to the max stack count
            myInventory.floatingItem.itemStackCount = item.maxStackCount;

            myInventory.AddItemToSlots(slotIndex, item);
        }

        /// <summary>
        /// Adds a number to items into the slot
        /// </summary>
        /// <param name="numerToAdd">Numebr or items to add to the slot</param>
        void AddToSlot(int numerToAdd)
        {
            //* if the item in the slot is null create it
            if (item == null)
            {
                item = myInventory.floatingItem.CloneObject();
                item.itemStackCount = 0;
            }

            //* add to number to add to the stack count
            item.itemStackCount += numerToAdd;
            
            //* if the stack count is now larger than it should be dont let it be
            if (item.itemStackCount > item.maxStackCount)
            {
                item.itemStackCount = item.maxStackCount;
            }

            //* remove the numebr if items form the floating item then check the floating item is not null
            myInventory.floatingItem.itemStackCount -= numerToAdd;
            CheckFloatingItem();
            //* save the inventory changes
            myInventory.AddItemToSlots(slotIndex, item);
        }

        /// <summary>
        /// Halfs a <see cref="Item.itemStackCount"/> between the slot and the <see cref="Inventory.floatingItem"/>
        /// </summary>
        /// <remarks>
        /// If the stack count is the slot is not an even number more items go to the floating item than go to the slot. This is so that right clicking on a slot when their is only 1 item in it actually make the item in that slot go into the floating item
        /// </remarks>
        void SplitStack()
        {
            myInventory.floatingItem = item.CloneObject();
            int give = (item.itemStackCount + 1) / 2;
            myInventory.floatingItem.itemStackCount = give;
            item.itemStackCount -= give;

            if (item.itemStackCount <= 0)
                item = null;

            myInventory.AddItemToSlots(slotIndex, item);
            Destroy(itemText);
        }

        /// <summary>
        /// Swaps the <see cref="Item"/> in the <see cref="Inventory.floatingItem"/> with the slots <see cref="item"/>
        /// </summary>
        void SwapItems()
        {
            //* temp copy of the item
            Item temp = myInventory.floatingItem;
            //* sets the floating item
            myInventory.floatingItem = item;
            //* sets the item that was in the floating item to the item in the the slot
            item = temp;
            //* Saves the changes to the inventory
            myInventory.AddItemToSlots(slotIndex, item);
            //* destroys the text as it is not needed anymore
            Destroy(itemText);
        }

        /// <summary>
        /// Checks if the <see cref="Inventory.floatingItem"/> should be null
        /// </summary>
        void CheckFloatingItem()
        {
            if(myInventory.floatingItem.itemStackCount <= 0)
            {
                myInventory.floatingItem = null;
            }
        }
        #endregion

        /// <summary>
        /// checks that the item is valid
        /// </summary>
        private void CheckItem()
        {
            if (item != null && myInventory != null)
            {
                if (item.itemStackCount == 0 || item == new Item())
                {
                    myInventory.items.itemsInInventory[slotIndex] = null;
                    Destroy(itemText);
                }
            }
        }

        #region Display Item On Hover
        /// <summary>
        /// Makes the text object when the cursor is over the slot
        /// </summary>
        /// <param name="eventData">Not used but required for the interface</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            //* if the item is null or the floating item has something in it dont display the item text as it is not necissary
            if (item != null && myInventory.floatingItem == null)
            {
                itemText = Instantiate(PrefabDictionary.GetPrefab("ItemDetails"));
                //* sets the text to the correct postion
                itemText.transform.GetChild(0).position = Input.mousePosition;
                //* puts the correct text in the box
                itemText.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = $"{item.GetItemName()}\nStack: {item.itemStackCount}";
            }
        }

        /// <summary>
        /// Destroys the text object when the cursor is not over the slot anymore
        /// </summary>
        /// <param name="eventData">Not used but required for the interface</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            Destroy(itemText);
        }

        /// <summary>
        /// Destroys the item text when the inventory is closed
        /// </summary>
        void OnDisable()
        {
            Destroy(itemText);
        }
        #endregion
    }
}
