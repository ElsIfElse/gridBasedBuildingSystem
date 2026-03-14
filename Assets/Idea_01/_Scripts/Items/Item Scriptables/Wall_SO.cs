using UnityEngine;

[CreateAssetMenu(fileName = "Wall", menuName = "Data/Items/Wall")]
public class Wall_SO : Item_SO
{
    public override Item CreateItemObject()
    {
        Item item = new Wall();
        item.InitializeFrom_SO(this);
        return item;
    }
}