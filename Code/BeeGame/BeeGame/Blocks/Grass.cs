using System;
using BeeGame.Core.Enums;
using BeeGame.Terrain.Chunks;
using BeeGame.Items;

namespace BeeGame.Blocks
{
    /// <summary>
    /// Grass Block
    /// </summary>
    [Serializable]
    public class Grass : Block
    {
        #region Data
        /// <summary>
        /// Name of the item
        /// </summary>
        private string itemName = "Grass";
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public Grass() : base() { }
        #endregion

        #region Mesh
        /// <summary>
        /// Will turn this <see cref="Block"/> into a <see cref="Dirt"/> block if another block is above it
        /// </summary>
        /// <param name="x">X pos if the block</param>
        /// <param name="y">Y pos if the block</param>
        /// <param name="z">Z pos if the block</param>
        /// <param name="chunk">Chunk that this block is in</param>
        public override void UpdateBlock(int x, int y, int z, Chunk chunk)
        {
            if (chunk.GetBlock(x, y + 1, z, false).IsSolid(Direction.DOWN))
                chunk.blocks[x, y, z] = new Dirt() { changed = changed };
        }

        /// <summary>
        /// Texture position of the <see cref="Block"/> face
        /// </summary>
        /// <param name="direction"><see cref="Direction"/> of the block face</param>
        /// <returns>Texture positon as a <see cref="Tile"/></returns>
        public override Tile TexturePosition(Direction direction)
        {
            //All textures are on the dame Y value for the texture atlas so Y can be set
            Tile tile = new Tile()
            {
                y = 9
            };

            switch (direction)
            {
                //if we want the top face return the full grass texture
                case Direction.UP:
                    tile.x = 3;
                    return tile;
                //if we want the bottom face return the dirt texture
                case Direction.DOWN:
                    tile.x = 2;
                    return tile;
                //return the 1/2 grass testure if a side face is wanted
                default:
                    tile.x = 4;
                    return tile;
            }
        }
        #endregion

        #region Overrides
        /// <summary>
        /// The Base id for the block
        /// </summary>
        /// <returns>4</returns>
        public override int GetHashCode()
        {
            return 4;
        }

        /// <summary>
        /// REturns the name and value for the block as a string
        /// </summary>
        /// <returns>A nicely formatted string</returns>
        public override string ToString()
        {
            return $"{itemName} ID: {GetItemID()}";
        }
        #endregion
    }
}
