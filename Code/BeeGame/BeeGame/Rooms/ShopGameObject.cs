using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeeGame.Rooms
{
    public class ShopGameObject : MonoBehaviour
    {
        public Room room;

        void Awake()
        {
            MakeNewRoom(new ShopRoom());
        }

        void Start()
        {
            Instantiate(room.RoomModel, transform, false);
            transform.position = room.Position;
            transform.position = new THVector3(0, -11, 0);
        }

        public void UpdateShop()
        {

        }

        public void MakeNewRoom(Room roomToMake)
        {
            switch (roomToMake)
            {
                case ShopRoom r:
                    room = r;
                    break;
                default:
                    break;
            }
        }
    }
}
