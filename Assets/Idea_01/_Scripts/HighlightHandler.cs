using System.Collections.Generic;

public class HighlightHandler
{
    public List<Tile> CurrentlyHighlightedTiles = new();

    public void HighlightTile(Tile tile,bool isValid)
    {
        tile.HighlightTile(true,isValid);
        CurrentlyHighlightedTiles.Add(tile);
    }

    public void DehighlightTile(Tile tile)
    {
        tile.HighlightTile(false);
        CurrentlyHighlightedTiles.Remove(tile);
    }

    public void HighlightTiles(List<Tile> tilesToHighlight, bool isValid)
    {
        foreach(Tile tile in tilesToHighlight) HighlightTile(tile,isValid); 
    }

    public void DehighlightAllTiles()
    {
        foreach(Tile tile in CurrentlyHighlightedTiles) tile.HighlightTile(false);
        CurrentlyHighlightedTiles.Clear();
    }

}