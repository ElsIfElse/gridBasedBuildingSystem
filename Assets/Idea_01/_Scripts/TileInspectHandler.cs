using UnityEngine;
using UnityEngine.InputSystem;

public class TileInspectHandler
{
    Tile _selectedTile;
    TileManager _tileManager;

    public void SetSelectedTile(Tile tile)
    {
        _selectedTile = tile;
        Debug.Log($"Selected tile: {_selectedTile.Coordinates}");
    }
    InputAction DeleteItemOnTileAction;
    InputAction DeleteWallOnTileAction;

    public TileInspectHandler(TileManager tileManager)
    {
        _tileManager = tileManager;
        InitializeActions();
    }

    void InitializeActions()
    {
        DeleteItemOnTileAction = new();
        DeleteItemOnTileAction.AddBinding("<Keyboard>/q");
        DeleteItemOnTileAction.performed += ctx => OnDeleteKeyPress_Item();
        DeleteItemOnTileAction.Enable();

        DeleteWallOnTileAction = new();
        DeleteWallOnTileAction.AddBinding("<Keyboard>/e");
        DeleteWallOnTileAction.performed += ctx => OnDeleteKeyPress_Wall();
        DeleteWallOnTileAction.Enable();
    }

    void OnDeleteKeyPress_Item()
    {
        if(_selectedTile == null) return;
        Debug.Log("Delete item key was pressed");
        MoneyHandler.Instance.AddMoney(_selectedTile.ItemOnTile.ItemPrice);
        _selectedTile.RemoveItemFromTile();
        _selectedTile = null;
    }
    void OnDeleteKeyPress_Wall()
    {
        if(_selectedTile == null) return;
        Debug.Log("Delete wall key was pressed");
        MoneyHandler.Instance.AddMoney(_selectedTile.WallOnTile.ItemPrice);
        _selectedTile.RemoveWallFromTile();
        _selectedTile = null;
    }
}