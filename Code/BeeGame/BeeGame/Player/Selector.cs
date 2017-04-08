using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static BeeGame.Terrain.LandGeneration.Terrain;

namespace BeeGame.Player
{
    public class Selector : MonoBehaviour
    {
        public GameObject selector;

        void FixedUpdate()
        {
            if(Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 15))
            {
                selector.transform.position = GetBlockPos(hit);
                selector.SetActive(BlockInPosition(GetBlockPos(hit), hit.collider.GetComponent<Terrain.Chunks.Chunk>()));
            }
            else
            {
                selector.SetActive(false);
            }
        }
    }
}
