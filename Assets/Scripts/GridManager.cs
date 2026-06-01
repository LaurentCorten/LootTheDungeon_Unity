using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int _gridSize;
    [SerializeField] int _unityGridSize;
    Dictionary<Vector2Int, Tile> _grid = new Dictionary<Vector2Int, Tile>();

    [SerializeField] int _nbEncounters = 10;
    [SerializeField] int _nbObstacles = 10;
    
    public int UnityGridSize { get { return _unityGridSize; } }
    public Dictionary<Vector2Int, Tile> Grid { get { return _grid; } }
    public Vector2Int startCoords;
    public Vector2Int exitCoords;

    private void Awake()
    {
        InitiateGrid();
        FillGrid();
    }


    private void FillGrid()
    {
        bool hasPossiblePath = false;

        do
        {
            DrawObstaclesPositions();
            startCoords = DrawTilePosition(TileState.Start);
            exitCoords = DrawTilePosition(TileState.Exit);
            hasPossiblePath = CheckPossiblePath(); //TODO !
        } while (!hasPossiblePath);

        DrawEncountersPositions();
    }

    // Pourrait regrouper les méthodes 2 par 2 qui prend l'enum en param et avec un switch dedans
    private void DrawEncountersPositions()
    {
        for(int i = 0; i < _nbEncounters; i++)
        {
            Vector2Int newEncounterPosition = PickNewAvailablePosition();
            Tile encounterTile = new Tile(newEncounterPosition, TileState.Encounter);
        }
    }
    private void DrawObstaclesPositions()
    {
        for(int i = 0; i < _nbObstacles; i++)
        {
            Vector2Int newObstaclePosition = PickNewAvailablePosition();
            Tile obstacleTile = new Tile(newObstaclePosition, TileState.Obstacle);
        }
    }

    private Vector2Int DrawTilePosition(TileState tileState)
    {
        Vector2Int exitPosition = PickNewAvailablePosition();
        Tile exitTile = new Tile(exitPosition, tileState);
        Debug.Log($"ExitPosition = ({exitPosition.x},{exitPosition.y})");
        return exitPosition;
    }

    /// <summary>
    /// Tire une position dans le _grid jusqu'à en trouver une libre.
    /// </summary>
    /// <returns>Retourne un vecteur position pas encore occupé sur la grille. Attention retourne le vecteur (-1, -1) en cas d'erreur de position hors grid !</returns>
    private Vector2Int PickNewAvailablePosition()
    {
        Vector2Int newPostion = new Vector2Int();
        bool isValidPosition = false;
        Tile testedTile;

        do
        {
            newPostion.x = Random.Range(1, _gridSize.x - 2);
            newPostion.y = Random.Range(1, _gridSize.y - 2);

            _grid.TryGetValue(newPostion, out testedTile);

            isValidPosition = testedTile.TileState == TileState.Available;

        } while (!isValidPosition);

        if(!_grid.ContainsKey(newPostion))
        {
            Debug.Log($"Position tirée = ({newPostion.x},{newPostion.y}) est non valide !");
            return new Vector2Int (-1, -1);
        }

        return newPostion;
    }

    private void InitiateGrid()
    {
        for(int x = 0; x < _gridSize.x; x++)
        {
            for(int y = 0; y < _gridSize.y; y++)
            {
                Vector2Int coords = new Vector2Int(x, y);
                Tile newTile;

                if(x == 0 || x == _gridSize.x-1
                  || y == 0 || y == _gridSize.y-1) 
                {
                    newTile = new Tile(coords, TileState.Obstacle);
                }
                else
                {
                    newTile = new Tile(coords);
                }

                _grid.Add(coords, newTile);
            }
        }
    }
}
