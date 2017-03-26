using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Quest;

namespace BeeGame.Items
{
    /// <summary>
    /// Interfaces between the <see cref="Item"/> and the Unity GameObject as GameObject is not serializable with c# <see cref="System.Runtime.Serialization.Formatters.Binary.BinaryFormatter"/> and neither is MonoBehaviour
    /// </summary>
    public class ItemGameObjectInterface : MonoBehaviour
    {
        /// <summary>
        /// This interfaces Item
        /// </summary>
        public Item item;

        /// <summary>
        /// When this item is made set its position
        /// </summary>
        void Awake()
        {
            if(item.pos.x != 0)
            {
                transform.position = item.pos;
            }
        }

        /// <summary>
        /// if the item does not have a parent sets it's location to where it is
        /// </summary>
        void Update()
        {
            if(transform.parent == null)
            {
                item.pos = transform.position;
            }
        }

        public void UpdateItemData(Item _item)
        {
            item = _item;
        }
    }
}