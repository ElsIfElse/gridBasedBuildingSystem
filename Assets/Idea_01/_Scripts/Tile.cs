using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Tile : MonoBehaviour
{   
    [Header("Positions and Coordinates")]
    public int Position_X;
    public int Position_Y;
    public Vector2 Coordinates = new();
    public Vector2 WorldPosition = new();
    public Material HoverMaterial_Invalid;
    public Material HoverMaterial_Valid;
    Material BaseMaterial;
    MeshRenderer Renderer;

    public bool IsTileEmpty => _builtBuildingGameobjectOnTile == null;

    public Item _itemOnTile;
    public GameObject _builtBuildingGameobjectOnTile;

    void Start()
    {
        Renderer = gameObject.GetComponentInChildren<MeshRenderer>();
        BaseMaterial = Renderer.material;
    }
    
    public void RegisterTileInTileManager(int x, int y)
    {
        Position_X = x;
        Position_Y = y; 
        Coordinates = new Vector2(x, y);

        TileManager.Instance.RegisterTile(this);
    }
    public void HighlightTile(bool state,bool isValid = true)
    {
        if(state)
        {
            if(isValid) Renderer.material = HoverMaterial_Valid;
            else Renderer.material = HoverMaterial_Invalid;
        }
        else Renderer.material = BaseMaterial;
    }
    public Item CurrentBuildingOnTile()
    {
        if(IsTileEmpty) return null;
        else return _itemOnTile;
    }
    public void AddBuildingToTile(Item building)
    {
        _itemOnTile = building;
        _builtBuildingGameobjectOnTile = _itemOnTile.BuiltItemGameobject;
    }
    public void RemoveBuildingFromTile()
    {
        _itemOnTile = null;
        Destroy(_builtBuildingGameobjectOnTile);
        _builtBuildingGameobjectOnTile = null;
    }
    public Item ItemOnTile => _itemOnTile;

}