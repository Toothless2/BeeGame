using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using BeeGame.Core;
using BeeGame.Inventory;
using BeeGame.Items;
using BeeGame.Blocks;
using System.Collections;

namespace BeeGame.Serialization
{
    public static class Serialization 
    {
        private static string basePath;
        //private static object[] allData;
        private static object[] playerData = new object[2];
        private static object[] item;
        private static object[] blocks;

        static void Init()
        {
            basePath = UnityEngine.Application.dataPath + "/Saves/";
            blocks = new object[1];
            item = new object[1];
        }

        public static void Save()
        {
            SavePlayer();
            SaveItems();
            SaveBlocks();
        }

        public static void Load()
        {
            Init();

            RemakePlayer();
            LoadBlocks();
            RemakeItems();
        }

        //void SaveAllItems()
        //{
        //    allData = new object[item.Length + blocks.Length + playerData.Length];
        //    allData.spal
        //}

        #region Items
        static void SaveItems()
        {
            GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

            item = new object[items.Length];

            for(int i = 0; i < items.Length; i++)
            {
                item[i] = items[i].GetComponent<ItemGameObjectInterface>().item;
            }

            SaveData(item, basePath + "items.dat");
            
        }

        static void RemakeItems()
        {
            if (File.Exists(basePath + "playerData.dat"))
            {
                item = LoadData(basePath + "items.dat");

                for (int i = 0; i < item.Length; i++)
                {
                    if(item[i] != null)
                    {
                        Item tempItem = (Item)item[i];
                        tempItem.UpdateSpriteAndObject();
                        GameObject temp = UnityEngine.Object.Instantiate(tempItem.itemGameobject);
                        temp.GetComponent<ItemGameObjectInterface>().UpdateItemData(tempItem);
                        temp.transform.position = temp.GetComponent<ItemGameObjectInterface>().item.pos.ToUnityVector3();

                        for (int h = temp.GetComponents<Component>().Length - 1; h >= 5; h--)
                        {
                            UnityEngine.Object.Destroy(temp.GetComponents<Component>()[h]);
                        }
                    }
                }
            }
        }
        #endregion

        #region Blocks
        public static void AddToSaveBlocks(GameObject block)
        {
            BlockGameObjectInterface blockItem = block.GetComponent<BlockGameObjectInterface>();
            
            Array.Resize(ref blocks, blocks.Length + 1);

            blocks[blocks.Length - 1] = blockItem.ReturnBlockData();

            SaveBlocks();
        }

        public static void RemoveFromSaveBlocks(GameObject _block)
        {
            int minus = 0;
            Block block = _block.GetComponent<BlockGameObjectInterface>().ReturnBlockData();

            object[] temp = new object[blocks.Length - 1];

            for(int i = 1; i < blocks.Length; i++)
            {
                if((Block)blocks[i] == block)
                {
                    minus += 1;
                    continue;
                }

                temp[i - minus] = blocks[i];
            }

            blocks = new object[temp.Length];
            blocks = temp;
        }

        static void SaveBlocks()
        {
            blocks[0] = null;
            SaveData(blocks, basePath + "blocks.dat");
        }

        static void LoadBlocks()
        {
            if(File.Exists(basePath + "blocks.dat"))
            {
                blocks = LoadData(basePath + "blocks.dat");

                for(int i = 0; i < blocks.Length; i++)
                {
                    if(blocks[i] != null)
                    {
                        Block tempBlock = (Block)blocks[i];
                        tempBlock.item.UpdateSpriteAndObject();
                        GameObject temp = UnityEngine.Object.Instantiate(tempBlock.item.itemGameobject, ExtenstionMethods.ToUnityVector3(tempBlock.position), Quaternion.identity);
                        temp.tag = "Block";
                        UnityEngine.Object.Destroy(temp.GetComponent<ItemGameObjectInterface>());
                        temp.AddComponent<BlockGameObjectInterface>();
                        temp.GetComponent<BlockGameObjectInterface>().UpdateBlockData(tempBlock);
                    }
                }
            }
        }
        #endregion

        #region Player
        static void SavePlayer()
        {
            GameObject player = GameObject.Find("Player");

            if(player.GetComponentInChildren<InventoryBase>())
            {
                PlayerSerialization playerPosition = new PlayerSerialization(player.GetComponent<Transform>());

                playerData[0] = playerPosition;
                playerData[1] = player.GetComponentInChildren<InventoryBase>().slotandItem;

                SaveData(playerData, (basePath + "playerData.dat"));
            }
        }

        static void RemakePlayer()
        {
            if(File.Exists(basePath + "playerData.dat"))
            {
                playerData = LoadData(basePath + "playerData.dat");

                GameObject player = UnityEngine.Object.Instantiate(PrefabDictionary.GetGameObjectItemFromDictionary("Player"));
                player.name = "Player";

                //sets players position
                PlayerSerialization playerpoz = playerData[0] as PlayerSerialization;
                player.GetComponent<Transform>().position = playerpoz.ReturnTransformPosition();
                player.GetComponent<Transform>().rotation = playerpoz.ReturnTransfomRotation();

                //sets players inv
                Item[] items = playerData[1] as Item[];
                player.GetComponentInChildren<InventoryBase>().slotandItem = items;
                player.GetComponentInChildren<InventoryBase>().UpdateSlots();
            }
        }
        #endregion


        static object[] LoadData(string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);

            try
            {
                object[] tempObject = (object[])bf.Deserialize(fs);

                return tempObject;
            }
            catch(SerializationException e)
            {
                Debug.LogWarning(e);
                return null;
            }
            finally
            {
                fs.Close();
            }
        }

        static void SaveData(object[] data, string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate);

            try
            {
                bf.Serialize(fs, data);
            }
            catch (SerializationException e)
            {
                Debug.LogWarning(e);
            }
            finally
            {
                fs.Close();
            }
        }
    }

    //Misc Serialization
    [Serializable]
    public class PlayerSerialization
    {
        public float x;
        public float y;
        public float z;

        public float rotw;
        public float rotx;
        public float roty;
        public float rotz;

        PlayerSerialization() { }

        public PlayerSerialization(Transform playerTransform)
        {
            x = playerTransform.position.x;
            y = playerTransform.position.y;
            z = playerTransform.position.z;

            rotw = playerTransform.rotation.w;
            rotx = playerTransform.rotation.x;
            roty = playerTransform.rotation.y;
            rotz = playerTransform.rotation.z;
        }

        public Vector3 ReturnTransformPosition()
        {
            return new Vector3(x, y, z);
        }

        public Quaternion ReturnTransfomRotation()
        {
            return new Quaternion(rotx, roty, rotz, rotw);
        }
    }
}