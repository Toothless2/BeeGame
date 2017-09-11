using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using BeeGame.Core;

namespace BeeGame.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject worldButton;
        public Transform scrollList;
        public Text worldInput;
        public string[] currentWorlds;

        protected void Start()
        {
            GetWorlds();
            MakeWorldButtons();
        }

        protected void Update()
        {
        }

        public void NewWorld()
        {
            foreach (var item in currentWorlds)
            {
                if (worldInput.text.ToLower(new System.Globalization.CultureInfo("en-US")) == item.Split(new char[] { '/', '\\' }).Last().ToLower(new System.Globalization.CultureInfo("en-US")))
                {
                    WorldAlreadyExists();
                    return;
                }
            }

            LoadWorld(worldInput.text.ToLower(new System.Globalization.CultureInfo("en-US")));
        }

        public void WorldAlreadyExists()
        {

        }

        public void GetWorlds()
        {
            var path = $"{Application.dataPath}/{Serialization.Serialization.saveFolderName}";

            if (!Directory.Exists(path))
                return;

            currentWorlds = Directory.GetDirectories(path);
        }

        public void MakeWorldButtons()
        {
            for (int i = 0; i < scrollList.childCount; i++)
            {
                Destroy(scrollList.GetChild(i).gameObject);
            }

            foreach (var item in currentWorlds)
            {
                var go = Instantiate(worldButton);

                go.gameObject.SetActive(true);
                go.transform.SetParent(scrollList, false);
                go.GetComponentInChildren<Text>().text = item.Split(new char[] { '/', '\\' }).Last();

                go.GetComponent<Button>().onClick.AddListener(() => LoadWorld(go.GetComponentInChildren<Text>().text));
            }
        }

        private void LoadWorld(string worldName)
        {
            Serialization.Serialization.worldName = worldName;

            SceneManager.LoadScene(0);
        }
    }
}
