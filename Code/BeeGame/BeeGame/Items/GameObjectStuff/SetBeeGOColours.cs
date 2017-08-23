using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.Core.Enums;
using BeeGame.Core.Dictionaries;
using UnityEngine;

namespace BeeGame.Items
{
    public class SetBeeGOColours : MonoBehaviour
    {
        public GameObject[] objects;
        public GameObject crown;
        public GameObject tiara;
        public BeeType beeType;
        public Color colour;

        protected void Start()
        {
            switch (beeType)
            {
                case BeeType.DRONE:
                    crown.SetActive(false);
                    tiara.SetActive(false);
                    break;
                case BeeType.PRINCESS:
                    crown.SetActive(false);
                    break;
            }

            foreach (var item in objects)
            {
                item.GetComponent<Renderer>().material.SetColor("_Color", colour);
            }
        }
    }
}
