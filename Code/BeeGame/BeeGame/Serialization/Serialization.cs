using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using BeeGame.Terrain;
using BeeGame.Terrain.Chunks;
using BeeGame.Inventory;
using BeeGame.Blocks;

namespace BeeGame.Serialization
{
    /// <summary>
    /// Serializes and Deserialises things
    /// </summary>
    public static class Serialization
    {
        #region Data
        /// <summary>
        /// Name if the world. If multiple world are ever added
        /// </summary>
        public static string worldName = "World";
        /// <summary>
        /// Save folder
        /// </summary>
        public static string saveFolderName = "Saves";
        /// <summary>
        /// Path to save things
        /// </summary>
        private static string savePath;
        #endregion

        /// <summary>
        /// Sets the paths for the save files
        /// </summary>
        public static void Init()
        {
            savePath = $"{Application.dataPath}/{saveFolderName}/{worldName}";

            if (!(Directory.Exists(savePath)))
                Directory.CreateDirectory(savePath);
        }

        #region Player
        public static void SerializeInventory(Inventory.Inventory inventory, string inventoryName)
        {
            Init();
            string inventorySavePath = $"{savePath}/Inventorys";

            if (!Directory.Exists(inventorySavePath))
                Directory.CreateDirectory(inventorySavePath);

            SaveFile(inventory.GetAllItems(), $"{inventorySavePath}/{inventoryName}.dat");
        }

        public static void DeSerializeInventory(Inventory.Inventory inventory, string inventoryName)
        {
            Init();
            string inventorySavePath = $"{savePath}/Inventorys/{inventoryName}.dat";

            if (!File.Exists(inventorySavePath))
                return;

            inventory.SetAllItems((ItemsInInventory)LoadFile($"{inventorySavePath}"));
        }
        #endregion

        #region Chunk
        /// <summary>
        /// Saves a given <see cref="Chunk"/> if a block in it has been changed
        /// </summary>
        /// <param name="chunk"></param>
        public static void SaveChunk(Chunk chunk)
        {
            //makes the folders
            Init();

            //saves the blocks
            SaveChunk save = new SaveChunk(chunk.blocks);

            //if no block was changed return early
            if (save.blocks.Count == 0)
                return;

            //otherwise save the file
            string saveFile = $"{savePath}/{FileName(chunk.chunkWorldPos)}.dat";

            SaveFile(save, saveFile);
        }

        /// <summary>
        /// Load a <see cref="Chunk"/>
        /// </summary>
        /// <param name="chunk"></param>
        /// <returns></returns>
        public static bool LoadChunk(Chunk chunk)
        {
            //Sets the folders
            Init();
            //gets teh save file
            string saveFile = $"{savePath}/{FileName(chunk.chunkWorldPos)}.dat";

            //if the file does not exist return false
            if (!File.Exists(saveFile))
                return false;

            //set all of the changed blocks in the chunk
            SaveChunk save = (SaveChunk)LoadFile(saveFile);

            foreach (var block in save.blocks)
            {
                chunk.blocks[block.Key.x, block.Key.y, block.Key.z] = block.Value;
            }

            return true;
        }

        /// <summary>
        /// Sets the file name of the <see cref="Chunk"/>
        /// </summary>
        /// <param name="pos">Position of teh <see cref="Chunk"/></param>
        /// <returns>The string of pos</returns>
        public static string FileName(ChunkWorldPos pos)
        {
            return $"{pos.x}, {pos.y}, {pos.z}";
        }
        #endregion

        #region Save/Load Files
        /// <summary>
        /// Saves the given data in the given file
        /// </summary>
        /// <param name="obj">Object to save</param>
        /// <param name="file">File path to save to</param>
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

        /// <summary>
        /// Loads the file at the given path
        /// </summary>
        /// <param name="file">File to load</param>
        /// <returns>returns the loaded file as an object</returns>
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
