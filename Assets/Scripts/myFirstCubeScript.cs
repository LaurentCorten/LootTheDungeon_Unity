using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class myFirstCubeScript : MonoBehaviour
{
    [SerializeField]
    TestInput _inputs;

    public Vector2 moveVector;
    GameObject myFirstCube;
    Transform _trans;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputs = new TestInput();
        Debug.Log("Etat de Input; " + _inputs.Player.enabled);
        InputAssignation();
        myFirstCube = gameObject;
        EnableAction();
        _trans = myFirstCube.GetComponent<Transform>();
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
        _trans.position += new Vector3(moveVector.x,0,moveVector.y);
        //_trans.position = new Vector3(_trans.position.x+moveVector.x, _trans.position.y, _trans.position.z+moveVector.y);
        //_trans.Translate(new Vector3(moveVector.x, 0, moveVector.y));
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
