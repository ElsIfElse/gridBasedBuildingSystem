using UnityEngine;

[CreateAssetMenu(fileName = "Fridge", menuName = "Data/Items/Fridge_01")]
public class Fridge_SO : Item_SO
{
    public float ElectricityUsage;
    public float Speed;
    public override Item CreateItemObject()
    {
        Item item = new Fridge();
        item.InitializeFrom_SO(this);
        return item;
    }
}