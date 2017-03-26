using UnityEngine;
using BeeGame.Core;
using BeeGame.Inventory;
using BeeGame.Items;
using BeeGame.Blocks;
using BeeGame.Serialization;

namespace BeeGame.Player
{
    public class PlayerInteract : MonoBehaviour
    {
        /// <summary>
        /// The players inventory
        /// </summary>
        public PlayerInventory playerInventory;
        /// <summary>
        /// the selector GameObject
        /// </summary>
        private GameObject selector;
        /// <summary>
        /// What the player is looking at
        /// </summary>
        private RaycastHit hit;
        /// <summary>
        /// The position of the selector alligned to the grid
        /// </summary>
        private Vector3 hitAlligned;

        /// <summary>
        /// Makes a selector and sets it's transform
        /// </summary>
        void Awake()
        {
            selector = PrefabDictionary.GetGameObjectItemFromDictionary("Selector");
            selector = Instantiate(selector);
            selector.transform.parent = transform;
        }

        /// <summary>
        /// Every update check for if the user wants to break, place, or interact with a block
        /// </summary>
        void Update()
        {
            LookPoz();

            UpdateSelector();

            if(Time.timeScale > 0)
            {
                if (THInput.GetButtonDown("Break Block"))
                {
                    BreakBlock();
                }

                if (THInput.GetButtonDown("Place/Interact"))
                {
                    if (!Interact())
                    {
                        Placeblock();
                    }
                }
            }
        }

        /// <summary>
        /// Updates where the block selector is based on the current look positon
        /// </summary>
        void UpdateSelector()
        {
            selector.transform.position = hitAlligned;
            selector.transform.rotation = Quaternion.identity;
        }

        /// <summary>
        /// Can the player already interact with what is selected
        /// </summary>
        /// <returns>true if selected object can be interacted with</returns>
        bool Interact()
        {
            if(hit.transform != null)
            {
                if(hit.collider.tag == "Block")
                {
                    // <TODO> Add other interaction scripts here when neccicary
                    if(hit.collider.GetComponent<ChestInventory>())
                    {
                        hit.collider.GetComponent<ChestInventory>().OpenChest(playerInventory);
                        return true;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Places a block at the location currently being looked at
        /// </summary>
        void Placeblock()
        {
            if (playerInventory.ItemPlaceable())
            {
                if (!CheckForBlock((hitAlligned + hit.normal)))
                {
                    //Spawns the GameObject
                    GameObject temp = Instantiate(playerInventory.BlockToPlace(), hitAlligned + hit.normal, Quaternion.identity);
                    //sets the tag
                    temp.tag = "Block";
                    
                    temp.GetComponent<BlockGameObjectInterface>().UpdateBlockData(playerInventory.ItemData(), temp.transform.position);

                    //subtracts from the inventory stack count
                    playerInventory.RemoveItemFromStack();

                    temp.AddToSaveBlocks();
                }
            }
        }

        /// <summary>
        /// Checks if their is a block already in the position where the new one is supposed to be spawned
        /// </summary>
        /// <param name="checkPoz">the Position that sould be checked</param>
        /// <returns>Returns true if their is a block in the positon</returns>
        bool CheckForBlock(Vector3 checkPoz)
        {
            return Physics.CheckSphere(checkPoz, 0.1f);
        }

        /// <summary>
        /// Breaks the block that the player is looking at
        /// </summary>
        void BreakBlock()
        {
            if(hit.transform != null)
            {
                if (hit.transform.tag == "Block")
                {
                    hit.transform.GetComponent<BlockGameObjectInterface>().DestroyBlock();
                }
            }
        }

        /// <summary>
        /// Gets the postion of where the player is looking and sets it to the grid
        /// </summary>
        void LookPoz()
        {
            Physics.Raycast(transform.position, transform.forward, out hit);

            if(hit.transform != null)
            {
                if (hit.collider.tag != "Block")
                {
                    hitAlligned = new Vector3((int)hit.point.x, (int)hit.point.y, (int)hit.point.z);
                }
                else
                {
                    hitAlligned = hit.transform.position;
                }
            }
        }
    }
}