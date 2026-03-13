using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ItemChooseUiManager : MonoBehaviour
{
    #region Singleton
    public static ItemChooseUiManager Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public Transform ButtonParent;
    public GameObject ButtonPrefab;

    public void CreateButtons()
    {
        foreach(Item building in ItemFactory.Instance.ItemWithPreviewList.Keys)
        {
            GameObject buttonObj = Instantiate(ButtonPrefab, transform);
            buttonObj.transform.SetParent(ButtonParent,false);
            ItemChooserButton button = buttonObj.GetComponent<ItemChooserButton>();
            button.GetComponentInChildren<TextMeshProUGUI>().text = $"{building.ItemName} - ${building.ItemPrice}";
            button.SetupButtonClick(building);
        }
    }
}