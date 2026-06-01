using UnityEngine;


public class targetScript : MonoBehaviour
{
    [SerializeField]
    Color baseColor = Color.yellow;
    [SerializeField]
    Color triggerColor = Color.red;

    MeshRenderer meshRend;
    Collider col;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRend = GetComponent<MeshRenderer>();
        meshRend.material.color = baseColor;
        col= GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Qq chose est entré !");
        meshRend.material.color = triggerColor;
        //other.GetComponent<Renderer>().material.color = baseColor;

    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
