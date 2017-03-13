using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame.Items;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BeeGame.Inventory
{
    [System.Serializable]
    public class InventoryBase : MonoBehaviour
    {
        /// <summary>
        /// The Item that is currently being moved by the player
        /// </summary>
        public Item floatingItem;
        /// <summary>
        /// Where the item's information will show in the inventory
        /// </summary>
        public GameObject itemInfoPannel;
        /// <summary>
        /// The Unity GUI elements acting as the inventory slots
        /// </summary>
        public InventorySlot[] inventoryGUI;
        /// <summary>
        /// The item in each of the unventory slots
        /// </summary>
        public Item[] slotandItem;

        /// <summary>
        /// Sets the parent inventory variable for each of the inventory slots
        /// </summary>
        void Start()
        {
            for (int i = 0; i < inventoryGUI.Length; i++)
            {
                inventoryGUI[i].parentInventory = this;
            }
        }

        /// <summary>
        /// Updates the base inventory methods as unitys Update method is hidden due to this being a parent class
        /// </summary>
        protected void UpdateBase()
        {
            UpdateSlotAndItem();
            DropItem();
            UpdateItemDisplayData();
        }

        /// <summary>
        /// Updates the item info pannel to the right hand side of the inventory
        /// </summary>
        void UpdateItemDisplayData()
        {
            if(floatingItem.itemId != "" && floatingItem.itemId != null)
            {
                Text[] infoPannnelTexts = itemInfoPannel.GetComponentsInChildren<Text>();

                infoPannnelTexts[0].text = floatingItem.name;
                if (floatingItem.beeItem != null)
                {
                    if(floatingItem.CanSeeBeeData())
                    {
                        infoPannnelTexts[1].text = floatingItem.ReturnBeeDataAsText();
                    }
                    else
                    {
                        infoPannnelTexts[1].text = floatingItem.description;
                    }
                }
                else
                {
                    infoPannnelTexts[1].text = floatingItem.description;
                }
            }
        }

        /// <summary>
        /// Updates the slotandItem variable so their when the player is saved the inventory items are correct
        /// </summary>
        protected void UpdateSlotAndItem()
        {
            if(slotandItem == null)
            {
                slotandItem = new Item[inventoryGUI.Length];
            }

            if(slotandItem.Length  < inventoryGUI.Length)
            {
                slotandItem = new Item[inventoryGUI.Length];
            }

            for (int i = 0; i < inventoryGUI.Length; i++)
            {
                slotandItem[i] = inventoryGUI[i].item;
            }
        }

        /// <summary>
        /// Only used when the inventory needs to be updated eg when the player is remade or when a chest is remade
        /// </summary>
        public void UpdateSlots()
        {
            Start();
            for (int i = 0; i < inventoryGUI.Length; i++)
            {
                inventoryGUI[i].item = slotandItem[i];
                inventoryGUI[i].item.UpdateSpriteAndObject();
            }
        }

        /// <summary>
        /// Drops the item that is currently held in the floating item slot
        /// </summary>
        void DropItem()
        {
            if ((Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && (floatingItem.itemId != null && floatingItem.itemId != ""))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    GameObject temp = Instantiate(floatingItem.itemGameobject);
                    temp.GetComponent<Transform>().position = transform.position;
                    temp.GetComponent<ItemGameObjectInterface>().UpdateItemData(floatingItem);
                    floatingItem = new Item();
                }
            }
        }
    }
}