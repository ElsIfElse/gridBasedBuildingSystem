using UnityEngine;
using UnityEngine.UI;

public class ItemChooserButton : Button
{
    [Header("Item to build")]
    Item _itemToBuild;

    public void SetupButtonClick(Item itemToBuild)
    {
        onClick.AddListener(()=> ItemChooser.Instance.OnChoosingBuilding(itemToBuild));
        _itemToBuild = itemToBuild;
    }
}