using System.IO;
using UnityEngine;

[CreateAssetMenu (fileName = "Building",menuName = "Data/Buildings/Building Data")]
public abstract class Item_SO : ScriptableObject
{
    [Header("Item Size")]
    public int ItemSize_X;
    public int ItemSize_Y;
    
    [Header("Item Details")]
    public string ItemName;    
    public int ItemPrice;
    public GameObject ItemPrefab;

    public abstract Item CreateItemObject();
}