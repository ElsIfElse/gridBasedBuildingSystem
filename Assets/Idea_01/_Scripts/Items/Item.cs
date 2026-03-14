using System;
using UnityEngine;

[Serializable]
public abstract class Item
{
    public string ItemName;  
    public int ItemPrice;

    public int OriginalItemSize_X;
    public int OriginalItemSize_Y;

    public int CurrentItemSize_X;
    public int CurrentItemSize_Y;

    public GameObject ItemPrefab;
    public int CurrentRotation;
    public GameObject BuiltItemObj;

    public virtual void InitializeFrom_Object(Item objectData)
    {
        ItemName = objectData.ItemName;
        ItemPrice = objectData.ItemPrice;

        OriginalItemSize_X = objectData.OriginalItemSize_X;
        OriginalItemSize_Y = objectData.OriginalItemSize_Y;
        
        CurrentItemSize_X = objectData.CurrentItemSize_X;
        CurrentItemSize_Y = objectData.CurrentItemSize_Y;

        ItemPrefab = objectData.ItemPrefab;
    }

    public virtual void InitializeFrom_SO(Item_SO soData)
    {
        ItemName = soData.ItemName;
        ItemPrice = soData.ItemPrice;

        OriginalItemSize_X = soData.ItemSize_X;
        OriginalItemSize_Y = soData.ItemSize_Y;

        CurrentItemSize_X = soData.ItemSize_X;
        CurrentItemSize_Y = soData.ItemSize_Y;

        ItemPrefab = soData.ItemPrefab;
    }

    public void SetBuiltItemObject(GameObject obj) => BuiltItemObj = obj;
}