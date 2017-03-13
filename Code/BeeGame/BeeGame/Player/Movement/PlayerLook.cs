using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BeeGame.Player.Movement
{
    [RequireComponent(typeof(MovePlayer))]
    public class PlayerLook : MonoBehaviour
    {
        public Transform myTransform;
        [Range(0, 360)]
        public float rotationLock;
        public float speed = 5;
        float yRot = 0;
        float xRot = 0;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            Look();
        }

        void Look()
        {
            yRot += Input.GetAxis("Mouse X") * speed * Time.timeScale;
            xRot -= Input.GetAxis("Mouse Y") * speed * Time.timeScale;

            xRot = Mathf.Clamp(xRot, -rotationLock, rotationLock);

            myTransform.rotation = Quaternion.Euler(xRot, yRot, 0);
        }
    }
}