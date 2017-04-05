using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BeeGame.TerrainGeneration.Noise;
using UnityEngine;

namespace BeeGame.TerrainGeneration
{
    public class TerrainGenerator
    {
        /// <summary>
        /// where stone generation will start
        /// </summary>
        float stoneBaseHeight = -24;
        /// <summary>
        /// Stones base noise value will be added to the scale
        /// </summary>
        float stoneBaseNoise = 0.05f;
        /// <summary>
        /// Max difference between a peak and a valley
        /// </summary>
        float stoneBaseNoiseHeight = 4;
        
        /// <summary>
        /// Mas differnece between a peak and a valley
        /// </summary>
        float stoneMountainHeight = 48;
        /// <summary>
        /// How often this feature will appear
        /// </summary>
        float stoneMountainFrequency = 0.008f;
        /// <summary>
        /// Lowest Stone is allowed to go
        /// </summary>
        float stoneMinHeight = -12;

        //Adding a layer of dirt on the top
        float dirtBaseHeight = 1;
        float dirtNoise = 0.3f;
        float dirtNoiseHeight = 3;

        public Chunk ChunkGen(Chunk chunk)
        {
            for (int x = (int)chunk.worldPos.x; x < chunk.worldPos.x + Chunk.chunkSize; x++)
            {
                for (int z = (int)chunk.worldPos.z; z < chunk.worldPos.z + Chunk.chunkSize; z++)
                {
                    chunk = ChunkColumGen(chunk, x, z);
                }
            }

            return chunk;
       }

        public Chunk ChunkColumGen(Chunk chunk, int x, int z)
        {
            int stoneHeight = Mathf.FloorToInt(stoneBaseHeight) + GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

            if (stoneHeight < stoneMinHeight)
                stoneHeight = Mathf.FloorToInt(stoneMinHeight);

            stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

            int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);// + GetNoise(x, 0, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

            for (int y = (int)chunk.worldPos.y; y < chunk.worldPos.y + Chunk.chunkSize; y++)
            {
                if(y <= stoneHeight)
                {
                    chunk.SetBlock(x - (int)chunk.worldPos.x, y - (int)chunk.worldPos.y, z - (int)chunk.worldPos.z, new Block());
                }
                else if(y <= dirtHeight)
                {
                    chunk.SetBlock(x - (int)chunk.worldPos.x, y - (int)chunk.worldPos.y, z - (int)chunk.worldPos.z, new Blocks.Grass());
                }
                else
                {
                    chunk.SetBlock(x - (int)chunk.worldPos.x, y - (int)chunk.worldPos.y, z - (int)chunk.worldPos.z, new Blocks.Air());
                }
            }

            return chunk;
        }

        /// <summary>
        /// Returns a Noise Value for the gien x, y, z
        /// </summary>
        /// <param name="x">x to sample</param>
        /// <param name="y">y to sample</param>
        /// <param name="z">z to sample</param>
        /// <param name="scale">Smaller the value the smoother the retuned value</param>
        /// <param name="max">Max size of the returned value</param>
        /// <returns>0-<paramref name="max"/></returns>
        public static int GetNoise(int x, int y, int z, float scale, int max)
        {
            return (int)Math.Floor((SimplexNoise.Generate(x * scale, y * scale, z * scale) * (max / 2)));
        }
    }
}
