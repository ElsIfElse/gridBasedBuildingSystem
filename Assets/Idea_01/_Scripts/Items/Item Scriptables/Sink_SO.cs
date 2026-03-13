using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Sink", menuName = "Data/Items/Sink_01")]
public class Sink_SO : Item_SO
{   
    public override Item CreateItemObject()
    {
        Item item = new Sink();
        item.InitializeFrom_SO(this);
        return item;
    }
}