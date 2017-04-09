using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Terrain.Chunks;
using BeeGame.Blocks;

namespace BeeGame.Terrain.LandGeneration
{
    /// <summary>
    /// Should use as an interface between the rest of the game and the terrain
    /// </summary>
    public class Terrain
    {
        #region Setting Position To block Grid
        // TODO: Convert to THVector3
        /// <summary>
        /// Gets a block postion from a <see cref="Vector3"/>
        /// </summary>
        /// <param name="pos">Position of the block as a <see cref="Vector3"/></param>
        /// <returns><see cref="ChunkWorldPos"/> of the <see cref="Block"/></returns>
        /// <remarks>
        /// Convert to <see cref="THVector3"/> when reimplemented
        /// </remarks>
        public static ChunkWorldPos GetBlockPos(Vector3 pos)
        {
            return new ChunkWorldPos()
            {
                x = Mathf.RoundToInt(pos.x),
                y = Mathf.RoundToInt(pos.y),
                z = Mathf.RoundToInt(pos.z)
            };
        }

        // TODO: Convert to THVector3 or possibly remove and use below function instead
        /// <summary>
        /// Returns the positon of the block hit as a <see cref="Vector3"/>
        /// </summary>
        /// <param name="hit"><see cref="RaycastHit"/></param>
        /// <param name="adjacent">Do you want the face adjecent to the block hit</param>
        /// <returns><see cref="THVector3"/> of the block you hit in world cordinates</returns>
        /// <remarks>
        /// When <see cref="THVector3"/> is reimplemented change to that
        /// </remarks>
        public static Vector3 GetBlockPos(RaycastHit hit)
        {
            Vector3 vec3 = new Vector3()
            {
                x = RoundXZ(hit.point.x, hit.normal.x),
                y = RoundY(hit.point.y, hit.normal.y),
                z = RoundXZ(hit.point.z, hit.normal.z)
            };
#if DEBUG
            //MonoBehaviour.print($"{hit.point.y} : {hit.normal.y} : {vec3.y} : {hit.point.y >= 0} : {Math.Round(hit.point.y, 1)} : {Mathf.RoundToInt(((float)Math.Round(hit.point.y, 1)))}");
#endif

            return (vec3);
        }

        /// <summary>
        /// <see cref="GetBlockPos(Vector3)"/> does the same thing but returns a <see cref="ChunkWorldPos"/>
        /// </summary>
        /// <param name="hit"></param>
        /// <returns></returns>
        public static ChunkWorldPos GetBlockPosFromRayCast(RaycastHit hit)
        {
            return new ChunkWorldPos((int)RoundXZ(hit.point.x, hit.normal.x), (int)RoundY(hit.point.y, hit.normal.y), (int)RoundXZ(hit.point.z, hit.normal.z));
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
                if (normal < 0)
                {
                    pos = (int)pos;
                    return pos;
                }
                //if their is no normal
                
                //if pos is greater than 0.5 we are in the next block so go to it
                if ((-pos - (int)-pos) > 0.5)
                {
                    return (int)pos - 1;
                }

                return (int)pos;
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
            pos = (float)Math.Round(pos, 1);
            if (pos >= 0)
            {
                if(normal > 0)
                {
                    if((int)pos % 2 == 0)
                        return Mathf.RoundToInt((float)Math.Round(pos, 1));

                    return Mathf.RoundToInt((float)Math.Round(pos, 1)) - normal;
                }

                if((int)pos % 2 == 0)
                    return Mathf.RoundToInt((float)Math.Round(pos, 1)) - normal;

                return Mathf.RoundToInt((float)Math.Round(pos, 1));
            }

            //if the normal is above 0 subtract it from the pos otherwise add it
            if (normal > 0 && (int)pos % 2 == 0)
                //the Math.Round removes strange rounding errors shown with Mathf.Round eg sometimes 0.5 would round to 0 not 1
                return Mathf.RoundToInt((float)Math.Round(pos, 1)) - normal;

            if ((int)pos % 2 == 0)
                return Mathf.RoundToInt((float)Math.Round(pos, 1)) + normal;

            return Mathf.RoundToInt((float)Math.Round(pos, 1));
        }

