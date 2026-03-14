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

    // public bool IsTileEmpty => BuiltItemGameobjectOnTile == null;
    public bool IsTileEmpty(Item itemToBuild = default)
    {
        if(itemToBuild == default)
        {
            return BuiltItemGameobjectOnTile == null && BuiltWallGameobjectOnTile == null;
        }

        if(itemToBuild is Wall)
        {
            if(BuiltWallGameobjectOnTile == null) return true;
            else return false;
        }
        else
        {
            if(BuiltItemGameobjectOnTile == null) return true;
            else return false;
        }
    }

    public Item _itemOnTile;
    public GameObject BuiltItemGameobjectOnTile;
    public Wall _wallOnTile;
    public GameObject BuiltWallGameobjectOnTile;

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
    public Item CurrentItemOnTile()
    {
        if(_itemOnTile == null) return null;
        else return _itemOnTile;
    }
    public Wall CurrentWallOnTile()
    {
        if(_wallOnTile == null ) return null;
        else return _wallOnTile;
    }
    public void AddItemToTile(Item item,GameObject itemObj)
    {
        if(item is Wall)
        {
            _wallOnTile = item as Wall;
            BuiltWallGameobjectOnTile = itemObj;
        }
        else
        {
            _itemOnTile = item;
            BuiltItemGameobjectOnTile = itemObj;
        }
    }
    public void RemoveItemFromTile()
    {
        _itemOnTile = null;
        Destroy(BuiltItemGameobjectOnTile);
        BuiltItemGameobjectOnTile = null;
    }
    public void RemoveWallFromTile()
    {
        _wallOnTile = null;
        Destroy(BuiltWallGameobjectOnTile);
        BuiltWallGameobjectOnTile = null;
    }
    public Item ItemOnTile => _itemOnTile;
    public Wall WallOnTile => _wallOnTile;

}