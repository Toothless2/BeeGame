using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Enums;
using BeeGame.Core;

namespace BeeGame.Rooms
{
    [Serializable]
    public class Room : World
    {
        private THVector3 position;
        [NonSerialized]
        private GameObject roomModel;
        private RoomType roomType;

        public THVector3 Position
        {
            get
            {
                if (position == null || position == new THVector3())
                    return new THVector3(0, 0, 0);

                return position;
            }
            set
            {
                if (value == null || value == new THVector3())
                {
                    position = value;
                    Debug.LogWarning("Position is Null/Empty");
                    return;
                }

                position = value;
            }
        }

        public GameObject RoomModel
        {
            get
            {
                if (roomModel == null)
                {
                    roomModel = PrefabDictionary.GetGameObjectItemFromDictionary(RoomType.ToString());
                }

                return roomModel;
            }
        }

        public RoomType RoomType
        {
            get
            {
                return roomType;
            }

            set
            {
                roomType = value;
            }
        }
    }
}
