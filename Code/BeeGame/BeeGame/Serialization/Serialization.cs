using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using BeeGame.Terrain;
using BeeGame.Terrain.Chunks;
using BeeGame.Terrain.Blocks;
using System.Threading;

namespace BeeGame.Serialization
{
    public static class Serialization
    {
        public static string worldName = "World";

        public static string saveFolderName = "Saves";

        private static string savePath;

        public static void Init()
        {
            savePath = $"{Application.dataPath}/{saveFolderName}/{worldName}";

            if (!(Directory.Exists(savePath)))
                Directory.CreateDirectory(savePath);
        }

        #region Chunk
        public static void SaveChunk(Chunk chunk)
        {
            Init();
            Thread thread = new Thread(() => SaveChunkThread(chunk)) { Name = $"Save Chunk at @ {chunk.chunkWorldPos.ToString()}" };

            thread.Start();
        }

        private static void SaveChunkThread(Chunk chunk)
        {
            SaveChunk save = new SaveChunk(chunk);

            if (save.blocks.Count == 0)
                return;

            string saveFile = $"{savePath}/{FileName(chunk.chunkWorldPos)}.dat";

            SaveFile(save, saveFile);
        }

        public static bool LoadChunk(Chunk chunk)
        {
            Init();
            string saveFile = $"{savePath}/{FileName(chunk.chunkWorldPos)}.dat";

            if (!File.Exists(saveFile))
                return false;

            SaveChunk save = (SaveChunk)LoadFile(saveFile);

            foreach (var block in save.blocks)
            {
                chunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
            }

            return true;
        }

        private static void LoadChunkThread()
        {

        }

        public static string FileName(ChunkWorldPos pos)
        {
            return $"{pos.x}, {pos.y}, {pos.z}";
        }
        #endregion

        #region Save/Load Files
        private static void SaveFile(object obj, string file)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(file, FileMode.OpenOrCreate);

            try
            {
                bf.Serialize(fs, obj);
            }
            catch(SerializationException e)
            {
                Debug.Log($"Serialization Exception: {e}");
                throw new SerializationException();
            }
            finally
            {
                fs.Close();
            }
        }

        private static object LoadFile(string file)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(file, FileMode.Open);

            try
            {
                return bf.Deserialize(fs);
            }
            catch(SerializationException e)
            {
                Debug.Log($"Deserialization Exception {e}");
                throw new SerializationException();
            }
            finally
            {
                fs.Close();
            }
        }
        #endregion
    }
}
