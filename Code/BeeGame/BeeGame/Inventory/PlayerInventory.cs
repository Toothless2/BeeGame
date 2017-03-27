using UnityEngine;
using System.Linq;
using BeeGame.Core;
using BeeGame.Items;
using BeeGame.Blocks;

namespace BeeGame.Inventory
{
    [System.Serializable]
    public class PlayerInventory : InventoryBase
    {
        public GameObject inventory;
        
        /// <summary>
        /// The held object inventory(hotbar) of the player
        /// </summary>
        [System.NonSerialized]
        public GameObject heldObjectInventory;

        /// <summary>
        /// the inventory slot index of the slot the player currently has selected in the hotbar
        /// </summary>
        [System.NonSerialized]
        private int currentHeldItemIndex;

        /// <summary>
        /// The item currently held by the player
        /// </summary>
        [System.NonSerialized]
        private GameObject heldObject;

        void Start()
        {
            heldObjectInventory = GameObject.Find("Inventory");
            currentHeldItemIndex = inventoryGUI.Length - 5;
            inventoryGUI[currentHeldItemIndex].UpdateSelectedSlot(true);
        }

        /// <summary>
        /// Will update the players inventory every frame
        /// </summary>
        void Update()
        {
            UpdateBase();
            UpdateHotbarItem();
            PickupItem();
            ShowHideInventory();
            
            UpdateHeldGameObject();
        }

