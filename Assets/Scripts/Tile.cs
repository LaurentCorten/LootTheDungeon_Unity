using UnityEngine;

public class Tile
{
    private Vector2Int _coords;
    private TileState _tileState;

    public Tile(Vector2Int coords, TileState tileState = TileState.Available)
    {
        _coords = coords;
        _tileState = tileState;
    }

    public Vector2Int Coord { get =>_coords;}
    public TileState TileState { get => _tileState; }
}

public enum TileState
{
    Available,
    Obstacle,
    Encounter,
    Exit,
    Start
}
