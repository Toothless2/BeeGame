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
            //Makes every colum of the chunk
            for (int x = (int)chunk.worldPos.x; x < chunk.worldPos.x + Chunk.chunkSize; x++)
            {
                for (int z = (int)chunk.worldPos.z; z < chunk.worldPos.z + Chunk.chunkSize; z++)
                {
                    chunk = ChunkColumGen(chunk, x, z);
                }
            }

            return chunk;
       }

        /// <summary>
        /// Generates a columb of the given <see cref="Chunk"/>
        /// </summary>
        /// <param name="chunk"><see cref="Chunk"/> to generate a columb for</param>
        /// <param name="x">X world pos of the columb</param>
        /// <param name="z">Y world pos of the columb</param>
        /// <returns><see cref="Chunk"/> with 1 new colum added</returns>
        public Chunk ChunkColumGen(Chunk chunk, int x, int z)
        {
            //is this columb a mountain?
            int stoneHeight = Mathf.FloorToInt(stoneBaseHeight) + GetNoise(x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

            //if the height was to low make it the same hieght as the lowest possible stone block
            if (stoneHeight < stoneMinHeight)
                stoneHeight = Mathf.FloorToInt(stoneMinHeight);
            
            //make the rest of the stone useing noise
            stoneHeight += GetNoise(x, 0, z, stoneBaseNoise, Mathf.FloorToInt(stoneBaseNoiseHeight));

            //add a layer is dirt on top not useing noise
            int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);// + GetNoise(x, 0, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

            //make the colum
            for (int y = (int)chunk.worldPos.y; y < chunk.worldPos.y + Chunk.chunkSize; y++)
            {
                //if the current y is less than the stone heigt make it stone
                if(y <= stoneHeight)
                {
                    chunk.SetBlock(x - (int)chunk.worldPos.x, y - (int)chunk.worldPos.y, z - (int)chunk.worldPos.z, new Block());
                }
                //if we are out if the stone layer and we need to put dirt on top do it
                else if(y <= dirtHeight)
                {
                    chunk.SetBlock(x - (int)chunk.worldPos.x, y - (int)chunk.worldPos.y, z - (int)chunk.worldPos.z, new Blocks.Grass());
                }
                //if everthing else is done make the rest of the chunk out of air
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
        /// <remarks>
        /// Could look into makeing my own noise generator but for now this will do
        /// </remarks>
        public static int GetNoise(int x, int y, int z, float scale, int max)
        {
            return (int)Math.Floor((SimplexNoise.Generate(x * scale, y * scale, z * scale) * (max / 2)));
        }
    }
}
