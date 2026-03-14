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

    Item GetClosestItemType(Item item, Vector3 position)
    {
        switch(item)
        {
            case Chair:
                float lowestDistance = float.MaxValue;
                foreach(Chair chair in Chairs)
                {
                    float distance = Vector3.Distance(chair.ChairObj.transform.position, position);
                    if(distance < lowestDistance)
                    {
                        lowestDistance = distance;
                        return chair;
                    }
                }
                break;
        }

        return null;
    }

    public Chair GetClosestChair(Vector3 position)
    {
        return GetClosestItemType(new Chair(), position) as Chair;
    }
}