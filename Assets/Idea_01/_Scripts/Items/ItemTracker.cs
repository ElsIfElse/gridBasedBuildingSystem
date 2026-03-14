using System.Collections.Generic;
using UnityEngine;

public class ItemTracker : MonoBehaviour
{
    #region Singleton
    public static ItemTracker Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public List<Chair> Chairs = new();

    public void AddItem(Item item)
    {
        switch(item)
        {
            case Chair:
                Chairs.Add(item as Chair);
                Debug.Log($"Added chair item. Count: {Chairs.Count}");
                break;
        }
    }

    public void RemoveItem(Item item)
    {
        switch(item)
        {
            case Chair:
                Chairs.Remove(item as Chair);
                break;
        }
    }
}