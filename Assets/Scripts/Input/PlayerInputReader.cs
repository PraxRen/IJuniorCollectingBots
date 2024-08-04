using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, PlayerInput.IPlayerActions
{
    private PlayerInput _playerInput;

    public event Action ActivetedCursor;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _playerInput.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        _playerInput.Player.Enable();
    }

    private void OnDisable()
    {
        _playerInput.Player.Disable();
    }

    void PlayerInput.IPlayerActions.OnActiveCursor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ActivetedCursor?.Invoke();
        }
    }
}