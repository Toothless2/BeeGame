using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeeGame.Player
{
    public class EnterShop : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.transform.position = new THVector3(0, -9, 0);
        }
    }
}
