using System;
using System.Collections.Generic;
using BeeGame.Blocks;


namespace BeeGame.Terrain.Chunks
{
    /// <summary>
    /// Saves a <see cref="Chunk"/>s modified <see cref="Block"/>s for save optimisation
    /// </summary>
    [Serializable]
    public class SaveChunk
    {
        /// <summary>
        /// <see cref="Block"/>s to be saved
        /// </summary>
        public Dictionary<ChunkWorldPos, Block> blocks = new Dictionary<ChunkWorldPos, Block>();

        /// <summary>
        /// Will search all the the given <see cref="Block"/>s for modified blocks
        /// </summary>
        /// <param name="blockArray"><see cref="Chunk"/>s blocks (Must be [16, 16, 16])</param>
        public SaveChunk(Block[,,] blockArray)
        {
            for (int x = 0; x < Chunk.chunkSize; x++)
            {
                for (int y = 0; y < Chunk.chunkSize; y++)
                {
                    for (int z = 0; z < Chunk.chunkSize; z++)
                    {
                        //*if the block has changed save it
                        if (blockArray[x, y, z].changed)
                            blocks.Add(new ChunkWorldPos(x, y, z), blockArray[x, y, z]);
                    }
                }
            }
        }
    }
}
