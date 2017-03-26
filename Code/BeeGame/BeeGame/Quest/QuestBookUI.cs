using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEditor;
using BeeGame.Core;

namespace BeeGame.Quest
{
    public class QuestBookUI : MonoBehaviour
    {
        public GameObject questUI;
        public GameObject moveUI;
        private THVector3 mousePoz = new THVector3();
        private bool uIOpen;

        void Update()
        {
            if(uIOpen)
                MoveUI();

            if (THInput.GetButtonDown("Place/Interact"))
            {
                uIOpen = !uIOpen;
                ShowHideUI();
            }
        }

        void ShowHideUI()
        {
            questUI.SetActive(!questUI.activeInHierarchy);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = !Cursor.visible;
        }

        void MoveUI()
        {
            if (THInput.GetButtonDown("Break Block"))
            {
                mousePoz = Input.mousePosition;
            }

            if (THInput.GetButton("Break Block"))
            {
                if (Input.mousePosition.ToTHVecotr3() != mousePoz)
                {
                    THVector3 moveDirection = Input.mousePosition.ToTHVecotr3() - mousePoz;
                    moveUI.transform.position = moveDirection + moveUI.transform.position.ToTHVecotr3();
                    mousePoz = Input.mousePosition;
                }
            }
        }
    }
}
