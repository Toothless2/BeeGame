using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Core;

namespace BeeGame.TerrainGeneration
{
    public class Modify : MonoBehaviour
    {
        public GameObject selector;

        private void Update()
        {
            if (THInput.GetButtonDown("Interact"))
            {
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100))
                {
                    Terrain.SetBlock(hit, new Blocks.Apiary());
                }
            }

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit2, 100))
            {
                selector.transform.position = Terrain.GetBlockPos(hit2);
            }
        }
    }
}
