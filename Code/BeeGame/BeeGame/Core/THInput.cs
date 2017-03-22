using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeeGame.Core
{
    /// <summary>
    /// My implementation of the unity input system. Acts as a buffer layer to the unity system so that the input keys can be changed at runtime
    /// </summary>
    public static class THInput
    {
        /// <summary>
        /// Button identifiers and <see cref="KeyCode"/>
        /// </summary>
        static Dictionary<string, KeyCode> inputButtons = new Dictionary<string, KeyCode>()
        {
            {"Forward" , KeyCode.W},
            {"Backward", KeyCode.S },
            {"Right", KeyCode.D },
            {"Left", KeyCode.A },
            {"Inventory", KeyCode.E },
            {"Place/Interact", KeyCode.Mouse1 },
            {"Break Block", KeyCode.Mouse0 }
        };

        /// <summary>
        /// Has the given button been pressed this update
        /// </summary>
        /// <param name="button">The button name eg "Inventory"</param>
        /// <returns>true if the given button has been pressed this update</returns>
        public static bool GetButtonDown(string button)
        {
            if(!inputButtons.ContainsKey(button))
            {
                throw new Exception("Input Manager: Key button name not defined: " + button);
            }

            return Input.GetKeyDown(inputButtons[button]);
        }
        /// <summary>
        /// Is the given button currently being held down
        /// </summary>
        /// <param name="button">The button name eg "Forward"</param>
        /// <returns>true if the given button is currently being held down</returns>
        public static bool GetButton(string button)
        {
            if (!inputButtons.ContainsKey(button))
            {
                throw new Exception("Input Manager: Key button name not defined: " + button);
            }

            return Input.GetKey(inputButtons[button]);
        }
        /// <summary>
        /// Has the given button been relesed this update
        /// </summary>
        /// <param name="button">Button name eg "Inventory"</param>
        /// <returns>true if the button has been relesed during this update</returns>
        public static bool GetButtonUp(string button)
        {
            if (!inputButtons.ContainsKey(button))
            {
                throw new Exception("Input Manager: Key button name not defined: " + button);
            }

            return Input.GetKeyUp(inputButtons[button]);
        }
    }
}
