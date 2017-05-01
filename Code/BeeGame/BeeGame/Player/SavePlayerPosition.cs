using UnityEngine;
using BeeGame.Serialization;

namespace BeeGame.Player
{
    /// <summary>
    /// Saves the player postion
    /// </summary>
    public class SavePlayerPosition : MonoBehaviour
    {
        /// <summary>
        /// Timer for saveing the player
        /// </summary>
        int counter = 0;

        /// <summary>
        /// Saves the player every 1000 frames
        /// </summary>
        void Update()
        {
            if(counter == 0)
            {
                counter = 1000;
                Serialization.Serialization.SavePlayerPosition(transform);
                //print("saved player");
            }

            counter--;
        }
    }
}
