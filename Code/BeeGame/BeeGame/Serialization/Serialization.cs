using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Terrain;
using BeeGame.Terrain.Chunks;
using BeeGame.Inventory;
using BeeGame.Blocks;

namespace BeeGame.Serialization
{
    /// <summary>
    /// Serializes and Deserialises things
    /// </summary>
    /// <remarks>
    /// Binary serialization is SLOW try to only serialize only what is absolutly necessary
    /// </remarks>
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
        public static void MakeDirectorys()
        {
            savePath = $"{Application.dataPath}/{saveFolderName}/{worldName}";

            if (!(Directory.Exists(savePath)))
                Directory.CreateDirectory(savePath);
        }

        /// <summary>
        /// Deletes the given file if it exists, Starts in <see cref="Application.dataPath"/>
        /// </summary>
        /// <param name="fileName">File to delete</param>
        public static void DeleteFile(string fileName)
        {
            string[] file = Directory.GetFiles(Application.dataPath + "/Saves", "*.dat", SearchOption.AllDirectories);

            string[] splitCharacters = { "/", "\\" };

            for (int i = 0; i < file.Length; i++)
            {
                string[] temp = file[i].Split(splitCharacters, System.StringSplitOptions.RemoveEmptyEntries);

                if(temp[temp.Length - 1] == fileName)
                {
                    File.Delete(file[i]);

                    return;
                }
            }
        }

        #region Player
        /// <summary>
        /// Saves the player positon, rotation, and scale
        /// </summary>
        /// <param name="positon">Transform to get the data from</param>
        public static void SavePlayerPosition(Transform positon)
        {
            THVector3[] playerTransform = new THVector3[3];

            playerTransform[0] = positon.position;
            playerTransform[1] = positon.rotation.eulerAngles;
            playerTransform[2] = positon.localScale;

            string playerPosSavePath = $"{savePath}/player.dat";

            SaveFile(playerTransform, playerPosSavePath);
        }

        /// <summary>
        /// Loads the players positon, roatation, and scale if it has previously been saved
        /// </summary>
        /// <param name="playerTransfom">Transform to apply the data to</param>
        public static void LoadPlayerPosition(Transform playerTransfom)
        {
            string playerPosSavePath = $"{savePath}/player.dat";

            if (!File.Exists(playerPosSavePath))
                return;

            THVector3[] pos = (THVector3[])LoadFile(playerPosSavePath);

            playerTransfom.position = pos[0];
            playerTransfom.rotation = (Quaternion)pos[1];
            playerTransfom.localScale = pos[2];
        }
        #endregion

        #region Inventorys
        /// <summary>
        /// Serializes a given <see cref="Inventory"/>
        /// </summary>
        /// <param name="inventory">Invenotry to Serialize</param>
        /// <param name="inventoryName">Name of the inventory</param>
        /// <remarks>
        /// The name of the inventory for the player is "PlayerInventory". \n
        /// For all other ivnetorys the name is the block type + its position eg, Apiay@0, 0, 0
        /// </remarks>
        public static void SerializeInventory(Inventory.Inventory inventory, string inventoryName)
        {
            string inventorySavePath = $"{savePath}/Inventorys";

            if (!Directory.Exists(inventorySavePath))
                Directory.CreateDirectory(inventorySavePath);

            SaveFile(inventory.GetAllItems(), $"{inventorySavePath}/{inventoryName}.dat");
        }

        /// <summary>
        /// Deserializesd an <see cref="Inventory"/> from its name into a given <paramref name="inventory"/>
        /// </summary>
        /// <param name="inventory">Inventory to apply the data to</param>
        /// <param name="inventoryName">Inventory to deserialize</param>
        public static void DeSerializeInventory(Inventory.Inventory inventory, string inventoryName)
        {
            //* make the path
            string inventorySavePath = $"{savePath}/Inventorys/{inventoryName}.dat";

            //* checks that the file exists
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
            //* saves the blocks
            SaveChunk save = new SaveChunk(chunk.blocks);

            //* if no block was changed return early
            if (save.blocks.Count == 0)
                return;

            //* otherwise save the file
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
            //* gets the save file
            string saveFile = $"{savePath}/{FileName(chunk.chunkWorldPos)}.dat";

            //* if the file does not exist return false
            if (!File.Exists(saveFile))
                return false;

            //* set all of the changed blocks in the chunk
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
