using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Blocks;
using BeeGame.Terrain.Chunks;
using BeeGame.Items;
using static BeeGame.Terrain.LandGeneration.Terrain;
using static BeeGame.Core.THInput;

namespace BeeGame.Player
{
    public class Selector : MonoBehaviour
    {
        public GameObject selector;

        void FixedUpdate()
        {
            UpdateSelector();

        }

        void Update()
        {
            if (GetButtonDown("Break Block"))
                BreakBlock();
        }

        void UpdateSelector()
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 15))
            {
                selector.transform.position = GetBlockPos(hit);
                selector.SetActive(BlockInPosition(GetBlockPos(hit), hit.collider.GetComponent<Terrain.Chunks.Chunk>()));
            }
            else
            {
                selector.SetActive(false);
            }
        }

        void BreakBlock()
        {
            Chunk chunk = GetChunk(selector.transform.position);

            Block block = chunk.world.GetBlock((int)selector.transform.position.x, (int)selector.transform.position.y, (int)selector.transform.position.z);

            if (!block.breakable)
                return;

            chunk.world.SetBlock((int)selector.transform.position.x, (int)selector.transform.position.y, (int)selector.transform.position.z, new Air());


            block.BreakBlock(selector.transform.position);
        }
    }
}
