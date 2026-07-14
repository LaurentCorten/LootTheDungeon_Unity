using TMPro;
using UnityEngine;

[ExecuteAlways]
public class TileLabeller : MonoBehaviour
{
    TextMeshPro label;
    Vector2Int coords = new Vector2Int();
    GridManager gridManager;

    private void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>(); //? Qu'est-ce que ça change concrètement de faire ça ou de [SerializeField] l'objet et drag an drop dans unity ?
        label = GetComponentInChildren<TextMeshPro>();
        //label.enabled = false; // À activer en prod
        DisplayCoords();
    }

    private void Update()
    {
        DisplayCoords();
        transform.name = "Tile " + coords.ToString();
    }

    private void DisplayCoords()
    {
        if(!gridManager) { return; }
        coords.x = Mathf.RoundToInt(transform.position.x / gridManager.UnityGridSize);
        coords.y = Mathf.RoundToInt(transform.position.z / gridManager.UnityGridSize);

        label.text = $"{coords.x}, {coords.y}";
    }

}
