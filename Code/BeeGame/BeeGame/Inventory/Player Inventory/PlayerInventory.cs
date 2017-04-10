using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Items;

namespace BeeGame.Inventory.Player_Inventory
{
    public class PlayerInventory : Inventory
    {
        void Start()
        {
            SetPlayerInventory();
        }

        void Update()
        {

            RaycastHit[] hit = Physics.SphereCastAll(transform.position, 10f, transform.forward);

            for (int i = hit.Length - 1; i >= 0; i--)
            {
                if (hit[i].collider.GetComponent<ItemGameObject>())
                    PickupItem(hit[i].collider.GetComponent<ItemGameObject>());
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
