using UnityEngine;
using UnityEngine.UI;

public class ItemChooserButton : Button
{
    [Header("Item to build")]
    Item _itemToBuild;

    public void SetupButtonClick(Item itemToBuild)
    {
        onClick.AddListener(()=> BuildingChooser.Instance.OnChoosingBuilding(itemToBuild));
        _itemToBuild = itemToBuild;
    }
}