using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject ennemyPrefab;
    public GameObject playerPrefab;
    public GameObject obstaclePrefab;
    public GameObject tilePrefab;
    public GameObject exitPrefab;

    public GridManager gridManager;

    private void Awake()
    {
        
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
                    spawnObject = playerPrefab;
                    break;
                default:
                    spawnObject = null;
                    break;
            }
            
            Vector3 spawnPositionModifier = ConvertPositionGridToMap(kvp.Key)+transform.position;
            
            Instantiate(tilePrefab, tilePrefab.transform.position + spawnPositionModifier, tilePrefab.transform.rotation);
            if (spawnObject != null)
            {
                Instantiate(spawnObject, spawnObject.transform.position + spawnPositionModifier, spawnObject.transform.rotation);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 ConvertPositionGridToMap(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.y);
    }
}
