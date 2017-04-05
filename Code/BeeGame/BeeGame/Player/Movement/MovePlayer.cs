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
        
        void FixedUpdate()
        {
            //Player will not move if a UI is open
            if (!THInput.isAnotherInventoryOpen)
                Move();
        }

        void Move()
        {
            myConroller.SimpleMove(new THVector3(0, -0.001f, 0));

            if(THInput.GetButton("Forward"))
            {
                myConroller.SimpleMove(transform.forward * speed * Time.deltaTime * Time.timeScale);
            }

            if(THInput.GetButton("Backward"))
            {
                myConroller.SimpleMove(transform.forward * -speed * Time.deltaTime * Time.timeScale);
            }

            if(THInput.GetButton("Right"))
            {
                myConroller.SimpleMove(transform.right * speed * Time.deltaTime * Time.timeScale);
            }

            if (THInput.GetButton("Left"))
            {
                myConroller.SimpleMove(transform.right * -speed * Time.deltaTime * Time.timeScale);
            }

            if(THInput.GetButton("Jump"))
            {
                myConroller.Move((THVector3)(Vector3.up * speed * Time.deltaTime * Time.timeScale));
            }
        }
    }
}
