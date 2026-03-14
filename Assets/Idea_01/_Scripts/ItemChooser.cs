using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemChooser : MonoBehaviour
{
    #region Singleton
    public static ItemChooser Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion
    public Item SelectedItem;
    public GameObject CurrentlyActivePreviewObj;

    InputAction DeselectSelectionsAction;

    void Start()
    {
        DeselectSelectionsAction = new InputAction("DeselectSelections", InputActionType.Button, "<Keyboard>/escape");
        DeselectSelectionsAction.Enable();
    }

    void Update()
    {
        if(DeselectSelectionsAction.WasPressedThisFrame() && SelectedItem != null) DeselectItemSelection();
    }

    public void OnChoosingBuilding(Item building)
    {
        // Cache the currently hovered Tile and if it is null then return
        Tile currentlyHoveredTile = TileManager.Instance.CachedHoveredTile;
        // Debug.Log($"Currently hovered tile: {currentlyHoveredTile.Coordinates}");

        DisableAllPreviewObjects();
        SelectedItem = building;
        Debug.Log($"Selected item: {SelectedItem.ItemName}");

        GameObject previewObj = ItemFactory.Instance.ItemWithPreviewList[SelectedItem];
        Debug.Log($"Preview object: {previewObj.name}");

        CurrentlyActivePreviewObj = previewObj;

        if(currentlyHoveredTile == null)
        {
            CurrentlyActivePreviewObj.SetActive(false);
        }
        else
        {
            CurrentlyActivePreviewObj.SetActive(true);            
            previewObj.transform.position = currentlyHoveredTile.transform.position;
        }
    }

    public void DeselectItemSelection()
    {
        SelectedItem = null;
        CurrentlyActivePreviewObj = null;
        DisableAllPreviewObjects();
    }

    public void DisableAllPreviewObjects()
    {
        List<GameObject> objs = ItemFactory.Instance.PreviewObjects;
        foreach(GameObject obj in objs)
        {
            if(obj != null && obj.activeSelf) obj.SetActive(false);
        }
    }

    public void MakeCurrentPreviewObjectVisible()
    {
        if(CurrentlyActivePreviewObj != null) CurrentlyActivePreviewObj.SetActive(true);
    }
    public void MakeCurrentPreviewObjectInvisible()
    {
        if(CurrentlyActivePreviewObj != null) CurrentlyActivePreviewObj.SetActive(false);
    }
}