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

    public GameObject CurrentlyActivePreviewObj;
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
            if(BuildingChooser.Instance.CurrentlyActivePreviewObj != null) BuildingChooser.Instance.CurrentlyActivePreviewObj.transform.Rotate(0, 90, 0);
        }
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
        Item building = BuildingChooser.Instance.SelectedBuilding;
        if(MoneyHandler.Instance.IsAffordable(building) == false) return;
        if(building == null)
        {
            Debug.LogError("No building selected");
            return;
        }
        if(building.ItemPrefab == null)
        {
            Debug.LogError("No prefab was found on selected building");
            return;
        }
        
        GameObject buildingObj = Instantiate(building.ItemPrefab);
        building.SetBuiltBuildingGameobject(buildingObj);
        
        foreach(Tile tile in affectedTiles) tile.AddBuildingToTile(building);

        building.BuiltItemGameobject.transform.SetPositionAndRotation(mainTile.transform.position, BuildingChooser.Instance.CurrentlyActivePreviewObj.transform.rotation);
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
