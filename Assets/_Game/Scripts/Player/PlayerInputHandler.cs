using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    public string ActiveControlScheme => _input.currentControlScheme;

    public Vector2 AimDirection => _aimDirection;
    public bool UsingGamepad => _usingGamepad;
    public bool IsShooting => _isShooting;

    private PlayerControls _controls;
    private PlayerInput _input;

    private Vector2 _aimDirection;
    private Vector2 _gamepadDirection;
    private bool _isShooting;
    private bool _usingGamepad;

    private void Awake()
    {
        _input = GetComponent<PlayerInput>();

        _controls = new PlayerControls();
        _controls.Enable();

        _controls.Gameplay.Aim.performed += ctx => HandleAim(ctx);
        _controls.Gameplay.Aim.canceled += ctx => HandleAim(ctx);

        _controls.Gameplay.Shoot.performed += ctx => _isShooting = true;
        _controls.Gameplay.Shoot.canceled += ctx => _isShooting = false;
    }

    private void Update()
    {
        _usingGamepad = _input.currentControlScheme == "Gamepad";
        if (_usingGamepad && Gamepad.current != null)
        {
            _aimDirection = Gamepad.current.rightStick.value;
        }
    }

    private void HandleAim(InputAction.CallbackContext ctx)
    {
        _aimDirection = ctx.ReadValue<Vector2>();
    }

    private void OnDestroy()
    {
        _controls.Disable();
    }
}