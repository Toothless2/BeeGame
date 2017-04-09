using UnityEngine;
using BeeGame.Terrain.Chunks;
using BeeGame.Terrain.LandGeneration.Noise;
using BeeGame.Serialization;
using System.Collections.Generic;

namespace BeeGame.Terrain.LandGeneration
{
    /// <summary>
    /// Generates the terrain for the game
    /// </summary>
    public class TerrainGeneration
    {
        #region Data
        /// <summary>
        /// Base height of stone
        /// </summary>
        private float stoneBaseHeight = -24;
        /// <summary>
        /// Base noise of stone
        /// </summary>
        private float stoneBaseNoise = 0.05f;
        /// <summary>
        /// Base noise heigh for stone
        /// </summary>
        private float stoneBaseNoiseHeight = 4;

        /// <summary>
        /// Base height for a mountain
        /// </summary>
        private float stoneMountainHeight = 48;
        /// <summary>
        /// Frequency of mountains (larger value = more choppy terrain)
        /// </summary>
        private float stoneMountainFrequency = 0.008f;
        /// <summary>
        /// Minimun height for stone
        /// </summary>
        private float stoneMinHeight = -12;

        /// <summary>
        /// Where does dirt start
        /// </summary>
        private float dirtBaseHeight = 1;
        /// <summary>
        /// How much of the surface is dirt
        /// </summary>
        private float dirtNoise = 0.04f;
        /// <summary>
        /// How tall dirt can be
        /// </summary>
        private float dirtNoiseHeight = 3;

        /// <summary>
        /// How often do caves happen
        /// </summary>
        private float caveFrequency = 0.025f;
        /// <summary>
        /// Threashold for makeing a cave
        /// </summary>
        private int caveSize = 8;
        #endregion

        /// <summary>
        /// Generates a <see cref="Chunk"/> in a new thread
        /// </summary>
        /// <param name="chunk"><see cref="Chunk"/> to populate with <see cref="Block"/>s</param>
        /// <returns><see cref="Chunk"/> with <see cref="Block"/>s generated</returns>
        public Chunk ChunkGen(Chunk chunk)
        {
            ChunkGenThread(chunk, out chunk);

            return chunk;
        }

        /// <summary>
        /// Generates a new <see cref="Chunk"/>
        /// </summary>
        /// <param name="chunk"><see cref="Chunk"/> to be generated</param>
        /// <param name="outChunk">Generated <see cref="Chunk"/> to return</param>
        public void ChunkGenThread(Chunk chunk, out Chunk outChunk)
        {
            //for each x and z position in teh chunk
            for (int x = chunk.chunkWorldPos.x; x < chunk.chunkWorldPos.x + Chunk.chunkSize; x++)
            {
                for (int z = chunk.chunkWorldPos.z; z < chunk.chunkWorldPos.z + Chunk.chunkSize; z++)
                {
                    chunk = GenChunkColum(chunk, x, z);
                }
            }

            outChunk = chunk;
        }

        /// <summary>
        /// Generates a colum of the <see cref="Chunk"/>
        /// </summary>
        /// <param name="chunk"><see cref="Chunk"/> to generate a colum for</param>
        /// <param name="x">X pos to make the colum</param>
        /// <param name="z">Z pos to make the colum</param>
        /// <returns><see cref="Chunk"/> with a new colum ob blocks generated</returns>
        public Chunk GenChunkColum(Chunk chunk, int x, int z)
        {
            //the height of the mountain
            int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
            stoneHeight += GetNoise(-x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

            //if the colum is currenly to low make it not so low
            if (stoneHeight < stoneMinHeight)
                stoneHeight = Mathf.FloorToInt(stoneMinHeight);

            //add the height of normal stone on to the mountain
            stoneHeight += GetNoise(x, 0, -z, stoneBaseNoise, Mathf.RoundToInt(stoneBaseNoiseHeight));

            //put dirt on top
            int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
            dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

            //set the colum to the correct blocks
            for (int y = chunk.chunkWorldPos.y; y < chunk.chunkWorldPos.y + Chunk.chunkSize; y ++)
            {
                int caveChance = GetNoise(x + 40, y + 100, z - 50, caveFrequency, 200);

                //puts a layer of bedrock at the botton the the world
                if (y <= (chunk.chunkWorldPos.y) && chunk.chunkWorldPos.y == -16)
                {
                    SetBlock(x, y, z, new Blocks.Bedrock(), chunk);
                }
                else if (y <= stoneHeight && caveSize < caveChance)
                {
                    SetBlock(x, y, z, new Blocks.Block(), chunk);
                }
                else if (y <= dirtHeight && caveSize < caveChance)
                {
                    SetBlock(x, y, z, new Blocks.Grass(), chunk);
                }
                else
                {
                    SetBlock(x, y, z, new Blocks.Air(), chunk);
                }
            }

                return chunk;
        }

        public static void SaveNoise(int x, int y, int z)
        {

        }

        /// <summary>
        /// Get a noise value
        /// </summary>
        /// <param name="x">X pos of the noise</param>
        /// <param name="y">Y pos of the noise</param>
        /// <param name="z">Z pos of the noise</param>
        /// <param name="scale">What the step shout bee from the last x, y, z</param>
        /// <param name="max">Max value of the noise</param>
        /// <returns>A noise value as an int</returns>
        public static int GetNoise(int x, int y, int z, float scale, int max)
        {
            return Mathf.FloorToInt((SimplexNoise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
        }

        /// <summary>
        /// Sets a <see cref="Block"/> in the position
        /// </summary>
        /// <param name="x">X pos of the block</param>
        /// <param name="y">Y pos of the block</param>
        /// <param name="z">Z pos of the block</param>
        /// <param name="block"><see cref="Block"/> to set</param>
        /// <param name="chunk"><see cref="Chunk"/> to set the block in</param>
        /// <param name="replacesBlocks">Can it replace blocks</param>
        public static void SetBlock(int x, int y, int z, Blocks.Block block, Chunk chunk, bool replacesBlocks = false)
        {
            //corrects the x, y, z pos of the so that the block is placed in the correct position
            x -= chunk.chunkWorldPos.x;
            y -= chunk.chunkWorldPos.y;
            z -= chunk.chunkWorldPos.z;

            //chechs that the block is in the chunk and that no block is already their then sets it
            if (Chunk.InRange(x) && Chunk.InRange(y) && Chunk.InRange(z))
                if (replacesBlocks || chunk.blocks[x, y, z] == null)
                    chunk.SetBlock(x, y, z, block);
        }
    }
}
