using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Terrain.Chunks;
using BeeGame.Terrain.Blocks;

namespace BeeGame.Terrain.LandGeneration
{
    public class World : MonoBehaviour
    {
        public Dictionary<ChunkWorldPos, Chunk> chunks = new Dictionary<ChunkWorldPos, Chunk>();

        public GameObject chunkPrefab;

        public void CreateChunk(int x, int y, int z)
        {
            ChunkWorldPos pos = new ChunkWorldPos(x, y, z);

            GameObject newChunk = Instantiate(chunkPrefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity);

            Chunk chunk = newChunk.GetComponent<Chunk>();

            chunk.chunkWorldPos = pos;
            chunk.world = this;

            chunks.Add(pos, chunk);

            chunk = new TerrainGeneration().ChunkGen(chunk);

            chunk.SetBlocksUnmodified();
            Serialization.Serialization.LoadChunk(chunk);
        }

        public void DestroyChunk(int x, int y, int z)
        {
            if (chunks.TryGetValue(new ChunkWorldPos(x, y, z), out Chunk chunk))
            {
                Serialization.Serialization.SaveChunk(chunk);
                Destroy(chunk.gameObject);
                chunks.Remove(new ChunkWorldPos(x, y, z));
            }
        }

        public Chunk GetChunk(int x, int y, int z)
        {
            float multiple = Chunk.chunkSize;
            ChunkWorldPos pos = new ChunkWorldPos()
            {
                x = Mathf.FloorToInt(x / multiple) * Chunk.chunkSize,
                y = Mathf.FloorToInt(y / multiple) * Chunk.chunkSize,
                z = Mathf.FloorToInt(z / multiple) * Chunk.chunkSize
            };
            
            chunks.TryGetValue(pos, out Chunk chunk);

            return chunk;
        }

        public Block GetBlock(int x, int y, int z)
        {
            Chunk chunk = GetChunk(x, y, z);

            if(chunk != null)
            {
                return chunk.GetBlock(x - chunk.chunkWorldPos.x, y - chunk.chunkWorldPos.y, z - chunk.chunkWorldPos.z);
            }

            return new Air();
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            Chunk chunk = GetChunk(x, y, z);

            if(chunk != null)
            {
                chunk.SetBlock(x - chunk.chunkWorldPos.x, y - chunk.chunkWorldPos.y, z - chunk.chunkWorldPos.z, block);
                chunk.update = true;

                UpdateIfEqual(x - chunk.chunkWorldPos.x, 0, new ChunkWorldPos(x - 1, y, z));
                UpdateIfEqual(x - chunk.chunkWorldPos.x, Chunk.chunkSize - 1, new ChunkWorldPos(x + 1, y, z));
                UpdateIfEqual(y - chunk.chunkWorldPos.y, 0, new ChunkWorldPos(x, y - 1, z));
                UpdateIfEqual(y - chunk.chunkWorldPos.y, Chunk.chunkSize - 1, new ChunkWorldPos(x, y + 1, z));
                UpdateIfEqual(z - chunk.chunkWorldPos.z, 0, new ChunkWorldPos(x, y, z - 1));
                UpdateIfEqual(z - chunk.chunkWorldPos.z, Chunk.chunkSize - 1, new ChunkWorldPos(x, y, z + 1));
            }
        }

        void UpdateIfEqual(int value1, int value2, ChunkWorldPos pos)
        {
            if(value1 == value2)
            {
                Chunk chunk = GetChunk(pos.x, pos.y, pos.z);

                if (chunk != null)
                    chunk.update = true;
            }
        }
    }
}
