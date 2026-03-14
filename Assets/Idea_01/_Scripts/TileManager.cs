using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class TileManager : MonoBehaviour
{
    #region Singleton
    public static TileManager Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    Vector2 MousePosition
    {
        get
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if(Physics.Raycast(ray, out RaycastHit hit))
                return new Vector2(hit.point.x, hit.point.z);
            return Vector2.zero;
        }
    }
    public Dictionary<Vector2,Tile> Tiles = new();
    public Tile SelectedTile;
    public List<Tile> HighlightedTiles;
    public Tile CachedHoveredTile;
    public HighlightHandler HighlightHandler;
    public InputAction BuildClickAction;
    TileInspectHandler TileInspectHandler;

    void Start()
    {
        InitializeActions();
        TileInspectHandler = new(this);
        HighlightHandler = new();
    }

    void InitializeActions()
    {
        BuildClickAction = new();
        BuildClickAction.AddBinding("<Mouse>/leftButton");
        BuildClickAction.Enable();
    }

    void Update()
    {

        HandleHighlights();
        HandleTileClick();
    }
    
    void HandleHighlights()
    {
        // If the mouse moves
        if(Mouse.current.delta.ReadValue() != Vector2.zero)
        {
            // Get the hovered tile
            Tile newHoveredTile = GetHoveredTile();

            // If it is not the one we have cached
            if(newHoveredTile != CachedHoveredTile)
            {
                // If the saved one is not null then dehighlight all tiles
                if(CachedHoveredTile != null) HighlightHandler.DehighlightAllTiles();

                // Cache the new hovered tile
                CachedHoveredTile = newHoveredTile;
                
                // If the new cached tile is not null
                if(CachedHoveredTile != null)
                {
                    List<Tile> tilesToBeHighlighted = new();

                    // If we selected an item to build, check the size of it and the effected tiles and add them to the to be highlighted list
                    if(BuildingChooser.Instance.SelectedBuilding != null)
                    {
                        List<Tile> neighbourTiles = GetNeighbourTilesInRange(
                            CachedHoveredTile, BuildingChooser.Instance.SelectedBuilding.ItemSize_X - 1, 
                            BuildingChooser.Instance.SelectedBuilding.ItemSize_Y - 1);

                        tilesToBeHighlighted.AddRange(neighbourTiles);
                    }
                    // Else just add the hovered tile
                    else tilesToBeHighlighted.Add(CachedHoveredTile);

                    // Check if the area is valid for building deciding the color of the highlight
                    bool isValid = IsAreaValid(tilesToBeHighlighted);
                    HighlightHandler.HighlightTiles(tilesToBeHighlighted,isValid);
                    
                    // Set the preview object position to the highlighted tile
                    if(BuildingChooser.Instance.CurrentlyActivePreviewObj != null)
                    {
                        BuildingChooser.Instance.SetCurrentPreviewObjectVisible();
                        BuildingChooser.Instance.CurrentlyActivePreviewObj.transform.position = CachedHoveredTile.transform.position;
                    }
                    
                }
                else
                {
                    HighlightHandler.DehighlightAllTiles();
                    BuildingChooser.Instance.SetCurrentPreviewObjectInvisible();
                }
            }
        }
    }

    bool IsAreaValid(List<Tile> tilesToCheck)
    {
        foreach(Tile tile in tilesToCheck)
        {
            if(!tile.IsTileEmpty) return false;
        }
        return true;
    }

    void HandleTileClick()
    {
        if(CachedHoveredTile == null) return;
        
        if(BuildClickAction.WasPressedThisFrame())
        {
            if(BuildingChooser.Instance.SelectedBuilding == null && !CachedHoveredTile.IsTileEmpty)
            {
                HandleInspectItemClick();
            }
            else if(CachedHoveredTile.IsTileEmpty && BuildingChooser.Instance.SelectedBuilding != null)
            {
                HandlePlaceItemClick();
            }
        }
    }

    void HandleInspectItemClick()
    {
        Item itemOnTile = CachedHoveredTile.CurrentBuildingOnTile();
        string info = $"[Name = {itemOnTile.ItemName}] | [Price = {itemOnTile.ItemPrice}]";
        if(itemOnTile is IElectrical) info += $" | [Usage = {(itemOnTile as IElectrical).ElectricityUsage}W]";
        if(itemOnTile is ISpeedBased) info += $" | [Speed = {(itemOnTile as ISpeedBased).Speed}]";

        TileInspectHandler.SetSelectedTile(CachedHoveredTile);

        Debug.Log(info);
    }
    void HandlePlaceItemClick()
    {
        Item selectedBuilding = BuildingChooser.Instance.SelectedBuilding;
        if(selectedBuilding == null) { Debug.Log("No building selected"); return; }
        List<Tile> neighbourTiles = GetNeighbourTilesInRange(CachedHoveredTile, selectedBuilding.ItemSize_X - 1, selectedBuilding.ItemSize_Y - 1);
        foreach(Tile tile in neighbourTiles) if(!tile.IsTileEmpty) { Debug.Log("Tile is not empty"); return; }
        ItemFactory.Instance.PlaceBuildingOnTile(CachedHoveredTile, neighbourTiles);
    }
    Tile GetHoveredTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        if(Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Tile"))) return hit.collider.GetComponent<Tile>();
        return null;
    }
    
    public void RegisterTile(Tile tile) => Tiles.Add(tile.Coordinates,tile);

    public Tile GetTileByWorldPosition(Vector2 worldPosition)
    {
        float halfSize = GridMaker.Instance.TileSize / 2;

        foreach(KeyValuePair<Vector2,Tile> pair in Tiles)
        {
            Tile tile = pair.Value;

            float x_max = tile.transform.position.x + halfSize;
            float x_min = tile.transform.position.x - halfSize;
            float y_max = tile.transform.position.z + halfSize;
            float y_min = tile.transform.position.z - halfSize;

            if(worldPosition.x > x_min && worldPosition.x < x_max && worldPosition.y > y_min && worldPosition.y < y_max) return tile;
        }

        return null;
    }
    public Tile GetTileByCoordinates(Vector2 coordinates)
    {
        foreach(KeyValuePair<Vector2,Tile> tile in Tiles)
        {
            if(tile.Value.Coordinates == coordinates)
            {
                return tile.Value;
            }
        }

        return null;
    }
    public List<Tile> GetNeighbourTilesInRange(Tile tile, int x, int y)
    {
        List<Tile> neighbourTiles = new();

        for(int i = -x; i <= x; i++)
        {
            for(int j = -y; j <= y; j++)
            {
                Tile neighbourTile = GetTileByCoordinates(new Vector2(tile.Coordinates.x + i, tile.Coordinates.y + j));
                if(neighbourTile != null) neighbourTiles.Add(neighbourTile);
            }
        }
        return neighbourTiles;
    }
}