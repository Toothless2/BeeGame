using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeeGame.TerrainGeneration
{
    public class World : MonoBehaviour
    {
        public Dictionary<THVector3, Chunk> chunks = new Dictionary<THVector3, Chunk>();

        public GameObject chunkPrefab;

        public void CreateChunk(int x, int y, int z)
        {
            //coords of the chunk on the world
            THVector3 worldPos = new THVector3(x, y, z);
            
            //makes the chunk in the world
            GameObject newChunkGameObject = Instantiate(chunkPrefab, worldPos, Quaternion.identity);

            //gets teh Chunk component so that we an do things to it
            Chunk newChunk = newChunkGameObject.GetComponent<Chunk>();

            //Seta the chunks positon and world
            newChunk.worldPos = worldPos;
            newChunk.world = this;

            //Adds the new chunk to the dictionary
            chunks.Add(worldPos, newChunk);


            var terrainGen = new TerrainGenerator();
            newChunk = terrainGen.ChunkGen(newChunk);

            newChunk.SetBlocksUnmodified();
            Serialization.Serialization.LoadChunk(newChunk);
        }

        public void DestroyChunk(int x, int y, int z)
        {
            Chunk chunk = GetChunk(x, y, z);

            if(chunk != null)
            {
                Serialization.Serialization.SaveChunk(chunk);
                Destroy(chunk.gameObject);
                chunks.Remove(chunk.worldPos);
            }
        }

        public Chunk GetChunk(int x, int y, int z)
        {
            //coords of the chunk in the world
            THVector3 vec3 = new THVector3(x, y, z);

            float multiple = Chunk.chunkSize;

            // Divides given corrd as a float then tiems by the chunk size so that the world corrd is set to the correct chunk
            // eg world coord is (16, 1, 0) the chunk coord would be (16, 0, 0)    (dividing float by 0 is zero)
            vec3.x = (int)Math.Floor(x / multiple) * multiple;
            vec3.y = (int)Math.Floor(y / multiple) * multiple;
            vec3.z = (int)Math.Floor(z / multiple) * multiple;
            chunks.TryGetValue(vec3, out Chunk containter);

            return containter;
        }

        public void SetBlock(int x, int y, int z, Block block)
        {
            Chunk chunk = GetChunk(x, y, z);

            if(chunk != null)
            {
                chunk.SetBlock(x - (int)chunk.worldPos.x, y - (int)chunk.worldPos.y, z - (int)chunk.worldPos.z, block);
                chunk.update = true;

                ////updates nebouring chunks if block dwstroyed was on the edge of a chunk
                //UpdateIfEqual(x - (int)chunk.worldPos.x, 0, new THVector3(x - 1, y, z));
                //UpdateIfEqual(x - (int)chunk.worldPos.x, Chunk.chunkSize - 1, new THVector3(x + 1, y, z));
                //UpdateIfEqual(y - (int)chunk.worldPos.y, 0, new THVector3(x, y - 1, z));
                //UpdateIfEqual(y - (int)chunk.worldPos.y, Chunk.chunkSize - 1, new THVector3(x, y + 1, z));
                //UpdateIfEqual(z - (int)chunk.worldPos.z, 0, new THVector3(x, y, z - 1));
                //UpdateIfEqual(z - (int)chunk.worldPos.z, Chunk.chunkSize - 1, new THVector3(x, y, z + 1));
            }
        }

        public Block GetBlock(int x, int y, int z, bool updateChunk = false)
        {
            // Gets the correct chunk the block is in
            Chunk chunk = GetChunk(x, y, z);

            if(chunk != null)
            {
                chunk.update = updateChunk;
                // gien corrd - the word coord as the given value wil be the vorld value and not the value of the chunk
                return chunk.GetBlock(x - (int)chunk.worldPos.x, y - (int)chunk.worldPos.y, z - (int)chunk.worldPos.z);
            }
            else
            {
                return new Blocks.Air();
            }
        }

        void UpdateIfEqual(int value1, int value2, THVector3 worldPos)
        {
            if(value1 == value2)
            {
                Chunk chunk = GetChunk((int)worldPos.x, (int)worldPos.y, (int)worldPos.z);

                if (chunk != null)
                    chunk.update = true;
            }
        }
    }
}
