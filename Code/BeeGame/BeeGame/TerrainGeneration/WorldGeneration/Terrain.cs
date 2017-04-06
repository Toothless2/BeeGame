using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeeGame.TerrainGeneration
{
    /// <summary>
    /// Used as an interface between the player and terrain
    /// </summary>
    public static class Terrain
    {
        [Obsolete("Use GetBlockPos(RaycastHit hit) instead")]
        public static THVector3 GetBlockPos(Vector3 pos)
        {
            THVector3 blockPos = new THVector3()
            {
                x = Mathf.RoundToInt(pos.x),
                y = Mathf.RoundToInt(pos.y),
                z = Mathf.RoundToInt(pos.z)
            };

            return blockPos;
        }

        /// <summary>
        /// Returns the positon of the block hit
        /// </summary>
        /// <param name="hit"><see cref="RaycastHit"/></param>
        /// <param name="adjacent">Do you want the face adjecent to the block hit</param>
        /// <returns><see cref="THVector3"/> of the block you hit in world cordinates</returns>
        public static THVector3 GetBlockPos(RaycastHit hit)
        {
            THVector3 vec3 = new THVector3()
            {
                x = RoundXZ(hit.point.x, hit.normal.x),
                y = RoundY(hit.point.y, hit.normal.y),
                z = RoundXZ(hit.point.z, hit.normal.z)
            };

            return (vec3);
        }

        /// <summary>
        /// Used to round the X/Z values when getting a block
        /// </summary>
        /// <param name="pos">X/Y pos</param>
        /// <param name="normal">X/Y normal</param>
        /// <returns>rounded <paramref name="pos"/></returns>
        /// <remarks>
        /// Do I realy need to do all this?
        /// </remarks>
        static float RoundXZ(float pos, float normal)
        {
            //if we are looking at + x/z vlaues
            if (pos > 0)
            {
                if (normal > 0)
                {
                    pos = (int)pos;
                    return pos;
                }
                else if (normal < 0)
                {
                    pos = (int)pos;
                    return pos - -1;
                }
                else
                {
                    if ((pos - (int)pos) > 0.5)
                    {
                        return (int)pos + 1;
                    }
                    return (int)pos;
                }
            }
            //if we are looking at - x/z values
            else
            {
                //if poitive normal
                if (normal > 0)
                {
                    pos = (int)pos;
                    return pos - 1;
                }
                //if negative nomrmal
                else if(normal < 0)
                {
                    pos = (int)pos;
                    return pos;
                }
                //if their is no normal
                else
                {
                    //if pos is greater than 0.5 we are in the next block so go to it
                    if ((-pos - (int)-pos) > 0.5)
                    {
                        return (int)pos - 1;
                    }
                    return (int)pos;
                }
            }
        }

        /// <summary>
        /// Round the Y value of the given coord
        /// </summary>
        /// <param name="pos">Y pos</param>
        /// <param name="normal">Y normal</param>
        /// <returns><paramref name="pos"/> rounded to 1 DP</returns>
        /// <remarks>
        /// Do I have to do this? or is their an easier way to do this
        /// </remarks>
        static float RoundY(float pos, float normal)
        {
            //if this y is divisable by 2 go in
            if((int)pos % 2 == 0)
            {
                //if the normal is above 0 subtract it from the pos otherwise add it
                if (normal > 0)
                {
                    return Mathf.RoundToInt(pos) - normal;
                }
                else
                {
                    return Mathf.RoundToInt(pos);
                }
            }

            return Mathf.RoundToInt(pos);
        }

        /// <summary>
        /// Sets a block in the chunk to the given <paramref name="block"/> at the given <see cref="RaycastHit.point"/>
        /// </summary>
        /// <param name="hit">Where the raycast hit</param>
        /// <param name="block">The block to set</param>
        /// <param name="adjacent">shoudl the adjacent block be changed</param>
        /// <returns></returns>
        public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
        {
            //gets the chunk
            Chunk chunk = hit.collider.GetComponent<Chunk>();

            //if their was no chunk their do nothign adn return
            if(chunk == null)
                return true;

            block.changed = true;
            //Get the block positon
            THVector3 worldPos = GetBlockPos(hit);
            //set the block at that position
            chunk.world.SetBlock((int)worldPos.x, (int)worldPos.y, (int)worldPos.z, block);
            //save teh changes to the chunk
            Serialization.Serialization.SaveChunk(chunk);

            return true;
        }

        /// <summary>
        /// Gest the <see cref="Block"/> at the given <see cref="RaycastHit.point"/>
        /// </summary>
        /// <param name="hit">Where the raycast hit</param>
        /// <param name="adjacent">should the dajacent block be returned</param>
        /// <returns><see cref="Block"/></returns>
        public static Block GetBlock(RaycastHit hit, bool adjacent = false)
        {
            //gets the chunk
            Chunk chunk = hit.collider.GetComponent<Chunk>();

            if (chunk == null)
                return null;

            //converts the world position to a chunk block positon
            THVector3 worldPos = GetBlockPos(hit);

            //returns the block at the position
            return chunk.world.GetBlock((int)worldPos.x, (int)worldPos.y, (int)worldPos.z);
        }
    }
}
