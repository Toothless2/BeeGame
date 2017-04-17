using UnityEngine;
using BeeGame.Core;

namespace BeeGame.Player
{
    /// <summary>
    /// The look for the player
    /// </summary>
    public class PlayerLook : MonoBehaviour
    {
        #region Data
        /// <summary>
        /// Player transfrom
        /// </summary>
        public Transform myTransform;
        /// <summary>
        /// Camera transfom
        /// </summary>
        public Transform cameraTransform;
        /// <summary>
        /// Lock for camera X rotation
        /// </summary>
        [Range(0, 360)]
        public float rotationLock;
        /// <summary>
        /// Look move speed
        /// </summary>
        public float speed = 5;
        /// <summary>
        /// Current Y rotation
        /// </summary>
        float yRot = 0;
        /// <summary>
        /// Current X rotation
        /// </summary>
        float xRot = 0;
        #endregion

        #region Unity Methods
        /// <summary>
        /// Locks teh cursor and hides it
        /// </summary>
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        /// <summary>
        /// Every fixed update check if the look shoud be moved
        /// </summary>
        void Update()
        {
            //*the look wil not update when a inventory GUI is open
            if (!THInput.isAnotherInventoryOpen)
            {
                Look();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Moves the look rotation
        /// </summary>
        void Look()
        {
            //Only X/Y rotation needed as Z rotation would be wierd
            yRot += Input.GetAxis("Mouse X") * speed * Time.timeScale;
            xRot -= Input.GetAxis("Mouse Y") * speed * Time.timeScale;

            //clamps the X rotation so the player camera cannot do flips
            xRot = Mathf.Clamp(xRot, -rotationLock, rotationLock);

            myTransform.rotation = Quaternion.Euler(0, yRot, 0);
            cameraTransform.localRotation = Quaternion.Euler(xRot, 0, 0);
        }
        #endregion
    }
}