using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BeeGame.Terrain.LandGeneration.Terrain;

namespace BeeGame.Terrain
{
    public class Modify : MonoBehaviour
    {
        Vector2 rot;

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 100))
                {
                    SetBlock(hit, new Blocks.Air());
                }
            }

            rot = new Vector2(rot.x + Input.GetAxis("Mouse X") * 3, rot.y + Input.GetAxis("Mouse Y") * 3);

            transform.localRotation = Quaternion.AngleAxis(rot.x, Vector2.up);
            transform.localRotation *= Quaternion.AngleAxis(rot.y, Vector3.left);

            transform.position += transform.forward * 3 * Input.GetAxis("Vertical");
            transform.position += transform.right * 3 * Input.GetAxis("Horizontal");
        }
    }
}
