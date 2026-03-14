using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemFactory : MonoBehaviour
{
    #region Singleton
    public static ItemFactory Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    public List<Item_SO> ItemDataList = new();
    public List<GameObject> PreviewObjects = new();
    public Material TransparentMaterial; 

    public Dictionary<Item, GameObject> ItemWithPreviewList = new();

    InputAction RotateObjectAction;

    void InitializeActions()
    {
        RotateObjectAction = new();
        RotateObjectAction.AddBinding("<Mouse>/rightButton");
        RotateObjectAction.Enable();
    }
    
    void Start()
    {
        InitializeActions();
        CreateBuildingObjects();
        ItemChooseUiManager.Instance.CreateButtons();
        Debug.Log($"Length of dictionary: {ItemWithPreviewList.Count}");
    }

    void Update()
    {
        HandleRotateObject();
    }

    void HandleRotateObject()
    {
        if(RotateObjectAction.WasPressedThisFrame())
        {
            if(ItemChooser.Instance.CurrentlyActivePreviewObj != null)
            {
                ItemChooser.Instance.CurrentlyActivePreviewObj.transform.Rotate(0, 90, 0);
                HandleSizeChangeWhileRotating();
                TileManager.Instance.RefreshHighlight();
            }
        }
    }

    void HandleSizeChangeWhileRotating()
    {
        Item item = ItemChooser.Instance.SelectedItem;
        int angle = Mathf.RoundToInt(ItemChooser.Instance.CurrentlyActivePreviewObj.transform.rotation.eulerAngles.y) % 360;

        switch(angle)
        {
            case 0: case 180:
                item.CurrentItemSize_X = item.OriginalItemSize_X;
                item.CurrentItemSize_Y = item.OriginalItemSize_Y;
                break;
            case 90: case 270:
                item.CurrentItemSize_X = item.OriginalItemSize_Y;
                item.CurrentItemSize_Y = item.OriginalItemSize_X;
                break;
        }

        item.CurrentRotation = angle;
        Debug.Log($"Size of selected item changed: {item.CurrentItemSize_X}x{item.CurrentItemSize_Y}");
    }

    GameObject CreateBuildingPreviewObject(Item building)
    {
        GameObject buildingPreview = Instantiate(building.ItemPrefab);
        MeshRenderer meshRenderer = buildingPreview.GetComponentInChildren<MeshRenderer>();
        Material material = new(meshRenderer.material){color = new Color(0.5f, 0.7f, 1f, 0.3f)};
        SetMaterialTransparent(material);
        meshRenderer.material = material;
        buildingPreview.SetActive(false);
        return buildingPreview;
    }

    void CreateBuildingObjects()
    {
        foreach(Item_SO data in ItemDataList)
        {
            Item item = data.CreateItemObject();
            GameObject previewObj = CreateBuildingPreviewObject(item);
            PreviewObjects.Add(previewObj);
            ItemWithPreviewList.Add(item, previewObj);
        }
    }

    public void PlaceBuildingOnTile(Tile mainTile,List<Tile> affectedTiles)
    {
        Item item = ItemChooser.Instance.SelectedItem;
        if(MoneyHandler.Instance.IsAffordable(item) == false) return;
        if(item == null)
        {
            Debug.LogError("No building selected");
            return;
        }
        if(item.ItemPrefab == null)
        {
            Debug.LogError("No prefab was found on selected building");
            return;
        }
        
        GameObject buildingObj = Instantiate(item.ItemPrefab);
        mainTile.AddItemToTile(item,buildingObj);
        
        foreach(Tile tile in affectedTiles) tile.AddItemToTile(item,buildingObj);

        if(item is Wall) mainTile.BuiltWallGameobjectOnTile.transform.SetPositionAndRotation(mainTile.transform.position, ItemChooser.Instance.CurrentlyActivePreviewObj.transform.rotation);
        else mainTile.BuiltItemGameobjectOnTile.transform.SetPositionAndRotation(mainTile.transform.position, ItemChooser.Instance.CurrentlyActivePreviewObj.transform.rotation);
    }

    public void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Surface", 1);          // 0 = Opaque, 1 = Transparent
        mat.SetFloat("_Blend", 0);            // 0 = Alpha
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }
    

}
