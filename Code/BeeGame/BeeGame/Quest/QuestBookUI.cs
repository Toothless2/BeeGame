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

            if (THInput.GetButtonDown("Quest Book"))
            {
                ShowHideUI();
            }
        }

        void ShowHideUI()
        {
            if (!THInput.isAnotherInventoryOpen)
            {
                THInput.isAnotherInventoryOpen = true;

                uIOpen = true;
                questUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else if(questUI.activeInHierarchy)
            {
                uIOpen = false;
                questUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                THInput.isAnotherInventoryOpen = false;
            }
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
