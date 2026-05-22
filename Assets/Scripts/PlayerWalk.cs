using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalk : MonoBehaviour
{

    public InputActionAsset inputActions;

    private InputAction _moveAction;
    private InputAction _lookAction;
    //private InputAction _jumpAction;

    private Vector2 _moveAmt;
    private Vector2 _lookAmt;
    private Rigidbody _rb;

    public float WalkSpeed = 10;
    public float RotateSpeed = 66;
    public float JumpSpeed = 5;

    private void OnEnable()
    {
        inputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        inputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        _moveAction.performed += OnMovePerformed;
        _moveAction.canceled += OnMoveCanceled;

        _lookAction = InputSystem.actions.FindAction("Look");
        _lookAction.performed += OnLookPerformed;
        _lookAction.canceled += OnLookCanceled;

        //_jumpAction = InputSystem.actions.FindAction("Jump");

        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        //if (_jumpAction.WasPressedThisFrame()) {
        //    Jump();
        //}
    }

    private void FixedUpdate()
    {
        Walking();
        Rotating();
    }

    private void OnDestroy()
    {
        _moveAction.performed -= OnMovePerformed;
        _moveAction.canceled -= OnMoveCanceled;
        _lookAction.performed -= OnLookPerformed;        
        _lookAction.canceled -= OnLookCanceled;        
    }

    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        _lookAmt = _lookAction.ReadValue<Vector2>().normalized;
    }

    private void OnLookCanceled(InputAction.CallbackContext context)
    {
        _lookAmt = Vector2.zero;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        _moveAmt = _moveAction.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveAmt = Vector2.zero;
    }

    private void Rotating()
    {
        float rotationAmt = _lookAmt.x * RotateSpeed * Time.deltaTime;
        Quaternion deltaRotation = Quaternion.Euler(0, rotationAmt, 0);
        _rb.MoveRotation(_rb.rotation *  deltaRotation);
    }

    private void Walking()
    {
        _rb.MovePosition(_rb.position + transform.forward * _moveAmt.y * WalkSpeed * Time.deltaTime);
        _rb.MovePosition(_rb.position + transform.right * _moveAmt.x * WalkSpeed * Time.deltaTime);
    }

    //private void Jump()
    //{
    //    _rb.AddForceAtPosition(new Vector3(0, 5f, 0), Vector3.up, ForceMode.Impulse);
    //}
}
