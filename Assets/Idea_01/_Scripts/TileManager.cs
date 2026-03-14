using System;
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
                    if(ItemChooser.Instance.SelectedItem != null)
                    {
                        // List<Tile> neighbourTiles = GetNeighbourTilesInRange(
                        //     CachedHoveredTile, BuildingChooser.Instance.SelectedBuilding.ItemSize_X - 1, 
                        //     BuildingChooser.Instance.SelectedBuilding.ItemSize_Y - 1);

                        List<Tile> effectedTiles = GetEffectedTilesBasedOnItemSize(CachedHoveredTile, ItemChooser.Instance.SelectedItem);
                        Debug.Log($"Length of effected tiles: {effectedTiles.Count}");

                        tilesToBeHighlighted.AddRange(effectedTiles);
                    }
                    // Else just add the hovered tile
                    else tilesToBeHighlighted.Add(CachedHoveredTile);

                    // Check if the area is valid for building deciding the color of the highlight
                    bool isValid = IsAreaValid(tilesToBeHighlighted,ItemChooser.Instance.SelectedItem);
                    HighlightHandler.HighlightTiles(tilesToBeHighlighted,isValid);
                    
                    // Set the preview object position to the highlighted tile
                    if(ItemChooser.Instance.CurrentlyActivePreviewObj != null)
                    {
                        ItemChooser.Instance.MakeCurrentPreviewObjectVisible();
                        ItemChooser.Instance.CurrentlyActivePreviewObj.transform.position = CachedHoveredTile.transform.position;
                    }
                }
                else
                {
                    HighlightHandler.DehighlightAllTiles();
                    ItemChooser.Instance.MakeCurrentPreviewObjectInvisible();
                }
            }
        }
    }

    bool IsAreaValid(List<Tile> tilesToCheck, Item itemToBuild)
    {
        foreach(Tile tile in tilesToCheck)
        {
            if(!tile.IsTileEmpty(itemToBuild)) return false;
        }
        return true;
    }

    void HandleTileClick()
    {
        if(CachedHoveredTile == null) return;
        Item selectedItem = ItemChooser.Instance.SelectedItem;


        if(BuildClickAction.WasPressedThisFrame())
        {
            if(ItemChooser.Instance.SelectedItem == null && (!CachedHoveredTile.IsTileEmpty()))
            {
                HandleInspectItemClick();
            }
            else if(selectedItem != null && CachedHoveredTile.IsTileEmpty(selectedItem))
            {
                HandlePlaceItemClick(CachedHoveredTile);
            }
        }
    }

    void HandleInspectItemClick()
    {
        Item itemOnTile = CachedHoveredTile.CurrentItemOnTile();
        string info ="";
        
        if(itemOnTile != null)
        {
            info += $"[Name = {itemOnTile.ItemName}] | [Price = {itemOnTile.ItemPrice}]";
            if(itemOnTile is IElectrical) info += $" | [Usage = {(itemOnTile as IElectrical).ElectricityUsage}W]";
            if(itemOnTile is ISpeedBased) info += $" | [Speed = {(itemOnTile as ISpeedBased).Speed}]";
        }

        TileInspectHandler.SetSelectedTile(CachedHoveredTile);

        Debug.Log(info);
    }
    void HandlePlaceItemClick(Tile mainTile)
    {
        Item selectedBuilding = ItemChooser.Instance.SelectedItem;
        if(selectedBuilding == null) { Debug.Log("No building selected"); return; }
        List<Tile> neighbourTiles = GetEffectedTilesBasedOnItemSize(mainTile,selectedBuilding);
        foreach(Tile tile in neighbourTiles) if(!tile.IsTileEmpty(selectedBuilding)) { Debug.Log("Tile is not empty"); return; }
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

    public List<Tile> GetEffectedTilesBasedOnItemSize(Tile tile, Item item)
    {
        List<Tile> effectedTiles = new();

        int dirX = (item.CurrentRotation == 180 || item.CurrentRotation == 270) ? -1 : 1;
        int dirY = (item.CurrentRotation == 180 || item.CurrentRotation == 90)  ? -1 : 1;

        for(int i = 0; i < item.CurrentItemSize_X; i++)
        {
            for(int j = 0; j < item.CurrentItemSize_Y; j++)
            {
                Tile neighbourTile = GetTileByCoordinates(
                    new Vector2(tile.Coordinates.x + i * dirX, tile.Coordinates.y + j * dirY));
                if(neighbourTile != null) effectedTiles.Add(neighbourTile);
            }
        }

        return effectedTiles;
    }

    public void RefreshHighlight()
    {
        if(CachedHoveredTile == null) return;
        HighlightHandler.DehighlightAllTiles();
        List<Tile> affectedTiles = GetEffectedTilesBasedOnItemSize(CachedHoveredTile, ItemChooser.Instance.SelectedItem);
        bool isValid = IsAreaValid(affectedTiles, ItemChooser.Instance.SelectedItem);
        HighlightHandler.HighlightTiles(affectedTiles, isValid);
    }
}