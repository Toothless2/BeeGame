using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeeGame.TerrainGeneration
{
    /// <summary>
    /// Load the chunks around the player and removes theones to far away
    /// </summary>
    public class LoadChunks : MonoBehaviour
    {
        public World world;

        private List<THVector3> updateList = new List<THVector3>();
        private List<THVector3> buildList = new List<THVector3>();

        /// <summary>
        /// Array of blocks to be renderd first as they will be the closest to the player. Never changes so calculation ar runtime would be a waste of time
        /// </summary>
        static THVector3[] chunkPositions = {   new THVector3( 0, 0,  0), new THVector3(-1, 0,  0), new THVector3( 0, 0, -1), new THVector3( 0, 0,  1), new THVector3( 1, 0,  0),
                             new THVector3(-1, 0, -1), new THVector3(-1, 0,  1), new THVector3( 1, 0, -1), new THVector3( 1, 0,  1), new THVector3(-2, 0,  0),
                             new THVector3( 0, 0, -2), new THVector3( 0, 0,  2), new THVector3( 2, 0,  0), new THVector3(-2, 0, -1), new THVector3(-2, 0,  1),
                             new THVector3(-1, 0, -2), new THVector3(-1, 0,  2), new THVector3( 1, 0, -2), new THVector3( 1, 0,  2), new THVector3( 2, 0, -1),
                             new THVector3( 2, 0,  1), new THVector3(-2, 0, -2), new THVector3(-2, 0,  2), new THVector3( 2, 0, -2), new THVector3( 2, 0,  2),
                             new THVector3(-3, 0,  0), new THVector3( 0, 0, -3), new THVector3( 0, 0,  3), new THVector3( 3, 0,  0), new THVector3(-3, 0, -1),
                             new THVector3(-3, 0,  1), new THVector3(-1, 0, -3), new THVector3(-1, 0,  3), new THVector3( 1, 0, -3), new THVector3( 1, 0,  3),
                             new THVector3( 3, 0, -1), new THVector3( 3, 0,  1), new THVector3(-3, 0, -2), new THVector3(-3, 0,  2), new THVector3(-2, 0, -3),
                             new THVector3(-2, 0,  3), new THVector3( 2, 0, -3), new THVector3( 2, 0,  3), new THVector3( 3, 0, -2), new THVector3( 3, 0,  2),
                             new THVector3(-4, 0,  0), new THVector3( 0, 0, -4), new THVector3( 0, 0,  4), new THVector3( 4, 0,  0), new THVector3(-4, 0, -1),
                             new THVector3(-4, 0,  1), new THVector3(-1, 0, -4), new THVector3(-1, 0,  4), new THVector3( 1, 0, -4), new THVector3( 1, 0,  4),
                             new THVector3( 4, 0, -1), new THVector3( 4, 0,  1), new THVector3(-3, 0, -3), new THVector3(-3, 0,  3), new THVector3( 3, 0, -3),
                             new THVector3( 3, 0,  3), new THVector3(-4, 0, -2), new THVector3(-4, 0,  2), new THVector3(-2, 0, -4), new THVector3(-2, 0,  4),
                             new THVector3( 2, 0, -4), new THVector3( 2, 0,  4), new THVector3( 4, 0, -2), new THVector3( 4, 0,  2), new THVector3(-5, 0,  0),
                             new THVector3(-4, 0, -3), new THVector3(-4, 0,  3), new THVector3(-3, 0, -4), new THVector3(-3, 0,  4), new THVector3( 0, 0, -5),
                             new THVector3( 0, 0,  5), new THVector3( 3, 0, -4), new THVector3( 3, 0,  4), new THVector3( 4, 0, -3), new THVector3( 4, 0,  3),
                             new THVector3( 5, 0,  0), new THVector3(-5, 0, -1), new THVector3(-5, 0,  1), new THVector3(-1, 0, -5), new THVector3(-1, 0,  5),
                             new THVector3( 1, 0, -5), new THVector3( 1, 0,  5), new THVector3( 5, 0, -1), new THVector3( 5, 0,  1), new THVector3(-5, 0, -2),
                             new THVector3(-5, 0,  2), new THVector3(-2, 0, -5), new THVector3(-2, 0,  5), new THVector3( 2, 0, -5), new THVector3( 2, 0,  5),
                             new THVector3( 5, 0, -2), new THVector3( 5, 0,  2), new THVector3(-4, 0, -4), new THVector3(-4, 0,  4), new THVector3( 4, 0, -4),
                             new THVector3( 4, 0,  4), new THVector3(-5, 0, -3), new THVector3(-5, 0,  3), new THVector3(-3, 0, -5), new THVector3(-3, 0,  5),
                             new THVector3( 3, 0, -5), new THVector3( 3, 0,  5), new THVector3( 5, 0, -3), new THVector3( 5, 0,  3), new THVector3(-6, 0,  0),
                             new THVector3( 0, 0, -6), new THVector3( 0, 0,  6), new THVector3( 6, 0,  0), new THVector3(-6, 0, -1), new THVector3(-6, 0,  1),
                             new THVector3(-1, 0, -6), new THVector3(-1, 0,  6), new THVector3( 1, 0, -6), new THVector3( 1, 0,  6), new THVector3( 6, 0, -1),
                             new THVector3( 6, 0,  1), new THVector3(-6, 0, -2), new THVector3(-6, 0,  2), new THVector3(-2, 0, -6), new THVector3(-2, 0,  6),
                             new THVector3( 2, 0, -6), new THVector3( 2, 0,  6), new THVector3( 6, 0, -2), new THVector3( 6, 0,  2), new THVector3(-5, 0, -4),
                             new THVector3(-5, 0,  4), new THVector3(-4, 0, -5), new THVector3(-4, 0,  5), new THVector3( 4, 0, -5), new THVector3( 4, 0,  5),
                             new THVector3( 5, 0, -4), new THVector3( 5, 0,  4), new THVector3(-6, 0, -3), new THVector3(-6, 0,  3), new THVector3(-3, 0, -6),
                             new THVector3(-3, 0,  6), new THVector3( 3, 0, -6), new THVector3( 3, 0,  6), new THVector3( 6, 0, -3), new THVector3( 6, 0,  3),
                             new THVector3(-7, 0,  0), new THVector3( 0, 0, -7), new THVector3( 0, 0,  7), new THVector3( 7, 0,  0), new THVector3(-7, 0, -1),
                             new THVector3(-7, 0,  1), new THVector3(-5, 0, -5), new THVector3(-5, 0,  5), new THVector3(-1, 0, -7), new THVector3(-1, 0,  7),
                             new THVector3( 1, 0, -7), new THVector3( 1, 0,  7), new THVector3( 5, 0, -5), new THVector3( 5, 0,  5), new THVector3( 7, 0, -1),
                             new THVector3( 7, 0,  1), new THVector3(-6, 0, -4), new THVector3(-6, 0,  4), new THVector3(-4, 0, -6), new THVector3(-4, 0,  6),
                             new THVector3( 4, 0, -6), new THVector3( 4, 0,  6), new THVector3( 6, 0, -4), new THVector3( 6, 0,  4), new THVector3(-7, 0, -2),
                             new THVector3(-7, 0,  2), new THVector3(-2, 0, -7), new THVector3(-2, 0,  7), new THVector3( 2, 0, -7), new THVector3( 2, 0,  7),
                             new THVector3( 7, 0, -2), new THVector3( 7, 0,  2), new THVector3(-7, 0, -3), new THVector3(-7, 0,  3), new THVector3(-3, 0, -7),
                             new THVector3(-3, 0,  7), new THVector3( 3, 0, -7), new THVector3( 3, 0,  7), new THVector3( 7, 0, -3), new THVector3( 7, 0,  3),
                             new THVector3(-6, 0, -5), new THVector3(-6, 0,  5), new THVector3(-5, 0, -6), new THVector3(-5, 0,  6), new THVector3( 5, 0, -6),
                             new THVector3( 5, 0,  6), new THVector3( 6, 0, -5), new THVector3( 6, 0,  5) };

        int timer = 0;

        private void Update()
        {
            DeleteChunk();
            FindChunksToLoad();
            LoadAndRenderChunks();
        }

        void LoadAndRenderChunks()
        {
            //builds 4 chunks in the build list per frame
            for (int i = 0; i < 4; i++)
            {
                if (buildList.Count != 0)
                {
                    BuildChunk(buildList[0]);
                    buildList.RemoveAt(0);
                }
            }

            //updates will only ever be 1 update per chunks buildso limit chunks built instead
            for (int i = 0; i < updateList.Count; i++)
            {
                Chunk chunk = world.GetChunk((int)updateList[0].x, (int)updateList[0].y, (int)updateList[0].z);

                if (chunk != null)
                    chunk.update = true;
                updateList.RemoveAt(0);
            }
        }

        void FindChunksToLoad()
        {
            THVector3 playerpos = new THVector3(Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize, Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize, Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize);

            //if the build list is empty
            if(buildList.Count == 0)
            {
                //make the chunks
                for (int i = 0; i < chunkPositions.Length; i++)
                {
                    //Converts player positon to chunk position
                    THVector3 newChunkPos = new THVector3(chunkPositions[i].x * Chunk.chunkSize + playerpos.x, 0, chunkPositions[i].z * Chunk.chunkSize + playerpos.z);

                    //Get the chunk in that postion
                    Chunk newChunk = world.GetChunk((int)newChunkPos.x, (int)newChunkPos.y, (int)newChunkPos.z);

                    //if the chunk already exists and it is rendered or about to be rendered continue
                    if (newChunk != null && (newChunk.rendered || updateList.Contains(newChunkPos)))
                        continue;

                    for (int y = -4; y < 4; y++)
                    {
                        buildList.Add(new THVector3(newChunkPos.x, newChunkPos.y * y, newChunkPos.z));
                    }

                    return;
                }
            }
        }

        /// <summary>
        /// Builds the chunk that that player is moveing towards
        /// </summary>
        /// <param name="pos">Positon of the chunk to build</param>
        void BuildChunk(THVector3 pos)
        {
            for (int y = (int)pos.y - Chunk.chunkSize; y <= pos.y + Chunk.chunkSize; y += Chunk.chunkSize)
            {
                if (y > 64 || y < -64)
                    continue;

                for (int x = (int)pos.x - Chunk.chunkSize; x <= pos.x + Chunk.chunkSize; x += Chunk.chunkSize)
                {
                    for (int z = (int)pos.z - Chunk.chunkSize; z <= pos.z + Chunk.chunkSize; z += Chunk.chunkSize)
                    {
                        //if their is not a chunk in that positon
                        if (world.GetChunk(x, y, z) == null)
                            ///make it
                            world.CreateChunk(x, y, z);
                    }
                }
            }

            updateList.Add(pos);
        }

        void DeleteChunk()
        {
            if (timer == 10)
            {
                var chunksToDelete = new List<THVector3>();

                foreach (var chunk in world.chunks)
                {
                    float distance = Vector3.Distance(new THVector3(chunk.Value.worldPos.x, 0, chunk.Value.worldPos.z), new THVector3(transform.position.x, 0, transform.position.z));

                    if (distance > 256)
                        chunksToDelete.Add(chunk.Key);
                }

                foreach (var chunk in chunksToDelete)
                {
                    world.DestroyChunk((int)chunk.x, (int)chunk.y, (int)chunk.z);
                }

                timer = 0;
            }

            timer++;
        }
    }
}
