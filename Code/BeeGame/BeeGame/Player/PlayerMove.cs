using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Core;

namespace BeeGame.Player
{
    /// <summary>
    /// Moves the player
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMove : MonoBehaviour
    {
        #region Data
        /// <summary>
        /// Speed of the player
        /// </summary>
        public float speed = 10f;
        /// <summary>
        /// Gravity of the player
        /// </summary>
        public float gravity = 9.81f;
        /// <summary>
        /// Max velocity of the player
        /// </summary>
        public float maxVelocity = 10f;

        /// <summary>
        /// Can the player jump?
        /// </summary>
        private bool canJump = false;
        /// <summary>
        /// How high can the player jump
        /// </summary>
        public float jumpHeight = 2f;

        /// <summary>
        /// Rigidbody for the player
        /// </summary>
        private Rigidbody myRigidBody;
        #endregion

        #region Unity Methods
        /// <summary>
        /// Gets the rigidbody and sets its variables
        /// </summary>
        private void Awake()
        {
            myRigidBody = GetComponent<Rigidbody>();

            //i want to use myown gravity and rotation
            myRigidBody.useGravity = false;
            myRigidBody.freezeRotation = true;
        }

        /// <summary>
        /// Updates the player move
        /// </summary>
        void FixedUpdate()
        {
            //If the player is grounded it can move
            if (canJump)
            {
                MovePlayer();
            }

            //adds the downward force
            myRigidBody.AddForce(new Vector3(0, myRigidBody.mass * -gravity, 0));
        }

        /// <summary>
        /// Sets that the player can jump when it hits the ground
        /// </summary>
        /// <param name="collision">What the player hit</param>
        private void OnCollisionStay(Collision collision)
        {
            canJump = true;
        }
        #endregion

        #region Movement Methods
        /// <summary>
        /// Moves the player
        /// </summary>
        void MovePlayer()
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

        /// <summary>
        /// Vertical Jump speed of the character
        /// </summary>
        /// <returns>Speed of the jump</returns>
        float VerticalJumpSpeed()
        {
            //Gets the correct of fore required for the player to reach the desired apex
            //Can this be done without Square Root as that take alot of work?
            return Mathf.Sqrt(2 * jumpHeight * gravity);
        }
        #endregion
    }
}
