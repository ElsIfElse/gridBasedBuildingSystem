using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;

public class GridMaker : MonoBehaviour
{
    #region Singleton
    public static GridMaker Instance;
    private void Awake() 
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    [Header("Grid Creation")]
    public int TileSize;
    public int WorldSize_X;
    public int WorldSize_Y;
    public int MapHeight;

    public float GridPrintingSpeed;

    [Header("Tiles")]
    public GameObject TilePrefab;
    public GameObject TileParent;
    NavMeshSurface surface;

    private void Start() 
    {
        surface = gameObject.GetComponent<NavMeshSurface>();
        CreateGrid(WorldSize_X,WorldSize_Y);
    }
    void CreateGrid(int gridSize_x,int gridSize_y)
    {
        for(int i = 0; i < gridSize_y; i++)
        {
            for(int j = 0; j < gridSize_x; j++)
            {
                CreateTileAtCoordinates(j,i);
            }
        }

        surface.BuildNavMesh();
    }

    Tile CreateTileAtCoordinates(int x,int y)
    {
        GameObject tileObj = Instantiate(TilePrefab);
        tileObj.transform.SetParent(TileParent.transform,true);
        tileObj.name = $"[{x}][{y}]";
        
        if (tileObj.TryGetComponent(out Tile tile)){}
        else tile = tileObj.AddComponent<Tile>();

        tile.RegisterTileInTileManager(x,y);
        tile.transform.position = new Vector3(x * TileSize,MapHeight,y * TileSize);
        return tile;
    }



}
