using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public InputActionAsset inputActions;

    private InputAction _moveAction;
    private InputAction _lookAction;

    private Vector2 _moveDir;
    private Vector2 _lookDir;

    private Rigidbody _rb;


    private void OnEnable()
    {
    }
    private void OnDisable()
    {
    }

    private void Awake()
    {
        inputActions.FindActionMap("Player").Enable();

        _moveAction = inputActions.FindAction("Move");
        _moveAction.performed += OnMovePerformed;
        _moveAction.canceled += OnMoveCanceled;

        _lookAction = inputActions.FindAction("Look");
        _lookAction.performed += OnLookPerformed;
        _lookAction.canceled += OnLookCanceled;

        _rb = GetComponent<Rigidbody>();
    }

    private void OnDestroy()
    {
        _moveAction.performed -= OnMovePerformed;
        _moveAction.canceled -= OnMoveCanceled;
        _lookAction.performed -= OnLookPerformed;
        _lookAction.canceled -= OnLookCanceled;
        inputActions.FindActionMap("Player").Disable();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
    }
        
    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        _lookDir = _lookAction.ReadValue<Vector2>().normalized;
        Rotating();
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        _lookDir = Vector2.zero;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _moveDir = _moveAction.ReadValue<Vector2>().normalized;
        Moving();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveDir = Vector2.zero;
    }

    private void Rotating()
    {
        float rotationAmt = _lookDir.x * 90;
        Quaternion deltaRotation = Quaternion.Euler(0, rotationAmt, 0);
        gameObject.transform.rotation = gameObject.transform.rotation * deltaRotation;
        //_rb.MoveRotation(_rb.rotation * deltaRotation);
    }

    private void Moving()
    {
        gameObject.transform.position += transform.forward * _moveDir.y + transform.right * _moveDir.x;
        //_rb.MovePosition(_rb.position + transform.forward * _moveDir.y);
        //_rb.MovePosition(_rb.position + transform.right * _moveDir.x);
    }
}
