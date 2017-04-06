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

        public ChunkSave(Chunk chunk)
        {
            //searches through all blocks in the chunk and if the block has changed add it to the saved blocks
            for (int x = 0; x < Chunk.chunkSize; x++)
            {
                for (int y = 0; y < Chunk.chunkSize; y++)
                {
                    for (int z = 0; z < Chunk.chunkSize; z++)
                    {
                        if(chunk.blocks[x, y, z].changed)
                        {
                            blocks.Add(new THVector3(x, y, z), chunk.blocks[x, y, z]);
                        }
                    }
                }
            }
        }
    }
}
