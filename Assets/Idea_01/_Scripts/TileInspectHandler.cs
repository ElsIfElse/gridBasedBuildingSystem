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
    InputAction DeleteAction;

    public TileInspectHandler(TileManager tileManager)
    {
        _tileManager = tileManager;
        InitializeActions();
    }

    void InitializeActions()
    {
        DeleteAction = new();
        DeleteAction.AddBinding("<Keyboard>/q");
        DeleteAction.performed += ctx => OnDeleteKeyPress();
        DeleteAction.Enable();
    }

    void OnDeleteKeyPress()
    {
        if(_selectedTile == null) return;
        Debug.Log("Delete key was pressed");
        MoneyHandler.Instance.AddMoney(_selectedTile.ItemOnTile.ItemPrice);
        _selectedTile.RemoveBuildingFromTile();
        _selectedTile = null;
    }
}