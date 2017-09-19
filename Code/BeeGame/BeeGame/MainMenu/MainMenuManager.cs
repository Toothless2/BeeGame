using System.Linq;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BeeGame.MainMenu
{
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject worldButton;
        public Transform scrollList;
        public Text worldInput;
        public string[] currentWorlds;
        public GameObject failText;

        protected void Start()
        {
            GetWorlds();
            MakeWorldButtons();
        }

        public void NewWorld()
        {
            string inputText = worldInput.text;

            if(inputText.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                Failure($"\"{inputText}\" is an invalid file name please try again");
                return;
            }

            foreach (var item in currentWorlds)
            {
                if (inputText.ToLower(new System.Globalization.CultureInfo("en-US")) == item.Split(new char[] { '/', '\\' }).Last().ToLower(new System.Globalization.CultureInfo("en-US")))
                {
                    Failure($"A World with the name \"{inputText}\" already exists");
                    return;
                }
            }

            LoadWorld(inputText.ToLower(new System.Globalization.CultureInfo("en-US")));
        }

        public void Failure(string reason)
        {
            failText.SetActive(true);
            failText.GetComponentInChildren<Text>().text = reason;
        }

        public void CloseFailDialog()
        {
            failText.SetActive(false);
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

            if (currentWorlds.Length <= 0)
                scrollList.parent.parent.gameObject.SetActive(false);
            else
                scrollList.parent.parent.gameObject.SetActive(true);
        }

        private void LoadWorld(string worldName)
        {
            Serialization.Serialization.worldName = worldName;

            SceneManager.LoadScene(0);
        }
    }
}
