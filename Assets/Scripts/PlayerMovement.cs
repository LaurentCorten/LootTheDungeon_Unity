using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public event Action<Vector2Int> OnPlayerMoved;
    public InputActionAsset inputActions;
    public GridManager gridManager;

    private InputAction _moveAction;
    private InputAction _lookAction;

    private Vector2 _moveDir;
    private Vector2 _lookDir;



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
        _moveAction = inputActions.FindAction("Move");
        _moveAction.performed += OnMovePerformed;
        _moveAction.canceled += OnMoveCanceled;

        _lookAction = inputActions.FindAction("Look");
        _lookAction.performed += OnLookPerformed;
        _lookAction.canceled += OnLookCanceled;
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
    }

    private void Moving()
    {
        Vector3 nextPosition = transform.position + transform.forward * _moveDir.y + transform.right * _moveDir.x;
        Vector2Int newPosition = gridManager.ConvertPositionMapToGrid(nextPosition);
        if(CheckWalkableTile(newPosition))
        { 
            transform.position = nextPosition;
            OnPlayerMoved?.Invoke(newPosition);
        }
    }

    private bool CheckWalkableTile(Vector2Int postionToCheck)
    {
        bool isOk = gridManager.Grid[postionToCheck].TileState != TileState.Obstacle;
        return isOk;
    }
}
