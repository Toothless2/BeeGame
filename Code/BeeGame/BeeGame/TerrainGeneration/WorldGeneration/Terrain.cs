using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BeeGame.TerrainGeneration
{
    public static class Terrain
    {
        public static THVector3 GetBlockPos(Vector3 pos)
        {
            THVector3 blockPos = new THVector3()
            {
                x = (int)Math.Floor(pos.x),
                y = (int)Math.Floor(pos.y),
                z = (int)Math.Floor(pos.z)
            };

            return blockPos;
        }

        /// <summary>
        /// Resurns the positon of the block hit
        /// </summary>
        /// <param name="hit"><see cref="RaycastHit"/></param>
        /// <param name="adjacent">Do you want the face adjecent to the block hit</param>
        /// <returns><see cref="THVector3"/> of the block you hit in world cordinates</returns>
        public static THVector3 GetBlockPos(RaycastHit hit, bool adjacent = false)
        {
            return new THVector3()
            {
                x = Round(hit.point.x, hit.normal.x, adjacent),
                y = Round(hit.point.y, hit.normal.y, adjacent),
                z = Round(hit.point.z, hit.normal.z, adjacent)
            };
        }

        static float Round(float pos, float norm, bool adjacent = false)
        {
            if(pos - (int)pos == 0.5f || pos - (int)pos == -0.5f)
            {
                if (adjacent)
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

        public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
        {
            Chunk chunk = hit.collider.GetComponent<Chunk>();
            block.changed = true;

            if(chunk == null)
                return true;

            THVector3 worldPos = GetBlockPos(hit, adjacent);

            chunk.world.SetBlock((int)worldPos.x, (int)worldPos.y, (int)worldPos.z, block);

            Serialization.Serialization.SaveChunk(chunk, worldPos - chunk.worldPos, block);

            return true;
        }

        public static Block GetBlock(RaycastHit hit, bool adjacent = false)
        {
            Chunk chunk = hit.collider.GetComponent<Chunk>();

            if (chunk == null)
                return null;

            THVector3 worldPos = GetBlockPos(hit, adjacent);

            return chunk.world.GetBlock((int)worldPos.x, (int)worldPos.y, (int)worldPos.z);
        }
    }
}
