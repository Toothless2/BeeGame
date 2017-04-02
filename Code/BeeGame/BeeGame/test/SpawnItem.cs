using UnityEngine;
using BeeGame.Items;
using BeeGame.Core;
using BeeGame.Enums;
using BeeGame.Quest;
using BeeGame.Bee;
using BeeGame.Blocks;

public class SpawnItem : MonoBehaviour
{
    void Start()
    {
        Item item = new Item("5");

        //item.ApplyDefaultBeeData(BeeSpecies.FOREST);
        //item.honeyComb = new BeeGame.Bee.ColourChanger.HoneyComb(HoneyCombType.ICEY);

        Spawn(item);
    }

    public static void Spawn(Item item)
    {
        item.UpdateSpriteAndObject();
        GameObject go = Instantiate(item.itemGameobject);

        if (go.GetComponent<BlockGameObjectInterface>() != null)
        {
            go.GetComponent<BlockGameObjectInterface>().UpdateBlockData(item, new BeeGame.THVector3());
            go.GetComponent<BlockGameObjectInterface>().DestroyBlock();
        }
        else
        {
            go.GetComponent<ItemGameObjectInterface>().UpdateItemData(item);
        }

        go.transform.position = go.GetComponent<ItemGameObjectInterface>().item.pos;
        go.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }
}
