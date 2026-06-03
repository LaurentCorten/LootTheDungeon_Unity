using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject ennemyPrefab;
    public GameObject player;
    public GameObject obstaclePrefab;
    public GameObject tilePrefab;
    public GameObject exitPrefab;

    public GridManager gridManager;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (KeyValuePair<Vector2Int, Tile> kvp in gridManager.Grid)
        {
            GameObject spawnObject;
            switch (kvp.Value.TileState)
            {
                case TileState.Obstacle:
                    spawnObject = obstaclePrefab;
                    break;
                case TileState.Encounter:
                    spawnObject = ennemyPrefab;
                    break;
                case TileState.Exit:
                    spawnObject = exitPrefab;
                    break;
                case TileState.Start:
                    spawnObject = player;
                    break;
                default:
                    spawnObject = null;
                    break;
            }
            
            Vector3 spawnPositionModifier = ConvertPositionGridToMap(kvp.Key);
            
            Instantiate(tilePrefab, tilePrefab.transform.position + spawnPositionModifier, tilePrefab.transform.rotation);
            if (spawnObject != null)
            {
                if (spawnObject == player)
                {
                    player.transform.Translate(spawnPositionModifier);
                }
                else
                {
                    Instantiate(spawnObject, spawnObject.transform.position + spawnPositionModifier, spawnObject.transform.rotation);
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 ConvertPositionGridToMap(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.y) + transform.position;
    }
}
