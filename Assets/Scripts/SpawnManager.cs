using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject ennemyPrefab;
    [SerializeField] GameObject obstaclePrefab;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] GameObject exit;
    [SerializeField] GameObject player;

    [SerializeField] GridManager gridManager;
    [SerializeField] Transform mapAnchor;

    public Dictionary<Tile, EnemyState> _tempLink = new Dictionary<Tile, EnemyState>();

    private void Awake()
    {
        gridManager.OnGridCreated += HandleGridCreated;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void HandleGridCreated()
    {
        ClearGridObjects();
        SpawnGridObjects();
        StartCoroutine(LinkEnemiesToTiles());
    }

    private void SpawnGridObjects()
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
                    spawnObject = exit;
                    break;
                case TileState.Start:
                    spawnObject = player;
                    break;
                default:
                    spawnObject = null;
                    break;
            }

            Vector3 spawnPositionModifier = gridManager.ConvertPositionGridToMap(kvp.Key);

            GameObject newTile = Instantiate(tilePrefab, tilePrefab.transform.position + spawnPositionModifier, tilePrefab.transform.rotation, mapAnchor);

            if (spawnObject != null)
            {
                if (spawnObject == player || spawnObject == exit)
                {
                    Vector3 spawnPostion = new Vector3(0, player.transform.position.y, 0) + spawnPositionModifier;
                    spawnObject.transform.position = spawnPostion;
                }
                else
                {
                    GameObject newObject = Instantiate(spawnObject, spawnObject.transform.position + spawnPositionModifier, spawnObject.transform.rotation);
                    newObject.transform.SetParent(newTile.transform, true);

                    if (kvp.Value.TileState == TileState.Encounter)
                    {
                        _tempLink.Add(kvp.Value, newObject.GetComponent<EnemyState>());
                    }
                }
            }
        }
    }

    private IEnumerator LinkEnemiesToTiles()
    {
        yield return new WaitForEndOfFrame();

        foreach (var kvp in _tempLink)
        {
           kvp.Key.SetEnemy(kvp.Value);
        }
    }

    private void ClearGridObjects()
    { 
        foreach(Transform child in mapAnchor.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
