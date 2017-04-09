using System;
using System.Collections.Generic;
using UnityEngine;
using BeeGame.Terrain.LandGeneration;

namespace BeeGame.Terrain.Chunks
{
    /// <summary>
    /// Loads the <see cref="Chunk"/>s around the player
    /// </summary>
    public class LoadChunks : MonoBehaviour
    {
        public World world;

        private List<ChunkWorldPos> updateList = new List<ChunkWorldPos>();
        private List<ChunkWorldPos> buildList = new List<ChunkWorldPos>();

        private static ChunkWorldPos[] chunkPositions = {   new ChunkWorldPos( 0, 0,  0), new ChunkWorldPos(-1, 0,  0), new ChunkWorldPos( 0, 0, -1), new ChunkWorldPos( 0, 0,  1), new ChunkWorldPos( 1, 0,  0),
                             new ChunkWorldPos(-1, 0, -1), new ChunkWorldPos(-1, 0,  1), new ChunkWorldPos( 1, 0, -1), new ChunkWorldPos( 1, 0,  1), new ChunkWorldPos(-2, 0,  0),
                             new ChunkWorldPos( 0, 0, -2), new ChunkWorldPos( 0, 0,  2), new ChunkWorldPos( 2, 0,  0), new ChunkWorldPos(-2, 0, -1), new ChunkWorldPos(-2, 0,  1),
                             new ChunkWorldPos(-1, 0, -2), new ChunkWorldPos(-1, 0,  2), new ChunkWorldPos( 1, 0, -2), new ChunkWorldPos( 1, 0,  2), new ChunkWorldPos( 2, 0, -1),
                             new ChunkWorldPos( 2, 0,  1), new ChunkWorldPos(-2, 0, -2), new ChunkWorldPos(-2, 0,  2), new ChunkWorldPos( 2, 0, -2), new ChunkWorldPos( 2, 0,  2),
                             new ChunkWorldPos(-3, 0,  0), new ChunkWorldPos( 0, 0, -3), new ChunkWorldPos( 0, 0,  3), new ChunkWorldPos( 3, 0,  0), new ChunkWorldPos(-3, 0, -1),
                             new ChunkWorldPos(-3, 0,  1), new ChunkWorldPos(-1, 0, -3), new ChunkWorldPos(-1, 0,  3), new ChunkWorldPos( 1, 0, -3), new ChunkWorldPos( 1, 0,  3),
                             new ChunkWorldPos( 3, 0, -1), new ChunkWorldPos( 3, 0,  1), new ChunkWorldPos(-3, 0, -2), new ChunkWorldPos(-3, 0,  2), new ChunkWorldPos(-2, 0, -3),
                             new ChunkWorldPos(-2, 0,  3), new ChunkWorldPos( 2, 0, -3), new ChunkWorldPos( 2, 0,  3), new ChunkWorldPos( 3, 0, -2), new ChunkWorldPos( 3, 0,  2),
                             new ChunkWorldPos(-4, 0,  0), new ChunkWorldPos( 0, 0, -4), new ChunkWorldPos( 0, 0,  4), new ChunkWorldPos( 4, 0,  0), new ChunkWorldPos(-4, 0, -1),
                             new ChunkWorldPos(-4, 0,  1), new ChunkWorldPos(-1, 0, -4), new ChunkWorldPos(-1, 0,  4), new ChunkWorldPos( 1, 0, -4), new ChunkWorldPos( 1, 0,  4),
                             new ChunkWorldPos( 4, 0, -1), new ChunkWorldPos( 4, 0,  1), new ChunkWorldPos(-3, 0, -3), new ChunkWorldPos(-3, 0,  3), new ChunkWorldPos( 3, 0, -3),
                             new ChunkWorldPos( 3, 0,  3), new ChunkWorldPos(-4, 0, -2), new ChunkWorldPos(-4, 0,  2), new ChunkWorldPos(-2, 0, -4), new ChunkWorldPos(-2, 0,  4),
                             new ChunkWorldPos( 2, 0, -4), new ChunkWorldPos( 2, 0,  4), new ChunkWorldPos( 4, 0, -2), new ChunkWorldPos( 4, 0,  2), new ChunkWorldPos(-5, 0,  0),
                             new ChunkWorldPos(-4, 0, -3), new ChunkWorldPos(-4, 0,  3), new ChunkWorldPos(-3, 0, -4), new ChunkWorldPos(-3, 0,  4), new ChunkWorldPos( 0, 0, -5),
                             new ChunkWorldPos( 0, 0,  5), new ChunkWorldPos( 3, 0, -4), new ChunkWorldPos( 3, 0,  4), new ChunkWorldPos( 4, 0, -3), new ChunkWorldPos( 4, 0,  3),
                             new ChunkWorldPos( 5, 0,  0), new ChunkWorldPos(-5, 0, -1), new ChunkWorldPos(-5, 0,  1), new ChunkWorldPos(-1, 0, -5), new ChunkWorldPos(-1, 0,  5),
                             new ChunkWorldPos( 1, 0, -5), new ChunkWorldPos( 1, 0,  5), new ChunkWorldPos( 5, 0, -1), new ChunkWorldPos( 5, 0,  1), new ChunkWorldPos(-5, 0, -2),
                             new ChunkWorldPos(-5, 0,  2), new ChunkWorldPos(-2, 0, -5), new ChunkWorldPos(-2, 0,  5), new ChunkWorldPos( 2, 0, -5), new ChunkWorldPos( 2, 0,  5),
                             new ChunkWorldPos( 5, 0, -2), new ChunkWorldPos( 5, 0,  2), new ChunkWorldPos(-4, 0, -4), new ChunkWorldPos(-4, 0,  4), new ChunkWorldPos( 4, 0, -4),
                             new ChunkWorldPos( 4, 0,  4), new ChunkWorldPos(-5, 0, -3), new ChunkWorldPos(-5, 0,  3), new ChunkWorldPos(-3, 0, -5), new ChunkWorldPos(-3, 0,  5),
                             new ChunkWorldPos( 3, 0, -5), new ChunkWorldPos( 3, 0,  5), new ChunkWorldPos( 5, 0, -3), new ChunkWorldPos( 5, 0,  3), new ChunkWorldPos(-6, 0,  0),
                             new ChunkWorldPos( 0, 0, -6), new ChunkWorldPos( 0, 0,  6), new ChunkWorldPos( 6, 0,  0), new ChunkWorldPos(-6, 0, -1), new ChunkWorldPos(-6, 0,  1),
                             new ChunkWorldPos(-1, 0, -6), new ChunkWorldPos(-1, 0,  6), new ChunkWorldPos( 1, 0, -6), new ChunkWorldPos( 1, 0,  6), new ChunkWorldPos( 6, 0, -1),
                             new ChunkWorldPos( 6, 0,  1), new ChunkWorldPos(-6, 0, -2), new ChunkWorldPos(-6, 0,  2), new ChunkWorldPos(-2, 0, -6), new ChunkWorldPos(-2, 0,  6),
                             new ChunkWorldPos( 2, 0, -6), new ChunkWorldPos( 2, 0,  6), new ChunkWorldPos( 6, 0, -2), new ChunkWorldPos( 6, 0,  2), new ChunkWorldPos(-5, 0, -4),
                             new ChunkWorldPos(-5, 0,  4), new ChunkWorldPos(-4, 0, -5), new ChunkWorldPos(-4, 0,  5), new ChunkWorldPos( 4, 0, -5), new ChunkWorldPos( 4, 0,  5),
                             new ChunkWorldPos( 5, 0, -4), new ChunkWorldPos( 5, 0,  4), new ChunkWorldPos(-6, 0, -3), new ChunkWorldPos(-6, 0,  3), new ChunkWorldPos(-3, 0, -6),
                             new ChunkWorldPos(-3, 0,  6), new ChunkWorldPos( 3, 0, -6), new ChunkWorldPos( 3, 0,  6), new ChunkWorldPos( 6, 0, -3), new ChunkWorldPos( 6, 0,  3),
                             new ChunkWorldPos(-7, 0,  0), new ChunkWorldPos( 0, 0, -7), new ChunkWorldPos( 0, 0,  7), new ChunkWorldPos( 7, 0,  0), new ChunkWorldPos(-7, 0, -1),
                             new ChunkWorldPos(-7, 0,  1), new ChunkWorldPos(-5, 0, -5), new ChunkWorldPos(-5, 0,  5), new ChunkWorldPos(-1, 0, -7), new ChunkWorldPos(-1, 0,  7),
                             new ChunkWorldPos( 1, 0, -7), new ChunkWorldPos( 1, 0,  7), new ChunkWorldPos( 5, 0, -5), new ChunkWorldPos( 5, 0,  5), new ChunkWorldPos( 7, 0, -1),
                             new ChunkWorldPos( 7, 0,  1), new ChunkWorldPos(-6, 0, -4), new ChunkWorldPos(-6, 0,  4), new ChunkWorldPos(-4, 0, -6), new ChunkWorldPos(-4, 0,  6),
                             new ChunkWorldPos( 4, 0, -6), new ChunkWorldPos( 4, 0,  6), new ChunkWorldPos( 6, 0, -4), new ChunkWorldPos( 6, 0,  4), new ChunkWorldPos(-7, 0, -2),
                             new ChunkWorldPos(-7, 0,  2), new ChunkWorldPos(-2, 0, -7), new ChunkWorldPos(-2, 0,  7), new ChunkWorldPos( 2, 0, -7), new ChunkWorldPos( 2, 0,  7),
                             new ChunkWorldPos( 7, 0, -2), new ChunkWorldPos( 7, 0,  2), new ChunkWorldPos(-7, 0, -3), new ChunkWorldPos(-7, 0,  3), new ChunkWorldPos(-3, 0, -7),
                             new ChunkWorldPos(-3, 0,  7), new ChunkWorldPos( 3, 0, -7), new ChunkWorldPos( 3, 0,  7), new ChunkWorldPos( 7, 0, -3), new ChunkWorldPos( 7, 0,  3),
                             new ChunkWorldPos(-6, 0, -5), new ChunkWorldPos(-6, 0,  5), new ChunkWorldPos(-5, 0, -6), new ChunkWorldPos(-5, 0,  6), new ChunkWorldPos( 5, 0, -6),
                             new ChunkWorldPos( 5, 0,  6), new ChunkWorldPos( 6, 0, -5), new ChunkWorldPos( 6, 0,  5) };

