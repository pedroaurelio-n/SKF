using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputManager : MonoBehaviour
{
    public string ActiveControlScheme => _input.currentControlScheme;

    PlayerCharacterController _characterController;
    PlayerInput _input;
    
    PlayerControls _controls;
    
    void Awake ()
    {
        _characterController = GetComponentInParent<PlayerCharacterController>();
        _input = GetComponent<PlayerInput>();
        
        _controls = new PlayerControls();
        _controls.Enable();
    }

    void Update ()
    {
        PlayerCharacterInputs inputs = new();

        Vector2 movement = _controls.Gameplay.Move.ReadValue<Vector2>();
        inputs.MoveRightAxis = movement.x;
        
        inputs.JumpPressed = _controls.Gameplay.Jump.triggered;

        _characterController.SetInputs(ref inputs);
    }

    void OnDestroy ()
    {
        _controls.Disable();
    }
}
