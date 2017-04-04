using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeeGame.Player
{
    public class ExitShop : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.transform.position = new THVector3(0, 1, 0);
            print("Leat Me Out!!!!");
        }
    }
}
