using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BeeGame.TerrainGeneration
{
    [Serializable]
    public class ChunkSave
    {
        public Dictionary<THVector3, Block> blocks = new Dictionary<THVector3, Block>();

        public ChunkSave(Chunk chunk, THVector3 blockPos, Block block)
        {
            for (int x = 0; x < Chunk.chunkSize; x++)
            {
                for (int y = 0; y < Chunk.chunkSize; y++)
                {
                    for (int z = 0; z < Chunk.chunkSize; z++)
                    {
                        if(chunk.blocks[x, y, z].changed)
                        {
                            blocks.Add(new THVector3(x, y, z), block);
                        }
                    }
                }
            }
        }
    }
}
