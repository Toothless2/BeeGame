using UnityEngine;
using BeeGame.Blocks;
using BeeGame.Terrain.Chunks;
using static BeeGame.Terrain.LandGeneration.Terrain;
using static BeeGame.Core.THInput;

namespace BeeGame.Player
{
    /// <summary>
    /// Moves the <see cref="Block"/> selector
    /// </summary>
    public class Selector : MonoBehaviour
    {
        #region Data
        /// <summary>
        /// Selector
        /// </summary>
        public GameObject selector;

        /// <summary>
        /// Layers for the selector to look at
        /// </summary>
        public LayerMask layers;
        /// <summary>
        /// Where the raycast hit
        /// </summary>
        private RaycastHit hit;
        #endregion

        #region Unity Methods
        /// <summary>
        /// Make the selector
        /// </summary>
        void Awake()
        {
            selector = Instantiate(selector);
        }

        /// <summary>
        /// Updates the selector if an inventory is not open
        /// </summary>
        void FixedUpdate()
        {
            if(!isAnotherInventoryOpen)
                UpdateSelector();
        }

        /// <summary>
        /// Breaks and places a <see cref="Block"/> if an inventory is no open
        /// </summary>
        void Update()
        {
            if (!isAnotherInventoryOpen)
            {
                if (GetButtonDown("Break Block"))
                    BreakBlock();
                if (GetButtonDown("Place"))
                    PlaceBlock();
            }
        }
        #endregion

        #region Update
        /// <summary>
        /// Updates teh selectors position
        /// </summary>
        void UpdateSelector()
        {
            if (Physics.Raycast(transform.position, transform.forward, out hit, 15, layers))
            {
                selector.SetActive(true);
                selector.transform.position = GetBlockPos(hit);
                //*selector.SetActive(BlockInPosition(GetBlockPos(hit), hit.collider.GetComponent<Chunk>()));
            }
            else
            {
                selector.SetActive(false);
            }
        }
        #endregion

        #region Break/Place
        /// <summary>
        /// Breaks the <see cref="Block"/> in the selectors postion
        /// </summary>
        void BreakBlock()
        {
            Chunk chunk = GetChunk(selector.transform.position);

            Block block = chunk.world.GetBlock((int)selector.transform.position.x, (int)selector.transform.position.y, (int)selector.transform.position.z);

            if (!block.breakable)
                return;

            chunk.world.SetBlock((int)selector.transform.position.x, (int)selector.transform.position.y, (int)selector.transform.position.z, new Air(), true);


            block.BreakBlock(selector.transform.position);
        }

        /// <summary>
        /// Places s <see cref="Block"/> in the selector postion
        /// </summary>
        void PlaceBlock()
        {
            Chunk chunk = GetChunk(selector.transform.position);

            if (chunk == null)
                return;

            chunk.world.SetBlock((int)(selector.transform.position.x + hit.normal.x), (int)(selector.transform.position.y + hit.normal.y), (int)(selector.transform.position.z + hit.normal.z), new Block(), true);
        }
        #endregion
    }
}
