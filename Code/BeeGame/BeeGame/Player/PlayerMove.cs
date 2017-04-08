using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Core;

namespace BeeGame.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMove : MonoBehaviour
    {
        public float speed = 10f;
        public float gravity = 9.81f;
        public float maxVelocity = 10f;

        private bool canJump = false;
        public float jumpHeight = 2f;

        private Rigidbody myRigidBody;

        private void Awake()
        {
            myRigidBody = GetComponent<Rigidbody>();

            myRigidBody.useGravity = false;
            myRigidBody.freezeRotation = true;
        }

        void FixedUpdate()
        {
            if (canJump)
            {
                //Calculate the speed we want to achive
                Vector3 targetVelocity = new Vector3(THInput.GetAxis("Horizontal"), 0, THInput.GetAxis("Vertical"));
                targetVelocity = transform.TransformDirection(targetVelocity);
                targetVelocity *= speed;

                //Apply a force to reach the target speed
                Vector3 velocity = myRigidBody.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);

                //Clamping the velocity so that the player does not infinatly accelerate
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocity, maxVelocity);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocity, maxVelocity);
                velocityChange.y = 0;

                //Adds the force to the player so they move in the correct direction
                myRigidBody.AddForce(velocityChange, ForceMode.Impulse);

                //Jumping
                if (canJump && THInput.GetButton("Jump"))
                {
                    canJump = false;
                    myRigidBody.velocity = new Vector3(velocity.x, VerticalJumpSpeed(), velocity.z);
                }
            }

            myRigidBody.AddForce(new Vector3(0, myRigidBody.mass * -gravity, 0));
        }

        private void OnCollisionStay(Collision collision)
        {
            canJump = true;
        }

        bool CanJump()
        {
            Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit);

            if(Vector3.Distance(hit.point, transform.position) < 5)
                return true;

            return false;
        }

        float VerticalJumpSpeed()
        {
            //Gets the correct of fore required for the player to reach the desired apex
            //Can this be done without Square Root as that take alot of work?
            return Mathf.Sqrt(2 * jumpHeight * gravity);
        }
    }
}
