using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int _gridSize;
    [SerializeField] int _unityGridSize;
    [SerializeField] Dictionary<Vector2Int, Tile> _grid = new Dictionary<Vector2Int, Tile>();

    [SerializeField] int _nbEncounters = 5;
    [SerializeField] int _nbObstacles = 10;
    
    public int UnityGridSize { get { return _unityGridSize; } }
    public Dictionary<Vector2Int, Tile> Grid { get { return _grid; } }
    public Vector2Int startCoords;
    public Vector2Int exitCoords;

    private void Awake()
    {
        bool hasPossiblePath = false;

        do
        {
            ClearGrid();
            InitiateGrid();
            FillGrid();
            hasPossiblePath = CheckPathExists();
        } while (!hasPossiblePath);

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

    // TODO: Mettre une distance minimum entre start et exit !
    private void FillGrid()
    {
        AssignManyTilesPositions(_nbObstacles, TileState.Obstacle);
        startCoords = AssignOneTilePosition(TileState.Start);
        exitCoords = AssignOneTilePosition(TileState.Exit);
        AssignManyTilesPositions(_nbEncounters, TileState.Encounter);        
    }

    private void ClearGrid()
    {
        _grid.Clear();
    }

    private bool CheckPathExists()
    {
        List<Vector2Int> tilesToCheck = new List<Vector2Int> { startCoords };
        List<Vector2Int> tilesChecked = new List<Vector2Int>();
        Vector2Int[] directions = {Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        Vector2Int centralNode;
        bool isPathFound = false;

        do
        {
            // Test de garde au cas où la liste serait vide même si ça ne devrait pas.
            if (tilesToCheck[0] == null)
            {
                Debug.LogWarning("Erreur ! Liste 'à étudier' vide !");
                break;
            }

            // Chope les premières coordonées à étudier
            centralNode = tilesToCheck[0];
            Debug.Log($"tuile étudiée = ({centralNode.x},{centralNode.y})");

            // Vérifie les 4 directions
            foreach (var dir in directions)
            {
                // variables locales
                Tile workingTile;
                Vector2Int workingNode = centralNode + dir;
                Debug.Log($"tuile contrôlée = ({workingNode.x},{workingNode.y})");

                // Tests de garde pour éviter les tests redondants
                if (tilesChecked.Contains(workingNode)) continue;
                if (tilesToCheck.Contains(workingNode)) continue;

                // Chope la tuile
                _grid.TryGetValue(workingNode, out workingTile);
                Debug.Log($"Tuile récupérée, status = {workingTile.TileState}");
                
                // Vérifie si c'est la sortie
                if(workingTile.TileState == TileState.Exit)
                {
                    isPathFound = true;
                    Debug.Log("C'était bien la sortie :D");
                    break;
                }

                // Vérifie si c'est une case traversable et l'enregistre dans la liste des "à étudier" le cas échéant
                if (workingTile.TileState != TileState.Obstacle)
                {
                    tilesToCheck.Add(workingNode);
                    Debug.Log("Ce n'est pas la sortie ni un obstacle, sera étudiée plus tard");
                }
            }

            // Déplace la tuile des "à étudier" aux "étudiées"
            tilesChecked.Add(centralNode);
            tilesToCheck.Remove(centralNode);
            Debug.Log($"Tuile déplacée de liste");

        } while (!isPathFound && tilesToCheck.Count > 0);

        Debug.Log($"Sortie de boucle do/while, isPathFound = {isPathFound}");
        return isPathFound;
    }

    private List<Vector2Int> AssignManyTilesPositions(int nbTiles, TileState tileState)
    {
        List<Vector2Int> tilesCoordList = new();
        for(int i = 0; i < nbTiles; i++)
        {
            Vector2Int tilePosition = PickNewAvailablePosition();
            Tile tile = new Tile(tilePosition, tileState);
            Debug.Log($"Tile intels = ({tile.Coord.x},{tile.Coord.y}) comme {tile.TileState}");
            _grid.Remove(tilePosition);
            _grid.Add(tile.Coord, tile);
            tilesCoordList.Add(tile.Coord);
            Debug.Log($"{tileState} {i + 1} : ({tile.Coord.x};{tile.Coord.y})");
        }
        return tilesCoordList;
    }

    private Vector2Int AssignOneTilePosition(TileState tileState)
    {
        Vector2Int tilePosition = PickNewAvailablePosition();
        Tile tile = new Tile(tilePosition, tileState);
        Debug.Log($"Tile intels = ({tile.Coord.x},{tile.Coord.y}) comme {tile.TileState}");
        _grid.Remove(tilePosition);
        _grid.Add(tile.Coord, tile);
        Debug.Log($"{tileState} : ({tile.Coord.x};{tile.Coord.y})");
        return tilePosition;
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
            newPostion.x = Random.Range(1,(_gridSize.x-2));
            newPostion.y = Random.Range(1,(_gridSize.y-2));
            Debug.Log($"newPosition coords = ({newPostion.x};{newPostion.y})");

            bool successfulExtraction = _grid.TryGetValue(newPostion, out testedTile);
            Debug.Log($"successfulExtraction = {successfulExtraction} et testedTile status = {testedTile.TileState}");

            isValidPosition = testedTile.TileState == TileState.Available;
            Debug.Log($"newPosition est valid = {isValidPosition}");

        } while (!isValidPosition);

        if(!_grid.ContainsKey(newPostion))
        {
            Debug.Log($"Position tirée = ({newPostion.x},{newPostion.y}) est non valide !");
            return new Vector2Int (-1, -1);
        }

        return newPostion;
    }


}
