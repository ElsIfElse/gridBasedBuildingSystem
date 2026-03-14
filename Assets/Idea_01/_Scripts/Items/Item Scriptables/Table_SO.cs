using UnityEngine;

[CreateAssetMenu(fileName = "Table", menuName = "Data/Items/Table")]
public class Table_SO : Item_SO
{
    public override Item CreateItemObject()
    {
        Item item = new Table();
        item.InitializeFrom_SO(this);
        return item;
    }
}