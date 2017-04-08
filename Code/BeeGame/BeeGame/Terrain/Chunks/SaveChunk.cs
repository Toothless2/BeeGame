using System;
using System.Collections.Generic;
using BeeGame.Terrain.Blocks;


namespace BeeGame.Terrain.Chunks
{
    [Serializable]
    public class SaveChunk
    {
        public Dictionary<ChunkWorldPos, Block> blocks = new Dictionary<ChunkWorldPos, Block>();
        
        public SaveChunk(Block[,,] blockArray)
        {
            unchecked
            {
                for (int x = 0; x < Chunk.chunkSize; x++)
                {
                    for (int y = 0; y < Chunk.chunkSize; y++)
                    {
                        for (int z = 0; z < Chunk.chunkSize; z++)
                        {
                            if (blockArray[x, y, z].changed)
                                blocks.Add(new ChunkWorldPos(x, y, z), blockArray[x, y, z]);
                        }
                    }
                }
            }
        }
    }
}
