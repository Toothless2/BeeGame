using UnityEngine;
using BeeGame.Blocks;
using BeeGame.Terrain.Chunks;
using static BeeGame.Terrain.LandGeneration.Terrain;
using static BeeGame.Core.THInput;

using BeeGame.Core;
using System.Diagnostics;

namespace BeeGame.Player
{
    public class Selector : MonoBehaviour
    {
        public GameObject selector;

        public LayerMask layers;
        private RaycastHit hit;

        private void Start()
        {
            //Block block = new Block();

            //Stopwatch st = new Stopwatch();
            //st.Start();
            //Block block2 = block.CloneObject();
            //st.Stop();

            //Stopwatch st2 = new Stopwatch();
            //st2.Start();
            //Block block3 = (Block)block.Clone();
            //st2.Stop();

            //print($"{st.ElapsedTicks} : {st2.ElapsedTicks} : Reflection is {st2.ElapsedTicks / st.ElapsedTicks} times faster");
        }

        private void Awake()
        {
            selector = Instantiate(selector);
        }

        void FixedUpdate()
        {
            if(!isAnotherInventoryOpen)
                UpdateSelector();
        }

        void Update()
        {
            if (GetButtonDown("Break Block"))
                BreakBlock();
            if (GetButtonDown("Place"))
                PlaceBlock();
        }

        void UpdateSelector()
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 15, layers))
            {
                selector.SetActive(true);
                selector.transform.position = GetBlockPos(hit);
                //selector.SetActive(BlockInPosition(GetBlockPos(hit), hit.collider.GetComponent<Chunk>()));
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

            chunk.world.SetBlock((int)selector.transform.position.x, (int)selector.transform.position.y, (int)selector.transform.position.z, new Air(), true);


            block.BreakBlock(selector.transform.position);
        }

        void PlaceBlock()
        {
            Chunk chunk = GetChunk(selector.transform.position);

            if (chunk == null)
                return;

            chunk.world.SetBlock((int)(selector.transform.position.x + hit.normal.x), (int)(selector.transform.position.y + hit.normal.y), (int)(selector.transform.position.z + hit.normal.z), new Block(), true);
        }
    }
}
