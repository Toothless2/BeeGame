using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Terrain.Chunks;
using BeeGame.Terrain.Blocks;

namespace BeeGame.Terrain.LandGeneration
{
    public class Terrain
    {
        public static ChunkWorldPos GetBlockPos(Vector3 pos)
        {
            return new ChunkWorldPos()
            {
                x = Mathf.RoundToInt(pos.x),
                y = Mathf.RoundToInt(pos.y),
                z = Mathf.RoundToInt(pos.z)
            };
        }

        public static ChunkWorldPos GetBlockPos(RaycastHit hit, bool adjacent = false)
        {
            return GetBlockPos(new Vector3()
            {
                x = Round(hit.point.x, hit.normal.x, adjacent),
                y = Round(hit.point.y, hit.normal.y, adjacent),
                z = Round(hit.point.z, hit.normal.z, adjacent)
            });
        }

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

        public static bool SetBlock(RaycastHit hit, Block block, bool adjacent = false)
        {
            Chunk chunk = hit.collider.GetComponent<Chunk>();

            if (chunk == null)
                return false;

            ChunkWorldPos pos = GetBlockPos(hit, adjacent);

            if (GetBlock(hit, adjacent).breakable)
            {
                chunk.world.SetBlock(pos.x, pos.y, pos.z, block);
                Serialization.Serialization.SaveChunk(chunk);
            }

            return true;
        }

        public static Block GetBlock(RaycastHit hit, bool adjacent = false)
        {
            Chunk chunk = hit.collider.GetComponent<Chunk>();

            if (chunk == null)
                return null;

            ChunkWorldPos pos = GetBlockPos(hit, adjacent);

            return chunk.world.GetBlock(pos.x, pos.y, pos.z);
        }
    }
}