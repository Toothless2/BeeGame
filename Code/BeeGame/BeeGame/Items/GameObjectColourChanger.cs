using UnityEngine;

namespace BeeGame.Items.ColorChanger
{
    public class GameObjectColourChanger : MonoBehaviour
    {
        public void UpdateCombColour(Color color)
        {
            GetComponent<Renderer>().material.SetColor("_OverlayColour", color);
        }
    }
}
