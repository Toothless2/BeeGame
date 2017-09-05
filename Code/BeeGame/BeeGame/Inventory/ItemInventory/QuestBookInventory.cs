using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeeGame.Quest;
using BeeGame.Items;
using UnityEngine;
using UnityEngine.UI;
using static BeeGame.Core.THInput;

namespace BeeGame.Inventory
{
    public class QuestBookInventory : Inventory
    {
        public Button questButton;
        public GameObject scrollObject;
        internal QuestBook myItem;

        protected void Start()
        {
            AddQuests();
        }

        protected void Update()
        {
            if (GetButtonDown("Close Menu/Inventory"))
                ToggleInventory(this);
        }

        public void AddQuests()
        {
            for (int i = 0; i < scrollObject.transform.childCount; i++)
            {
                Destroy(scrollObject.transform.GetChild(i).gameObject);
            }

            var compleatedQuests = Quests.ReturnCompleatedQuests();
            var currentQuests = Quests.ReturnCurrentQuests();
            var compleatClaimedQuests = Quests.ReturnCompleatedClaimedQuests();
                
            for (int i = 0; i < compleatedQuests.Count; i++)
            {
                var go = Instantiate(questButton);

                go.transform.SetParent(scrollObject.transform, false);

                go.gameObject.SetActive(true);

                go.GetComponentInChildren<Text>().text = (string)compleatedQuests[compleatedQuests.Keys.ElementAt(i)][compleatedQuests[compleatedQuests.Keys.ElementAt(i)].Length - 1];
                go.GetComponentInChildren<Text>().color = Color.yellow;

                var key = compleatedQuests.Keys.ElementAt(i);
                go.GetComponent<Button>().onClick.AddListener(() => QuestAchived(key));
            }

            for (int i = 0; i < currentQuests.Count; i++)
            {
                var go = Instantiate(questButton);

                go.transform.SetParent(scrollObject.transform, false);
                
                go.gameObject.SetActive(true);

                go.GetComponentInChildren<Text>().text = (string)currentQuests[currentQuests.Keys.ElementAt(i)][currentQuests[currentQuests.Keys.ElementAt(i)].Length - 1];
                go.GetComponentInChildren<Text>().color = Color.red;
                
            }

            for (int i = 0; i < compleatClaimedQuests.Count; i++)
            {
                var go = Instantiate(questButton);

                go.transform.SetParent(scrollObject.transform, false);

                go.gameObject.SetActive(true);

                go.GetComponentInChildren<Text>().text = (string)compleatClaimedQuests[compleatClaimedQuests.Keys.ElementAt(i)][compleatClaimedQuests[compleatClaimedQuests.Keys.ElementAt(i)].Length - 1];
                go.GetComponentInChildren<Text>().color = Color.green;

            }
        }

        private void QuestAchived(string key)
        {
            Quests.ClaimQuest(key);
            AddQuests();
            Serialization.Serialization.SaveQuests();
        }

        #region Open/Close Inventory
        /// <summary>
        /// Opens and closes this inventory
        /// </summary>
        /// <param name="inv"></param>
        public override void ToggleInventory(Inventory inv)
        {
            thisInventoryOpen = !thisInventoryOpen;

            isAnotherInventoryOpen = thisInventoryOpen;

            if (this.gameObject.activeInHierarchy && !thisInventoryOpen)
            {
                chestOpen = false;

                //* removes all of the items from thsi inventory

                //* tells item that inventory has been closed
                myItem.OpenItemInvnetory();
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                //* destroys this as it is not needed
                Destroy(this.gameObject);
            }
            else
            {
                chestOpen = true;

                //* hides and locks the cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        #endregion
    }
}