using System;
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
        private static Dictionary<string, object> inputButtons = new Dictionary<string, object>()
        {
            {"Forward" , KeyCode.W},
            {"Backward", KeyCode.S },
            {"Right", KeyCode.D },
            {"Left", KeyCode.A },
            {"Player Inventory", KeyCode.E },
            {"Quest Book", KeyCode.Mouse1 },
            {"Interact", KeyCode.Mouse1 },
            {"Place", KeyCode.Mouse1 },
            {"Break Block", KeyCode.Mouse0 },
            {"Close Menu/Inventory", new KeyCode[2] { KeyCode.Escape, KeyCode.E } },
            {"Jump", KeyCode.Space }
        };

        /// <summary>
        /// If another inventory is open true, else false
        /// </summary>
        public static bool isAnotherInventoryOpen;

        /// <summary>
        /// Was a Block inventory just closed
        /// </summary>
        public static bool blockInventoryJustClosed;

        /// <summary>
        /// Has the given button been pressed this update
        /// </summary>
        /// <param name="button">The button name eg "Inventory"</param>
        /// <returns>true if the given button has been pressed this update</returns>
        public static bool GetButtonDown(string button)
        {
            if (!inputButtons.ContainsKey(button))
            {
                throw new Exception("Input Manager: Key button name not defined: " + button);
            }

            switch (inputButtons[button])
            {
                case KeyCode[] arry:
                    //*for each posible key, check if it was pressed and if it was return that it was, if none of them was poressed return false
                    foreach (var item in arry)
                    {
                        if (Input.GetKeyDown(item))
                        {
                            return true;
                        }
                    }

                    return false;
                default:
                    return Input.GetKeyDown((KeyCode)inputButtons[button]);
            }
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

            switch (inputButtons[button])
            {
                case KeyCode[] arry:
                    //*for each posible key, check if it was pressed and if it was return that it was, if none of them was poressed return false
                    foreach (var item in arry)
                    {
                        if (Input.GetKey(item))
                        {
                            return true;
                        }
                    }

                    return false;
                default:
                    return Input.GetKey((KeyCode)inputButtons[button]);
            }
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

            switch (inputButtons[button])
            {
                case KeyCode[] arry:
                    //*for each posible key, check if it was pressed and if it was return that it was, if none of them was poressed return false
                    foreach (var item in arry)
                    {
                        if (Input.GetKeyUp(item))
                        {
                            return true;
                        }
                    }

                    return false;
                default:
                    return Input.GetKeyUp((KeyCode)inputButtons[button]);
            }
        }

        /// <summary>
        /// Gets the axis of a button press
        /// </summary>
        /// <param name="axis">Axis to check, Horizontal or Vertical</param>
        /// <returns>+1 or -1</returns>
        public static int GetAxis(string axis)
        {
            int returnAxis = 0;

            if (axis == "Horizontal")
            {
                if (GetButton("Right"))
                {
                    returnAxis += 1;
                }

                if (GetButton("Left"))
                {
                    returnAxis -= 1;
                }
            }
            else if (axis == "Vertical")
            {
                if (GetButton("Forward"))
                {
                    returnAxis += 1;
                }

                if (GetButton("Backward"))
                {
                    returnAxis -= 1;
                }
            }

            return returnAxis;
        }
    }
}
