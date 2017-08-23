using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeeGame.Items
{
    /// <summary>
    /// Applies a given colour to a gameobject
    /// </summary>
    public class ApplyColour : MonoBehaviour
    {
        #region Data
        /// <summary>
        /// Colour to apply
        /// </summary>
        public Color colour;
        /// <summary>
        /// Objects to apply the colour too
        /// </summary>
        /// <remarks>
        /// Array set in the editor
        /// </remarks>
        public GameObject[] objects;
        #endregion

        #region Unity Methods
        /// <summary>
        /// Applies the colour to the <see cref="GameObject"/>s in the <see cref="objects"/> array
        /// </summary>
        private void Start()
        {
            //* applies the correct colour to each object in the array
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].GetComponent<Renderer>().material.SetColor("_OverlayColour", colour);
            }
        }
        #endregion
    }
}