        /// <summary>
        /// Rounds the given pos to the correct position
        /// </summary>
        /// <param name="pos">Position that needs to be rounded</param>
        /// <param name="norm">Normal for the face</param>
        /// <param name="adjacent">Should the adjacent block be recived</param>
        /// <returns>rounded value of <paramref name="pos"/> as a <see cref="float"/></returns>
        /// <remarks>
        /// Check how this performs. Possibly change all uses of this to <see cref="RoundXZ(float, float)"/> and <see cref="RoundY(float, float)"/>
        /// </remarks>
        public static float Round(float pos, float norm, bool adjacent = false)
        {
            if(pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
            {
                if(adjacent)
                {
                    pos += (norm / 2);
                }
                else
                {
                    pos -= (norm / 2);
                }
            }

            return pos;
        }
        #endregion

        #region Get Block
        /// <summary>
        /// Gets a <see cref="Chunk"/>s world positon
        /// </summary>
        /// <param name="hit">Where the raycast hit</param>
        /// <param name="adjacent">Should the adjacent <see cref="Chunk"/> position be returned?</param>
        /// <returns><see cref="ChunkWorldPos"/> of the <see cref="Chunk"/><returns>
        public static ChunkWorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
        {
            return GetBlockPos(new Vector3()
            {
                //rounds the hit to the correct position
                x = Round(hit.point.x, hit.normal.x, adjacent),
                y = Round(hit.point.y, hit.normal.y, adjacent),
                z = Round(hit.point.z, hit.normal.z, adjacent)
            });
        }

        /// <summary>
        /// Get a <see cref="Block"/> at the given position
        /// </summary>
        /// <param name="hit">Where to get the block from</param>
        /// <param name="adjacent">Should the adjacent <see cref="Block"/> be returned</param>
        /// <returns><see cref="Block"/> at <paramref name="hit.point"/>, Null if no block was found</returns>
        public static Block GetBlock(RaycastHit hit, bool adjacent = false)
        {
            //checks that a chunk was hit and if it wasnt return early
            Chunk chunk = hit.collider.GetComponent<Chunk>();

            if (chunk == null)
                return null;

            //allignes the hit to the block grid and returns the block
            ChunkWorldPos pos = GetBlockPos(hit, adjacent);

            return chunk.world.GetBlock(pos.x, pos.y, pos.z);
        }

        public static bool BlockInPosition(Vector3 pos, Chunk chunk)
        {
            if (chunk == null)
                return false;

            if (chunk.GetBlock((int)pos.x, (int)pos.y, (int)pos.z) != new Air())
                return true;

            return false;
        }
        #endregion

        #region Set Block
        /// <summary>
        /// Sets the <see cref="Block"/> at the given point the given <see cref="Block"/>
        /// </summary>
        /// <param name="hit">Where the block should be set</param>
        /// <param name="block"><see cref="Block"/> to be set</param>
        /// <param name="adjacent">Should the adjacent <see cref="Block"/> be set</param>
        /// <returns><see cref="true"/> if block was set</returns>
        public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
        {
            //checks that a chnk was hit
            Chunk chunk = hit.collider.GetComponent<Chunk>();

            if (chunk == null)
                return false;

            //alligns the hit to the block grid
            ChunkWorldPos pos = GetBlockPosFromRayCast(hit);

            //checks that the block tryign to be replaced can be replaced eg bedrock cannot be replaced
            if (GetBlock(hit, adjacent).breakable)
            {
                //sets the position of the block and saves the chunk
                chunk.world.SetBlock(pos.x, pos.y, pos.z, block);
                Serialization.Serialization.SaveChunk(chunk);
            }

            return true;
        }
        #endregion
    }
}