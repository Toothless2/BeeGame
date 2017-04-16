using UnityEngine;
using BeeGame.Items;
using BeeGame.Core;
using System.Runtime.Serialization;

namespace BeeGame.Inventory.Player_Inventory
{
    public class PlayerInventory : Inventory
    {
        public GameObject playerInventory;

        void Start()
        {
            SetPlayerInventory();
            inventoryName = "PlayerInventory";
            Serialization.Serialization.DeSerializeInventory(this, inventoryName);
        }

        void Update()
        {
            UpdateBase();

            if (THInput.GetButtonDown("Player Inventory"))
                OpenPlayerInventory();

            RaycastHit[] hit = Physics.SphereCastAll(transform.position, 1f, transform.forward);

            for (int i = hit.Length - 1; i >= 0; i--)
            {
                if (hit[i].collider.GetComponent<ItemGameObject>())
                    PickupItem(hit[i].collider.GetComponent<ItemGameObject>());

                Serialization.Serialization.SerializeInventory(this, inventoryName);
            }

        }

        void OpenPlayerInventory()
        {
            playerInventory.SetActive(!playerInventory.activeInHierarchy);
            THInput.isAnotherInventoryOpen = !THInput.isAnotherInventoryOpen;

            Cursor.visible = !Cursor.visible;

            if (Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        void SetPlayerInventory()
        {
            if (InventorySet())
                SetInventorySize(20);
        }

        void PickupItem(ItemGameObject item)
        {
            if (AddItemToInventory(item.item))
                Destroy(item.gameObject);
        }
    }
}
