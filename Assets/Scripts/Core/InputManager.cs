using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
internal class InputManager : MonoBehaviour
{
  internal static InputManager Instance; 
  private PlayerInput _playerInput;

  internal Vector2 MoveInput { get; private set; }
  internal static event Action OnJumpPressed;
  internal static event Action OnPausePressed;


  internal void Awake()
  {
    if (Instance != null)
    {
      Destroy(gameObject);
      return;
    }

    Instance = this;
    transform.parent = null;
    DontDestroyOnLoad(gameObject);
    
    _playerInput = GetComponent<PlayerInput>();
    _playerInput.actions.Disable();

  }

  internal void SwitchMap(string mapName)
  {
    if (_playerInput != null)
      _playerInput.SwitchCurrentActionMap(mapName);
  }

  public bool IsJumpHeld => _playerInput.actions["Jump"].IsPressed();

  /* Callbacks del PlayerInput (vía Send Messages)*/
  internal void OnMove(InputValue value)
  {
    MoveInput = value.Get<Vector2>();
  }

  internal void OnJump(InputValue value)
  {
    if (value.isPressed)
      OnJumpPressed?.Invoke();
  }

  internal void OnPause(InputValue value)
    {
      if (value.isPressed)
        OnPausePressed?.Invoke();
    }
}