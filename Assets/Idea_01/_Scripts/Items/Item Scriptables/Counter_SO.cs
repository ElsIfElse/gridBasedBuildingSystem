using UnityEngine;

[CreateAssetMenu(fileName = "Counter", menuName = "Data/Items/Counter_01")]
public class Counter_SO : Item_SO
{
    public override Item CreateItemObject()
    {
        Item item = new Counter();
        item.InitializeFrom_SO(this);
        return item;
    }
}