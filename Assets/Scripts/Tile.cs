using UnityEngine;

public class Tile
{
    // Champs
    private Vector2Int _coords;
    private TileState _tileState;
    private Enemy _enemy;

    // Propriétés
    public Vector2Int Coord { get =>_coords;}
    public TileState TileState { get => _tileState; }
    public Enemy Enemy { get => _enemy; }

    // Ctor
    public Tile(Vector2Int coords, TileState tileState = TileState.Available)
    {
        _coords = coords;
        _tileState = tileState;
        _enemy = null;
    }
    //public Tile (Vector2Int coords, Enemy enemy)
    //{
    //    _coords = coords;
    //    _tileState = TileState.Encounter;
    //    _enemy = enemy;
    //}

    public void SetEnemy(EnemyState enemyState)
    {
        _enemy = enemyState.enemy;
    }
}

public enum TileState
{
    Available,
    Obstacle,
    Encounter,
    Exit,
    Start
}
