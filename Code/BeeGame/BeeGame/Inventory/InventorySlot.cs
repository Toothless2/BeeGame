using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BeeGame.Items;
using BeeGame.Enums;

namespace BeeGame.Inventory
{
    [System.Serializable]
    /// <summary>
    /// Holds the item data when it is displayed in an invertory to the user \n
    /// </summary>
    /// <remarks>
    /// This is only updated when it is being viewed. \n
    /// This is now serialized as the item data for the whole inventory is stored an one in the <see cref="InventoryBase.slotandItem"/> this makes serialization simpler as serializing each slot would be far to much work. \n
    /// The slot gets its data from the <see cref="InventoryBase.slotandItem"/> during deserialization. \n
    /// During runtime the slot's <see cref="item"/> is stored in the <see cref="InventoryBase.slotandItem"/> via an undate method (<see cref="InventoryBase.UpdateSlotAndItem()"/>) \n
    /// </remarks>
    public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        #region Data
        /// <summary>
        /// Can an Item be place into the inventory slot by the player? Default = true
        /// </summary>
        public bool canPlaceObjectInSlot = true;
        /// <summary>
        /// The inventory that is slot is attached to <see cref="InventoryBase"/>
        /// </summary>
        public InventoryBase parentInventory;
        /// <summary>
        /// The Unity GUI Text element that will display the stack count number
        /// </summary>
        public Text number;
        /// <summary>
        /// The item info pannel where the item data should be displayed
        /// </summary>
        public GameObject itemInfoPannel;
        /// <summary>
        /// The item in the slot
        /// </summary>
        public Item item;
        /// <summary>
        /// Is this slot currently selected in the hotbar (Default = False)
        /// </summary>
        private bool isSelected;
        #endregion

        #region Unity Methods
        /// <summary>
        /// When the slot is first seen the text that displayes the item stack count is set to green
        /// </summary>
        void Start()
        {
            number.color = Color.green;
        }

        /// <summary>
        /// Updates the slot every frame, when it is being viewed
        /// </summary>
        void Update()
        {
            if (item.stackCount < 1)
            {
                item = new Item();
            }
            UpdateGraphic();
            UpdateStackNumber();
        }
        #endregion

        #region Update Slot
        /// <summary>
        /// Updates the stack number on the GUI
        /// </summary>
        void UpdateStackNumber()
        {
            if (item.itemId != "")
            {
                if (item.itemId != null)
                {
                    number.text = item.stackCount.ToString();
                }
                else
                {
                    number.text = " ";
                }
            }
            else
            {
                number.text = " ";
            }
        }


        /// <summary>
        /// Updates the graphic on the slot GUI
        /// </summary>
        public void UpdateGraphic()
        {
            if (item.itemId == null)
            {
                GetComponent<Image>().sprite = null;
                GetComponent<Image>().color = Color.white;
            }
            else
            {
                item.UpdateSpriteAndObject();
                GetComponent<Image>().sprite = item.itemSpriteObject;
            }

            UpdateSelectedSlot(isSelected);
        }

        /// <summary>
        /// Only used for the hotbar item slots
        /// </summary>
        /// <param name="selected">Is his the slot the player currently has selected in the hotbar?</param>
        public void UpdateSelectedSlot(bool selected)
        {
            isSelected = selected;

            if (selected)
            {
                GetComponent<Image>().color = Color.gray;
            }
            else
            {
                if(item.honeyComb == null)
                {
                    GetComponent<Image>().color = Color.white;
                    return;
                }
                
                GetComponent<Image>().color = (Color)item.honeyComb?.colour;
            }
        }
        #endregion

