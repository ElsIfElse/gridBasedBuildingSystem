using System;
using UnityEngine;

[Serializable]
public abstract class Item
{
    public string ItemName;  
    public int ItemPrice;

    public int ItemSize_X;
    public int ItemSize_Y;

    public GameObject ItemPrefab;
    public GameObject BuiltItemGameobject;

    public virtual void InitializeFrom_Object(Item objectData)
    {
        ItemName = objectData.ItemName;
        ItemPrice = objectData.ItemPrice;

        ItemSize_X = objectData.ItemSize_X;
        ItemSize_Y = objectData.ItemSize_Y;
        ItemPrefab = objectData.ItemPrefab;
    }

    public virtual void InitializeFrom_SO(Item_SO soData)
    {
        ItemName = soData.ItemName;
        ItemPrice = soData.ItemPrice;

        ItemSize_X = soData.ItemSize_X;
        ItemSize_Y = soData.ItemSize_Y;
        ItemPrefab = soData.ItemPrefab;
    }

    public void SetBuiltBuildingGameobject(GameObject obj) => BuiltItemGameobject = obj;
}