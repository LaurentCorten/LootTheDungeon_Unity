using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public event Action<Vector2Int> OnPlayerMoved;

    public InputActionAsset inputActions;
    public InputActionMap inputActionMap;
    public GridManager gridManager;
    [SerializeField] GameManager gameManager;

    private InputAction _moveAction;
    private InputAction _lookAction;
    private Vector2 _moveDir;
    private Vector2 _lookDir;

    private void OnEnable()
    {
        inputActionMap = inputActions.FindActionMap("Player");
        inputActionMap.Enable();
    }
    private void OnDisable()
    {
        inputActionMap.Disable();
    }

    private void Awake()
    {
        _moveAction = inputActions.FindAction("Move");
        _moveAction.performed += OnMovePerformed;
        _moveAction.canceled += OnMoveCanceled;

        _lookAction = inputActions.FindAction("Look");
        _lookAction.performed += OnLookPerformed;
        _lookAction.canceled += OnLookCanceled;

        gameManager.OnStartNewLevel += HandleStartNewLevel;
    }

    private void OnDestroy()
    {
        _moveAction.performed -= OnMovePerformed;
        _moveAction.canceled -= OnMoveCanceled;
        _lookAction.performed -= OnLookPerformed;
        _lookAction.canceled -= OnLookCanceled;
        gameManager.OnStartNewLevel -= HandleStartNewLevel;
    }

    private void HandleStartNewLevel()
    {
            SetCanMove(true);
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
        
        _moveDir = _moveAction.ReadValue<Vector2>();
        if (_moveDir != Vector2.up && _moveDir != Vector2.left && _moveDir != Vector2.right && _moveDir != Vector2.down) return;
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
        //Debug.LogWarning($"moveDir.x = {_moveDir.x} -- moveDir.y = {_moveDir.y}" +
        //    $"\r\nHaut ? => {_moveDir == Vector2Int.up}" +
        //    $"\r\nBas ? => {_moveDir == Vector2Int.down}" +
        //    $"\r\nGauche ? => {_moveDir == Vector2Int.left}" +
        //    $"\r\ndroite ? => {_moveDir == Vector2Int.right}");
        
        Vector3 nextPosition3D = transform.position + transform.forward * _moveDir.y + transform.right * _moveDir.x;
        nextPosition3D = new Vector3(Mathf.Round(nextPosition3D.x), nextPosition3D.y, Mathf.Round(nextPosition3D.z));
        Vector2Int nextPosition2D = gridManager.ConvertPositionMapToGrid(nextPosition3D);
        
        //Debug.LogWarning($"On veut bouger de ({transform.position.x},{transform.position.z}) à ({nextPosition3D.x},{nextPosition3D.y}, {nextPosition3D.z}) => ({nextPosition2D.x},{nextPosition2D.y})");
        
        if (CheckWalkableTile(nextPosition2D))
        {
            transform.position = nextPosition3D;
            //Debug.LogWarning($"On bouge donc en ({transform.position.x},{transform.position.y},{transform.position.z})");
            OnPlayerMoved?.Invoke(nextPosition2D);
        }
    }

    private bool CheckWalkableTile(Vector2Int postionToCheck)
    {
        bool isOk = gridManager.Grid[postionToCheck].TileState != TileState.Obstacle;
        //Debug.LogWarning($"case walkable = {isOk}");
        return isOk;
    }

    public void SetCanMove(bool canMove)
    {
        switch(canMove)
        {
            case true:
                inputActionMap.Enable();
                break;
            case false:
                inputActionMap.Disable();
                break;
                
        }
    }
}
