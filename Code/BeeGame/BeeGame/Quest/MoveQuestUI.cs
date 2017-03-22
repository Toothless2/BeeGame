using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using BeeGame.Core;

namespace BeeGame.Quest
{
    public class MoveQuestUI : MonoBehaviour
    {
        THVector3 mousePoz = new THVector3();

        void Update()
        {
            if(InputManager.GetButtonDown("Break Block"))
            {
                mousePoz = Input.mousePosition.ToTHVecotr3();
            }

            if (InputManager.GetButton("Break Block"))
            {
                if(Input.mousePosition.ToTHVecotr3() != mousePoz)
                {
                    THVector3 moveDirection = Input.mousePosition.ToTHVecotr3() - mousePoz;
                    transform.position = (moveDirection + transform.position.ToTHVecotr3()).ToUnityVector3();
                    mousePoz = Input.mousePosition.ToTHVecotr3();
                }
            }
        }
    }
}
