using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BeeGame.MainMenu
{
    public class DeleteWorld : MonoBehaviour, IPointerClickHandler
    {
        public Text worldNamme;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Directory.Delete($"{Application.dataPath}/{Serialization.Serialization.saveFolderName}/{worldNamme.text}", true);
                FindObjectOfType<MainMenuManager>().GetWorlds();
                FindObjectOfType<MainMenuManager>().MakeWorldButtons();
                Destroy(this.gameObject);
            }
        }
    }
}
