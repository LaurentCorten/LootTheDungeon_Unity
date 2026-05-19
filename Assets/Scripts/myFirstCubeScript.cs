using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class myFirstCubeScript : MonoBehaviour
{
    [SerializeField]
    TestInput _inputs;

    public Vector2 moveVector;
    GameObject myFirstCube;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputs = new TestInput();
        Debug.Log("Etat de Input; " + _inputs.Player.enabled);
        InputAssignation();
        myFirstCube = gameObject;
        EnableAction();
    }

    private void EnableAction()
    {
        Debug.Log("Etat de Input; "+_inputs.Player.enabled);
        _inputs?.Player.Enable();
        Debug.Log("Etat de Input; " + _inputs.Player.enabled);

    }

    // Update is called once per frame
    void Update()
    {
        ApplyMove();
    }

//  Transform _Tr = MyGO.GetComponent<Transform>();
//  _Tr.position += New Vector3(0,1,0)
//  _Tr.position = New Vector3(Tr.position.x,1, Tr.position.z)
//  _Tr.Translate(New Vector3(0,1,0))

    private void ApplyMove()
    {
        
    }

    public void InputAssignation()
    {
        _inputs.Player.Move.performed += Move_performed;
        _inputs.Player.Move.canceled += Move_canceled;
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        Vector2 inputVector = Vector2.zero ;
        moveVector = inputVector;
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        Vector2 inputVector = obj.ReadValue<Vector2>();
        moveVector = inputVector;
    }
}