        #region ItemDataDisplay
        /// <summary>
        /// Updates the side pannel to show the items data
        /// </summary>
        /// <param name="eventData">unused only their to satisfy interface</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (parentInventory.floatingItem.itemId == "" || parentInventory.floatingItem.itemId == null)
            {
                if (item.itemId != "")
                {
                    Text[] infoPannnelTexts = itemInfoPannel.GetComponentsInChildren<Text>();

                    infoPannnelTexts[0].text = item.name;
                    if (item.beeItem != null)
                    {
                        if (item.CanSeeBeeData())
                        {
                            infoPannnelTexts[1].text = item.ReturnBeeDataAsText();
                        }
                        else
                        {
                            infoPannnelTexts[1].text = item.description;
                        }
                    }
                    else
                    {
                        infoPannnelTexts[1].text = item.description;
                    }
                }
                else
                {
                    Text[] infoPannnelTexts = itemInfoPannel.GetComponentsInChildren<Text>();

                    infoPannnelTexts[0].text = "";
                    infoPannnelTexts[1].text = "";
                }
            }
        }

        /// <summary>
        /// Removes the items data from the item data desplay pannel
        /// </summary>
        /// <param name="eventData">unused only their to satisfy interface</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (parentInventory.floatingItem.itemId == "")
            {
                Text[] infoPannnelTexts = itemInfoPannel.GetComponentsInChildren<Text>();

                infoPannnelTexts[0].text = "";
                infoPannnelTexts[1].text = "";
            }
        }
        #endregion

        #region User Interaction
        //\todo REFACTOR ME YOU IDIOT!!!!!!
        // Could split each of the things into seperate methods. This would reduce size of statement and reduce code repetition
        /// <summary>
        /// When the slot is clicked this is called. Controlls how an item is moved around the inventory
        /// </summary>
        /// <param name="eventData">Right, Left, or Middle click</param>
        /// <remarks>
        /// This needs refactoring to cretin.
        /// I know I just cant be bothered to do it.
        /// Just do it.
        /// But I dont wanna.
        /// </remarks>
        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                //if their is something in the item slot
                if (item.itemId != "" && item.itemId != null)
                {
                    //if their is somethign in the floating item
                    if (parentInventory.floatingItem.itemId != "" && parentInventory.floatingItem.itemId != null)
                    {
                        //if something can be place into the slot
                        if (canPlaceObjectInSlot)
                        {
                            //if items in both the slot and floating item are the same
                            if (item == parentInventory.floatingItem)
                            {
                                if (item.stackCount + parentInventory.floatingItem.stackCount < item.maxStackCount + 1)
                                {
                                    item.stackCount += parentInventory.floatingItem.stackCount;
                                    parentInventory.floatingItem = new Item();
                                }
                                else
                                {
                                    //if neither of the stacks are full but both of them added are more than the max stack count
                                    if (parentInventory.floatingItem.stackCount != item.maxStackCount && item.stackCount != item.maxStackCount)
                                    {
                                        int remove = (item.stackCount + parentInventory.floatingItem.stackCount) - item.maxStackCount;
                                        item.stackCount = item.maxStackCount;
                                        parentInventory.floatingItem.stackCount -= remove;

                                        if (parentInventory.floatingItem.stackCount < 1)
                                        {
                                            parentInventory.floatingItem = new Item();
                                        }
                                    }
                                    //if both stacks are full swap the stacks
                                    else
                                    {
                                        Item temp = item;
                                        item = parentInventory.floatingItem;
                                        parentInventory.floatingItem = temp;
                                    }
                                }
                            }
                            //if the items are not the same swap them
                            else
                            {
                                Item temp = item;
                                item = parentInventory.floatingItem;
                                parentInventory.floatingItem = temp;
                            }
                        }
                    }
                    //if their isn't something in the floting item copy this slots item in to it and empty the slot
                    else
                    {
                        parentInventory.floatingItem = item;
                        item = new Item();
                    }
                }
                else
                {
                    //if something can be place into the slot
                    if(canPlaceObjectInSlot)
                    {
                        //if their isnt something in the slot but ther is something in the floting item put floting item into the slot and empty the floating item 
                        if (parentInventory.floatingItem.itemId != "" && parentInventory.floatingItem.itemId != null)
                        {
                            item = parentInventory.floatingItem;
                            parentInventory.floatingItem = new Item();
                        }
                    }
                }
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                //if their is something in the item slot
                if (item.itemId != "" && item.itemId != null)
                {
                    //if their is somethign in the floating item
                    if (parentInventory.floatingItem.itemId != "" && parentInventory.floatingItem.itemId != null)
                    {
                        //If soemthign can be place into the slot
                        if (canPlaceObjectInSlot)
                        {
                            if (parentInventory.floatingItem == item)
                            {
                                if (item.stackCount + 1 < item.maxStackCount + 1)
                                {
                                    item.stackCount++;
                                    parentInventory.floatingItem.stackCount--;

                                    if (parentInventory.floatingItem.stackCount < 1)
                                    {
                                        parentInventory.floatingItem = new Item();
                                    }
                                }

                            }
                        }
                    }
                    //if their is nothign in the floting item add half of the stack to it
                    else
                    {
                        int give = (item.stackCount + 1) / 2;
                        parentInventory.floatingItem = item;
                        parentInventory.floatingItem.stackCount = give;
                        item.stackCount -= give;

                        if (item.stackCount < 1)
                        {
                            item = new Item();
                        }
                    }
                }
                else if (parentInventory.floatingItem.itemId != "" && parentInventory.floatingItem.itemId != null)
                {
                    //if something can be place into the item slot
                    if (canPlaceObjectInSlot)
                    {
                        item = parentInventory.floatingItem;
                        item.stackCount = 1;
                        parentInventory.floatingItem.stackCount -= 1;

                        if (parentInventory.floatingItem.stackCount < 1)
                        {
                            parentInventory.floatingItem = new Item();
                        }
                    }
                }

                UpdateGraphic();
                UpdateStackNumber();
            }
        }
        #endregion
    }
}