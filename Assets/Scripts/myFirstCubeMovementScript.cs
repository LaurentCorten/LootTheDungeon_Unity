using UnityEngine;
using UnityEngine.InputSystem;

public class myFirstCubeScript : MonoBehaviour
{
    [SerializeField]
    TestInput _inputs;

    public Vector2 moveVector;
    Transform _trans;

    public float velocity = 0.1f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputs = new TestInput();
        Debug.Log("Etat de Input; " + _inputs.Player.enabled);
        InputAssignation();
        EnableAction();
        _trans = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMove();
    }

    private void EnableAction()
    {
        Debug.Log("Etat de Input; "+_inputs.Player.enabled);
        _inputs?.Player.Enable();
        Debug.Log("Etat de Input; " + _inputs.Player.enabled);

    }

    private void ApplyMove()
    {
        _trans.position += new Vector3(moveVector.x,0,moveVector.y) * velocity;
        //_trans.position = new Vector3(_trans.position.x+moveVector.x, _trans.position.y, _trans.position.z+moveVector.y) * velocity;
        //_trans.Translate(new Vector3(moveVector.x, 0, moveVector.y)) * velocity;
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
