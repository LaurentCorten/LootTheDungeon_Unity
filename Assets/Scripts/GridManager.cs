using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] Vector2Int _gridSize;
    [SerializeField] int _unityGridSize;
    Dictionary<Vector2Int, Tile> _grid = new Dictionary<Vector2Int, Tile>();

    [SerializeField] Transform mapAnchor;
    [SerializeField] int _nbEncounters = 5;
    [SerializeField] int _nbObstacles = 10;
    [SerializeField] int _startExitOffset = 7;
    
    public int UnityGridSize { get { return _unityGridSize; } }
    public Dictionary<Vector2Int, Tile> Grid { get { return _grid; } }
    public Vector2Int startCoords;
    public Vector2Int exitCoords;

    private void Awake()
    {
        bool hasPossiblePath = false;

        do
        {
            ClearGrid(); // Pourrait être 'inutile' si Initiate faisait des attributions _Grid[pos]=tile plutôt que des Add à priori, mais ça me semble plus propre d'avoir un clear
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

    private void FillGrid()
    {
        AssignManyTilesPositions(_nbObstacles, TileState.Obstacle);
        startCoords = AssignOneTilePosition(TileState.Start);
        exitCoords = GetFarEnoughExit();
        AssignManyTilesPositions(_nbEncounters, TileState.Encounter);        
    }

    /// <summary>
    /// Permet de remettre une tuile en position neutre. Pour retirer la sortie ou après résolution d'encounter.
    /// </summary>
    private void ClearGrid()
    {
        _grid.Clear();
    }

    public void ClearOneTile(Vector2Int tilePostion)
    {
        Debug.Log($"Before Clear {tilePostion} => {_grid[tilePostion].TileState}");
        _grid[tilePostion] = new Tile(tilePostion);
        Debug.Log($"After Clear {tilePostion} => {_grid[tilePostion].TileState}");
    }

    private int CheckMinMvmt(Vector2Int pos1, Vector2Int pos2)
    {
        int tilesCount = Mathf.Abs(pos1.x - pos2.x) + Mathf.Abs(pos1.y - pos2.y);
        Debug.Log($"TileCount Start-Exit = {tilesCount}");
        return tilesCount;
    }

    private Vector2Int GetFarEnoughExit()
    {
        Vector2Int possibleExit = AssignOneTilePosition(TileState.Exit);
        Debug.Log($"PossibleExit : ({possibleExit.x};{possibleExit.y})");
        if(CheckMinMvmt(startCoords, possibleExit) < _startExitOffset)
        {
            Debug.Log($"Ecart Start-Exit < {_startExitOffset}");
            ClearOneTile(possibleExit);
            possibleExit = GetFarEnoughExit();
        }
        Debug.Log($"Ecart Start-Exit > {_startExitOffset}");
        return possibleExit;
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

    //? Réunir les 2 fonctions en une qui retourne éventuellement une liste de 1 seul item ?
    private List<Vector2Int> AssignManyTilesPositions(int nbTiles, TileState tileState)
    {
        List<Vector2Int> tilesCoordList = new();
        for(int i = 0; i < nbTiles; i++)
        {
            Vector2Int tilePosition = PickNewAvailablePosition();
            Tile tile = new Tile(tilePosition, tileState);
            Debug.Log($"Tile intels = ({tile.Coord.x},{tile.Coord.y}) comme {tile.TileState}");
            //_grid.Remove(tilePosition);
            //_grid.Add(tile.Coord, tile);
            _grid[tile.Coord] = tile; //? Préferable ?
            tilesCoordList.Add(tile.Coord);
            Debug.Log($"Dans le Grid : {_grid[tile.Coord].TileState} {i + 1} : ({_grid[tile.Coord].Coord.x};{_grid[tile.Coord].Coord.y})");
        }
        return tilesCoordList;
    }

    private Vector2Int AssignOneTilePosition(TileState tileState)
    {
        Vector2Int tilePosition = PickNewAvailablePosition();
        Tile tile = new Tile(tilePosition, tileState);
        Debug.Log($"Tile intels = ({tile.Coord.x},{tile.Coord.y}) comme {tile.TileState}");
        //_grid.Remove(tilePosition);
        //_grid.Add(tile.Coord, tile);
        _grid[tile.Coord] = tile; //? Préferable ?
        Debug.Log($"Dans le Grid : {_grid[tile.Coord].TileState} : ({_grid[tile.Coord].Coord.x};{_grid[tile.Coord].Coord.y})");

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
            newPostion.x = Random.Range(1,(_gridSize.x-1));
            newPostion.y = Random.Range(1,(_gridSize.y-1));
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

    public Vector3 ConvertPositionGridToMap(Vector2Int gridPosition)
    {
        Vector3 mapPosition = new Vector3(gridPosition.x, 0, gridPosition.y) + mapAnchor.position;
        return mapPosition;
    }

    public Vector2Int ConvertPositionMapToGrid(Vector3 mapPosition)
    {
        mapPosition -= mapAnchor.transform.position;
        Vector2Int gridPosition = new Vector2Int((int)mapPosition.x, (int)mapPosition.z);
        return gridPosition;
    }
}