        private static int timer = 0;

        void Update()
        {
            if (DeleteChunks())
                return;
            FindChunksToLoad();
            LoadAndRenderChunks();
        }

        void LoadAndRenderChunks()
        {
            if (buildList.Count != 0)
            {
                for (int i = 0; i < buildList.Count && i < 10; i++)
                {
                    BuildChunk(buildList[0]);
                    buildList.RemoveAt(0);
                }
            }

            if (updateList.Count != 0)
            {
                Chunk chunk = world.GetChunk(updateList[0].x, updateList[0].y, updateList[0].z);
                if (chunk != null)
                    chunk.update = true;
                updateList.RemoveAt(0);
            }
        }

        void FindChunksToLoad()
        {
            ChunkWorldPos playerPos = new ChunkWorldPos(Mathf.FloorToInt(transform.position.x / Chunk.chunkSize) * Chunk.chunkSize, Mathf.FloorToInt(transform.position.y / Chunk.chunkSize) * Chunk.chunkSize, Mathf.FloorToInt(transform.position.z / Chunk.chunkSize) * Chunk.chunkSize);

            if(updateList.Count == 0)
            {
                for (int i = 0; i < chunkPositions.Length; i++)
                {
                    ChunkWorldPos newChunkPos = new ChunkWorldPos(chunkPositions[i].x * Chunk.chunkSize + playerPos.x, 0, chunkPositions[i].z * Chunk.chunkSize + playerPos.z);

                    Chunk newChunk = world.GetChunk(newChunkPos.x, newChunkPos.y, newChunkPos.z);

                    if (newChunk != null && (newChunk.rendered || updateList.Contains(newChunkPos)))
                        continue;

                    for (int y = -1; y < 2; y++)
                    {
                        for (int x = newChunkPos.x - Chunk.chunkSize; x < newChunkPos.x + Chunk.chunkSize; x += Chunk.chunkSize)
                        {
                            for (int z = newChunkPos.z - Chunk.chunkSize; z < newChunkPos.z + Chunk.chunkSize; z += Chunk.chunkSize)
                            {
                                buildList.Add(new ChunkWorldPos(x, y * Chunk.chunkSize, z));
                            }
                        }

                        updateList.Add(new ChunkWorldPos(newChunkPos.x, y * Chunk.chunkSize, newChunkPos.z));
                    }


                    return;
                }
            }
        }

        void BuildChunk(ChunkWorldPos pos)
        {
            if (world.GetChunk(pos.x, pos.y, pos.z) == null)
                world.CreateChunk(pos.x, pos.y, pos.z);
        }

        bool DeleteChunks()
        {
            if(timer == 10)
            {
                timer = 0;
                var chunksToDelete = new List<ChunkWorldPos>();

                foreach (var chunk in world.chunks)
                {
                    float distance = Vector3.Distance(chunk.Value.transform.position, transform.position);

                    if (distance > 256)
                        chunksToDelete.Add(chunk.Key);
                }

                foreach (var chunk in chunksToDelete)
                {
                    world.DestroyChunk(chunk.x, chunk.y, chunk.z);
                }

                return true;
            }
            timer++;
            return false;
        }
    }
}