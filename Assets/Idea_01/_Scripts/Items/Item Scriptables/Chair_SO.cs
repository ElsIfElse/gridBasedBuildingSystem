using UnityEngine;
[CreateAssetMenu(fileName = "Chair", menuName = "Data/Items/Chair")]
public class Chair_SO : Item_SO
{
    public override Item CreateItemObject()
    {
        Item item = new Chair();
        item.InitializeFrom_SO(this);
        return item;
    }
}   