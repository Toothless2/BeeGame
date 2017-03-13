using System.Collections;
using System.Collections.Generic;
using BeeGame.Core;
using UnityEngine;

namespace BeeGame.Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public class MovePlayer : MonoBehaviour
    {
        public CharacterController myConroller;
        public float speed;
        
        void Update()
        {
            Move();
        }

        void Move()
        {
            if(InputManager.GetButton("Forward"))
            {
                myConroller.SimpleMove(transform.forward * speed * Time.deltaTime * Time.timeScale);
            }

            if(InputManager.GetButton("Backward"))
            {
                myConroller.SimpleMove(transform.forward * -speed * Time.deltaTime * Time.timeScale);
            }

            if(InputManager.GetButton("Right"))
            {
                myConroller.SimpleMove(transform.right * speed * Time.deltaTime * Time.timeScale);
            }

            if (InputManager.GetButton("Left"))
            {
                myConroller.SimpleMove(transform.right * -speed * Time.deltaTime * Time.timeScale);
            }
        }
    }
}
