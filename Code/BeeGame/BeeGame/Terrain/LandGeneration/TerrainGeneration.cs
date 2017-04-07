using UnityEngine;
using System.Collections;
using BeeGame.Terrain.Chunks;
using BeeGame.Terrain.LandGeneration.Noise;

namespace BeeGame.Terrain.LandGeneration
{
    public class TerrainGeneration
    {
        private float stoneBaseHeight = -24;
        private float stoneBaseNoise = 0.05f;
        private float stoneBaseNoiseHeight = 4;

        private float stoneMountainHeight = 48;
        private float stoneMountainFrequency = 0.008f;
        private float stoneMinHeight = -12;

        private float dirtBaseHeight = 1;
        private float dirtNoise = 0.04f;
        private float dirtNoiseHeight = 3;

        private float caveFrequency = 0.025f;
        private int caveSize = 8;

        public Chunk ChunkGen(Chunk chunk)
        {
            for (int x = chunk.chunkWorldPos.x; x < chunk.chunkWorldPos.x + Chunk.chunkSize; x++)
            {
                for (int z = chunk.chunkWorldPos.z; z < chunk.chunkWorldPos.z + Chunk.chunkSize; z++)
                {
                    chunk = GenChunkColum(chunk, x, z);
                }
            }
            
            return chunk;
        }

        public Chunk GenChunkColum(Chunk chunk, int x, int z)
        {
            int stoneHeight = Mathf.FloorToInt(stoneBaseHeight);
            stoneHeight += GetNoise(-x, 0, z, stoneMountainFrequency, Mathf.FloorToInt(stoneMountainHeight));

            if (stoneHeight < stoneMinHeight)
                stoneHeight = Mathf.FloorToInt(stoneMinHeight);

            stoneHeight += GetNoise(x, 0, -z, stoneBaseNoise, Mathf.RoundToInt(stoneBaseNoiseHeight));

            int dirtHeight = stoneHeight + Mathf.FloorToInt(dirtBaseHeight);
            dirtHeight += GetNoise(x, 100, z, dirtNoise, Mathf.FloorToInt(dirtNoiseHeight));

            for (int y = chunk.chunkWorldPos.y; y < chunk.chunkWorldPos.y + Chunk.chunkSize; y ++)
            {
                int caveChance = GetNoise(x + 40, y + 100, z - 50, caveFrequency, 200);

                //puts a layer of bedrock at the botton the the world
                if (y <= (chunk.chunkWorldPos.y) && chunk.chunkWorldPos.y == -32)
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

        public static int GetNoise(int x, int y, int z, float scale, int max)
        {
            return Mathf.FloorToInt((SimplexNoise.Generate(x * scale, y * scale, z * scale) + 1f) * (max / 2f));
        }

        public static void SetBlock(int x, int y, int z, Blocks.Block block, Chunk chunk, bool replacesBlocks = false)
        {
            x -= chunk.chunkWorldPos.x;
            y -= chunk.chunkWorldPos.y;
            z -= chunk.chunkWorldPos.z;

            if (Chunk.InRange(x) && Chunk.InRange(y) && Chunk.InRange(z))
                if (replacesBlocks || chunk.blocks[x, y, z] == null)
                    chunk.SetBlock(x, y, z, block);
        }
    }
}
