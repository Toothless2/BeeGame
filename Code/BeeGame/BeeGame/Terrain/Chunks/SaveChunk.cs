using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BeeGame.Terrain.Chunks
{
    [Serializable]
    public class SaveChunk
    {
        public Dictionary<ChunkWorldPos, Blocks.Block> blocks = new Dictionary<ChunkWorldPos, Blocks.Block>();
        
        public SaveChunk(Chunk chunk)
        {
            unchecked
            {
                for (int x = 0; x < Chunk.chunkSize; x++)
                {
                    for (int y = 0; y < Chunk.chunkSize; y++)
                    {
                        for (int z = 0; z < Chunk.chunkSize; z++)
                        {
                            if (chunk.blocks[x, y, z].changed)
                                blocks.Add(new ChunkWorldPos(x, y, z), chunk.blocks[x, y, z]);
                        }
                    }
                }
            }
        }
    }
}
