using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Quest;
using BeeGame.Items.ColorChanger;

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

            //updates the game objects colour if the item is a Honey Comb
            UpdateItemColour(item);
        }

        public void UpdateItemData(Item _item)
        {
            item = _item;
        }

        #region Change Object Colour
        /// <summary>
        /// Updates the game objects colour if the <see cref="item"/> is a <see cref="Bee.ColourChanger.HoneyComb"/>
        /// </summary>
        /// <param name="_item">the item that the game object is being made from</param>
        public void UpdateItemColour(Item _item)
        {
            if (_item.honeyComb != null)
            {
                ChangeColour((Color)_item.honeyComb?.colour);
            }
        }

        /// <summary>
        /// If the item is a Honey Comb (<see cref="Bee.ColourChanger.HoneyComb"/>) then change the game object colour accordingly
        /// </summary>
        /// <param name="colour"></param>
        public void ChangeColour(Color colour)
        {
            Transform temp = transform.GetChild(0);
            GameObjectColourChanger[] changeItems = temp.GetComponentsInChildren<GameObjectColourChanger>();

            for (int i = 0; i < changeItems.Length; i++)
            {
                changeItems[i].UpdateCombColour(colour);
            }
        }
        #endregion
    }
}