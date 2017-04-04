using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Enums;

namespace BeeGame.Rooms
{
    public class RoomGameObjectInterface : MonoBehaviour
    {
        public Room room;
        private GameObject[] madeWalls = new GameObject[4];

        private void Awake()
        {
            room = new StandardGarden();
            MakeRoom();
        }

        public void AddConnection(Connection direction)
        {
            var g = room as StandardGarden;
            g.AddConnection(direction);
            RemoveWall(g);
        }
        
        void RemoveWall(StandardGarden gard)
        {
            for (int i = 0; i < gard.direction.Length; i++)
            {
                if(gard.direction[i] != Connection.NOTCONNECTED)
                {
                    Destroy(transform.GetChild(0).GetChild(0).GetChild((int)gard.direction[i] - 1).gameObject);
                }
            }
        }

        void MakeRoom()
        {
            Instantiate(room.RoomModel, transform, false);
            transform.position = room.Position;

            switch (room)
            {
                default:
                    var g = room as StandardGarden;
                    RemoveWall(g);
                    break;
            }
        }
    }
}
