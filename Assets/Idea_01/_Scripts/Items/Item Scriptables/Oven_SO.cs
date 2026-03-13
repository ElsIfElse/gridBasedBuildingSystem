using UnityEngine;

[CreateAssetMenu(fileName = "Oven", menuName = "Data/Items/Oven_01")]
public class Oven_SO : Item_SO
{
    public float ElectricityUsage;
    public float Speed;
    public override Item CreateItemObject()
    {
        Item item = new Oven();
        item.InitializeFrom_SO(this);
        return item;
    }
}