        /// <summary>
        /// Checks wether the player as selected a dirrerent hotbar item and updates accordingly
        /// </summary>
        void UpdateHotbarItem()
        {
            if (Time.timeScale > 0)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0)
                {
                    HeldPlayerItem(1);
                }
                else if (Input.GetAxis("Mouse ScrollWheel") < 0)
                {
                    HeldPlayerItem(-1);
                }
            }
        }

        /// <summary>
        /// Shows and hides the player inventory
        /// </summary>
        void ShowHideInventory()
        {
            if (heldObjectInventory.activeInHierarchy)
            {
                if (THInput.GetButtonDown("Player Inventory"))
                {
                    if (THInput.isAnotherInventoryOpen)
                    {
                        if (inventory.activeInHierarchy)
                        {
                            inventory.SetActive(false);
                            Cursor.visible = false;
                            Cursor.lockState = CursorLockMode.Locked;
                            Time.timeScale = 1;
                            THInput.isAnotherInventoryOpen = false;
                        }
                    }
                    else
                    {
                        inventory.SetActive(true);
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.None;
                        Time.timeScale = 0;
                        THInput.isAnotherInventoryOpen = true;
                    }
                }
            }
        }

        /// <summary>
        /// Picksup an item on the ground if their is enough space in the inventory
        /// </summary>
        void PickupItem()
        {
            RaycastHit[] cols = Physics.SphereCastAll(transform.position, 4f, Vector3.up);

            for (int i = 0; i < cols.Length; i++)
            {
                if (cols[i].collider.tag == "Item" && cols[i].collider.transform.parent != transform.parent)
                {
                    Item item = cols[i].collider.GetComponent<ItemGameObjectInterface>().item;

                    for (int h = 0; h < inventoryGUI.Length; h++)
                    {
                        if (inventoryGUI[h].item.itemId == "" || inventoryGUI[h].item.itemId == null)
                        {
                            inventoryGUI[h].item = item;
                            Destroy(cols[i].collider.gameObject);
                            break;
                        }
                        if (inventoryGUI[h].item == item)
                        {
                            if (inventoryGUI[h].item.isStackable)
                            {
                                if (inventoryGUI[h].item.stackCount + item.stackCount < inventoryGUI[h].item.maxStackCount + 1)
                                {
                                    inventoryGUI[h].item.stackCount++;
                                    Destroy(cols[i].collider.gameObject);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// the item data that is currently being held
        /// </summary>
        /// <returns>Return the itm data that is currently being held</returns>
        public Item ItemData()
        {
            return inventoryGUI[currentHeldItemIndex].item;
        }
        
        #region Helper Functions for Placing Blocks
        /// <summary>
        /// Is the item currently selected placeable
        /// </summary>
        /// <returns>true is the item currently held is placeable</returns>
        public bool ItemPlaceable()
        {
            return inventoryGUI[currentHeldItemIndex].item.isPlaceable;
        }
        /// <summary>
        /// Returns the GameObject to be instantiated
        /// </summary>
        /// <returns>Gameobject of the currently selected item</returns>
        public GameObject BlockToPlace()
        {
            return PrefabDictionary.GetGameObjectItemFromDictionary(inventoryGUI[currentHeldItemIndex].item.objectName);
        }
        /// <summary>
        /// Removes an item from the stack count
        /// </summary>
        public void RemoveItemFromStack()
        {
            inventoryGUI[currentHeldItemIndex].item.stackCount -= 1;
        }
        #endregion

        #region Item Slot Currently Selected
        /// <summary>
        /// Changes the item currently held by the player
        /// </summary>
        /// <param name="direction">The direction that the player scrolled in (+ = right, - = left)</param>
        public void HeldPlayerItem(int direction)
        {
            if (currentHeldItemIndex + direction < inventoryGUI.Length && currentHeldItemIndex + direction > inventoryGUI.Length - 6)
            {
                ChangeSelectedItem(currentHeldItemIndex + direction);
            }
            else if (currentHeldItemIndex + direction >= inventoryGUI.Length)
            {
                ChangeSelectedItem(inventoryGUI.Length - 5);
            }
            else if (currentHeldItemIndex + direction < inventoryGUI.Length)
            {
                ChangeSelectedItem(inventoryGUI.Length - 1);
            }

            UpdateHeldGameObject();
        }

        /// <summary>
        /// Changes the invenotry slot graphic of the selected slot
        /// </summary>
        /// <param name="direction">The index of the slot to be selected</param>
        void ChangeSelectedItem(int direction)
        {
            inventoryGUI[currentHeldItemIndex].UpdateGraphic();
            inventoryGUI[currentHeldItemIndex].UpdateSelectedSlot(false);
            currentHeldItemIndex = direction;
            inventoryGUI[currentHeldItemIndex].UpdateGraphic();
            inventoryGUI[currentHeldItemIndex].UpdateSelectedSlot(true);
        }
        #endregion

        #region Item Currently In Players Hand
        int previousHeldIndex;
        /// <summary>
        /// Updates the object that is currently spawned to represent the object the player is holding
        /// </summary>
        void UpdateHeldGameObject()
        {
            if(heldObject == null && (previousHeldIndex != currentHeldItemIndex))
            {
                if(inventoryGUI[currentHeldItemIndex].item.itemId != null)
                {
                    previousHeldIndex = currentHeldItemIndex;
                    SpawnObject();
                }
            }
            else if(previousHeldIndex != currentHeldItemIndex)
            {
                Destroy(heldObject);
                previousHeldIndex = currentHeldItemIndex;

                if (inventoryGUI[currentHeldItemIndex].item.itemId != null)
                {
                    SpawnObject();
                }
            }
            else if(heldObject != null && inventoryGUI[currentHeldItemIndex].item.itemId == null)
            {
                Destroy(heldObject);
                previousHeldIndex = currentHeldItemIndex;
            }
        }

        /// <summary>
        /// Spawns the object that is currently being held by the player
        /// </summary>
        void SpawnObject()
        {
            if(inventoryGUI[currentHeldItemIndex].item.itemGameobject)
            {
                heldObject = Instantiate(inventoryGUI[currentHeldItemIndex].item.itemGameobject, transform.position, Quaternion.identity);
                heldObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                heldObject.tag = "Player";
                heldObject.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

                for (int i = heldObject.GetComponents<Component>().Length - 1; i >= 0; i--)
                {
                    switch (heldObject.GetComponents<Component>()[i])
                    {
                        case ItemGameObjectInterface t:
                            return;
                        case BlockGameObjectInterface t:
                            return;
                        default:
                            Destroy(heldObject.GetComponents<Component>()[i]);
                            break;
                    }
                }
            }
        }
        #endregion
    }
}