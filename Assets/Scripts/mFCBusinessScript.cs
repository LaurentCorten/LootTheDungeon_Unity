using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class mFCBusinessScript : MonoBehaviour
{
    [SerializeField]
    Color rareColor = Color.blue;
    [SerializeField]
    Color mediumColor = Color.crimson;
    [SerializeField]
    Color wellCookedColor = Color.brown;

    public TextMeshProUGUI textMeshProUGUI;

    public int _touch;
    MeshRenderer meshRend;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
        meshRend.material.color = rareColor;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.layer);
        Debug.Log(other.gameObject.tag);
        Debug.Log(other.name);

        if (other.gameObject.layer == 3) return;

        Debug.Log(textMeshProUGUI.GetParsedText());
        _touch += 1;
        textMeshProUGUI.text = $"{_touch}";
        Debug.Log(textMeshProUGUI.GetParsedText());
             
        if (_touch < 4) return;
        if (_touch < 7)
        {
            meshRend.material.color = mediumColor;
            return;
        }
        meshRend.material.color = wellCookedColor;
    }
}
