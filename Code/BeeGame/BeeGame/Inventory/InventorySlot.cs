using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using BeeGame.Items;

namespace BeeGame.Inventory
{
    public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        internal int slotIndex;
        public Item item;
        public Inventory myInventory;

        private void Update()
        {
            UpdateIcon();
        }

        void UpdateIcon()
        {
            if(item == null)
            {
                GetComponent<Image>().sprite = null;
            }
            else
            {
                GetComponent<Image>().sprite = item.GetItemSprite();
            }
        }

        void AddRemoveFromSlot()
        {
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (myInventory.floatingItem != null)
            {
                if (item == null)
                {
                    item = myInventory.floatingItem;
                    myInventory.floatingItem = null;
                    myInventory.AddItemToSlots(slotIndex, item);
                    return;
                }

                if (eventData.button == PointerEventData.InputButton.Left)
                {
                    if (myInventory.floatingItem.itemStackCount + item.itemStackCount <= item.maxStackCount)
                    {
                        item.itemStackCount += myInventory.floatingItem.itemStackCount;
                        myInventory.floatingItem = null;
                        myInventory.AddItemToSlots(slotIndex, item);
                        return;
                    }
                }
                else
                {
                    if (myInventory.floatingItem.itemStackCount + item.itemStackCount <= item.maxStackCount)
                    {
                        item.itemStackCount++;
                        myInventory.floatingItem.itemStackCount--;
                        CheckFloatingItem();
                        myInventory.AddItemToSlots(slotIndex, item);
                        return;
                    }
                }
            }
            else
            {
                myInventory.floatingItem = item;
                item = null;
                myInventory.AddItemToSlots(slotIndex, item);
                return;
            }

        }

        void CheckFloatingItem()
        {
            if(myInventory.floatingItem.itemStackCount <= 0)
            {
                myInventory.floatingItem = null;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            print("pointer entered me");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            print("pointer left me :(");
        }
    }
}
