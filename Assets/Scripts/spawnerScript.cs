using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class spawnerScript : MonoBehaviour
{
    public GameObject target;
    public int nbTarget = 10;
    public float offset = 2.0f;

    List<Vector3> targetsPositions = new List<Vector3>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        for (int i = 0; i < nbTarget; i++)
        {
            Debug.Log($"i = {i}");
            SpawnTarget(); 
        }
        Debug.Log($"Fin des instanciations !");
    }

    public void SpawnTarget()
    {
        bool isPositionValid = false;
        Vector3 spawnPosition;

        // Randomly pick a new position til it's a valid one
        do
        {
            int spawnPointX = Random.Range(-15, 15);
            int spawnPointZ = Random.Range(-15, 15);

            spawnPosition = new Vector3(spawnPointX, target.transform.position.y, spawnPointZ);
            Debug.Log($"Tried spawnPosition is ({spawnPosition.x}, {spawnPosition.y}, {spawnPosition.z})");

            isPositionValid = CheckPositionValidity(spawnPosition);
            Debug.Log($"=> isPositionValid = {isPositionValid}");

        } while (!isPositionValid);

        // Actually spawn the target
        Instantiate(target, spawnPosition, Quaternion.identity);
        Debug.Log("Nouvelle target instanciée !");

        // Store the occupied position
        targetsPositions.Add(spawnPosition);
        Debug.Log($"Et targetPositions = {targetsPositions}");
    }

    // Needa chack if we're not to close from another target or the player
    private bool CheckPositionValidity(Vector3 spawnPosition)
    {
        // So we check if either x or z value is too close from any other already spawned target
        foreach (Vector3 target in targetsPositions)
        {
            Debug.Log($"test : {target} Vs. {spawnPosition}");
            if (Mathf.Abs(target.x - spawnPosition.x) < offset && Mathf.Abs(target.z - spawnPosition.z) < offset ) return false;
        }
        Debug.Log($"On va jusqu'au true !");
        return true;
    }
}
