using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using BeeGame.Items;
using BeeGame.Blocks;
using BeeGame.Core.Enums;

namespace BeeGame
{
    class SpawnItem : MonoBehaviour
    {
        void Start()
        {
            //GameObject go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            //go.GetComponent<ItemGameObject>().item = new Bee(BeeType.DRONE, new NormalBee() { pSpecies = BeeSpecies.FOREST, sSpecies = BeeSpecies.FOREST });

            //go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            //go.GetComponent<ItemGameObject>().item = new Bee(BeeType.PRINCESS, new NormalBee() { pSpecies = BeeSpecies.FOREST, sSpecies = BeeSpecies.FOREST });

            //go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            //go.GetComponent<ItemGameObject>().item = new Bee(BeeType.QUEEN, new QueenBee() { queen = new NormalBee() { pSpecies = BeeSpecies.FOREST, sSpecies = BeeSpecies.FOREST }, drone = new NormalBee() { pSpecies = BeeSpecies.FOREST, sSpecies = BeeSpecies.FOREST } });

            //go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            //go.GetComponent<ItemGameObject>().item = new Bee(BeeType.DRONE, new NormalBee() { pSpecies = BeeSpecies.COMMON, sSpecies = BeeSpecies.COMMON });

            //go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            //go.GetComponent<ItemGameObject>().item = new Bee(BeeType.PRINCESS, new NormalBee() { pSpecies = BeeSpecies.COMMON, sSpecies = BeeSpecies.COMMON });

            //go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            //go.GetComponent<ItemGameObject>().item = new Bee(BeeType.QUEEN, new QueenBee() { queen = new NormalBee() { pSpecies = BeeSpecies.COMMON, sSpecies = BeeSpecies.COMMON }, drone = new NormalBee() { pSpecies = BeeSpecies.COMMON, sSpecies = BeeSpecies.COMMON } });

            //go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            //go.GetComponent<ItemGameObject>().item = new HoneyComb(HoneyCombType.ICEY);

            //go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            //go.GetComponent<ItemGameObject>().item = new HoneyComb(HoneyCombType.HONEY);

            //go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            //go.GetComponent<ItemGameObject>().item = new Chest();

            //go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            //go.GetComponent<ItemGameObject>().item = new Apiary();

            //go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            //go.GetComponent<ItemGameObject>().item = new CraftingTable();

            GameObject go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<ItemGameObject>().item = new QuestBook();

            go = Instantiate(UnityEngine.Resources.Load("Prefabs/ItemGameObject") as GameObject, transform.position, Quaternion.identity) as GameObject;
            go.GetComponent<ItemGameObject>().item = new BeeAlyzer();
        }

        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.green;
            //Gizmos.DrawSphere(transform.position, 0.5f);
        }
    }
}
