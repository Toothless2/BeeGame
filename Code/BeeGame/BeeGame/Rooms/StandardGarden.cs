using System;
using UnityEngine;
using BeeGame.Enums;
using BeeGame.Core;

namespace BeeGame.Rooms
{
    [Serializable]
    public class StandardGarden : Room
    {
        private float instability = 0;

        private BeeHumidityPreferance humidity = BeeHumidityPreferance.TEMPERATE;
        private BeeTempPreferance temp = BeeTempPreferance.TEMPERATE;
        public Connection[] direction = new Connection[4];
        [NonSerialized]
        private GameObject[] walls;

        public BeeHumidityPreferance Humidity
        {
            get
            {
                return humidity;
            }

            set
            {
                humidity = value;
            }
        }

        public BeeTempPreferance Temp
        {
            get
            {
                return temp;
            }

            set
            {
                temp = value;
            }
        }

        public GameObject[] Walls
        {
            get
            {
                if(walls == null)
                {
                    GetWalls();
                }

                return walls;
            }

            set
            {
                walls = value;
            }
        }

        public StandardGarden()
        {
            RoomType = RoomType.STANDARDROOM;

            GetWalls();
        }

        void GetWalls()
        {
            walls = new GameObject[4] { RoomModel.transform.GetChild(0).GetChild(0).gameObject, RoomModel.transform.GetChild(0).GetChild(1).gameObject, RoomModel.transform.GetChild(0).GetChild(2).gameObject, RoomModel.transform.GetChild(0).GetChild(3).gameObject };

            for (int i = 0; i < walls.Length; i++)
            {
                switch (i)
                {
                    //\todo change when real walls are modeled
                    case 0:
                        walls[i].transform.position = new THVector3(0, 1, 14 / 2);
                        walls[i].transform.rotation = Quaternion.identity;
                        break;
                    case 1:
                        walls[i].transform.position = new THVector3(14 / 2, 1, 0);
                        walls[i].transform.rotation = new Quaternion(0, 90, 0, 0);
                        break;
                    case 2:
                        walls[i].transform.position = new THVector3(0, 1, -14 / 2);
                        walls[i].transform.rotation = Quaternion.identity;
                        break;
                    case 3:
                        walls[i].transform.position = new THVector3(-14 / 2, 1, 0);
                        walls[i].transform.rotation = new Quaternion(0, 90, 0, 0);
                        break;
                    default:
                        break;
                }
            }
        }

        public void IncreaseInstability(float increase)
        {
            if(increase > 0)
            {
                instability += increase;
            }
        }

        public void AddConnection(Connection direction)
        {
            this.direction[(int)direction - 1] = direction;
            walls[(int)direction - 1] = null;
        }

        public GameObject WallToDestroy(Connection direction)
        {
            return walls[(int)direction - 1];
        }
    }
}
