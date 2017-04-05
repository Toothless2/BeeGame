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
            unchecked
            {
                //Looks at every block in the chunk and if the block has been modified add it to the dictionary
                for (int x = 0; x < Chunk.chunkSize; x++)
                {
                    for (int y = 0; y < Chunk.chunkSize; y++)
                    {
                        for (int z = 0; z < Chunk.chunkSize; z++)
                        {
                            if (!chunk.blocks[x, y, z].changed)
                                return;

                            THVector3 pos = new THVector3(x, y, z);
                            blocks.Add(pos, chunk.blocks[x, y, z]);
                        }
                    }
                }
            }
        }
    }
}
