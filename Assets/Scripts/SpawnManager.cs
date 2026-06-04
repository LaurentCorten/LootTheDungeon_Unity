using System.Collections.Generic;
using Unity.VisualScripting;
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
            
            Vector3 spawnPositionModifier = gridManager.ConvertPositionGridToMap(kvp.Key);
            
            Instantiate(tilePrefab, tilePrefab.transform.position + spawnPositionModifier, tilePrefab.transform.rotation);
            if (spawnObject != null)
            {
                if (spawnObject == player)
                {
                    Vector3 spawnPostion = new Vector3(0, player.transform.position.y, 0) + spawnPositionModifier;
                    player.transform.position = spawnPostion;
                }
                else
                {
                    Instantiate(spawnObject, spawnObject.transform.position + spawnPositionModifier, spawnObject.transform.rotation);
                }
            }
        }
    }
}
