using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingChooser : MonoBehaviour
{
    #region Singleton
    public static BuildingChooser Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion
    public Item SelectedBuilding;
    public GameObject CurrentlyActivePreviewObj;

    InputAction DeselectSelectionsAction;

    void Start()
    {
        DeselectSelectionsAction = new InputAction("DeselectSelections", InputActionType.Button, "<Keyboard>/escape");
        DeselectSelectionsAction.Enable();
    }

    void Update()
    {
        if(DeselectSelectionsAction.WasPressedThisFrame() && SelectedBuilding != null) DeselectItemSelection();
    }

    public void OnChoosingBuilding(Item building)
    {
        // Cache the currently hovered Tile and if it is null then return
        Tile currentlyHoveredTile = TileManager.Instance.CachedHoveredTile;
        DisableAllPreviewObjects();
        SelectedBuilding = building;
        GameObject previewObj = ItemFactory.Instance.ItemWithPreviewList[SelectedBuilding];
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
        SelectedBuilding = null;
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

    public void SetCurrentPreviewObjectVisible()
    {
        if(CurrentlyActivePreviewObj != null) CurrentlyActivePreviewObj.SetActive(true);
    }
    public void SetCurrentPreviewObjectInvisible()
    {
        if(CurrentlyActivePreviewObj != null) CurrentlyActivePreviewObj.SetActive(false);
    }
}