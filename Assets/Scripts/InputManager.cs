using UnityEngine;
using UnityEngine.InputSystem;
using System;
public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerControls playerInputs;

    //MOVEMENT
    private InputAction movement;
    private Vector2 moveInput;

    // LAUNCH
    private InputAction launch;
    public bool LaunchPressed => launch.IsPressed();
    public bool LaunchReleased => launch.WasReleasedThisFrame();

    //JUMP / DASH
    private InputAction jump;
    public event Action OnJump;
    private InputAction dash;
    public bool DashPressed => dash.WasPressedThisFrame();

    public Vector2 MoveInput => moveInput; //Public Getter

    //SELECTION MENU - HOLD
    private InputAction selectionMenu;
    private bool selection;
    public bool Selection => selection; //Public Getter


    private void Awake()
    {
        Instance = this;

        //Access the Player Action Map
        playerInputs = new PlayerControls();
        var gameplayActionMap = playerInputs.Player;

        //MOVEMENT
        movement = gameplayActionMap.Move;

        //SELECTION MENU - HOLD
        selectionMenu = gameplayActionMap.Selection;

        launch = gameplayActionMap.Launch;

        jump = gameplayActionMap.Jump;
        dash = gameplayActionMap.Dash;
    }

    private void OnEnable()
    {
        //MOVEMENT
        movement.Enable();
        movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        movement.canceled += ctx => moveInput = Vector2.zero;

        //SELECTION MENU - HOLD
        selectionMenu.Enable();
        selectionMenu.started += ctx => StartSelection();
        selectionMenu.canceled += ctx => StopSelection();

        launch.Enable();

        jump.Enable();
        jump.performed += HandleJump;
        dash.Enable();
    }

    private void OnDisable()
    {
        //MOVEMENT
        movement.Disable();

        //SELECTION MENU - HOLD
        selectionMenu.started -= ctx => StartSelection();
        selectionMenu.canceled -= ctx => StopSelection();
        selectionMenu.Disable();

        launch.Disable();

        jump.Disable();
        jump.performed -= HandleJump;
        dash.Disable();
    }

        //SELECTION MENU - HOLD
    private void StartSelection()
    {
        selection = true;
    }

    private void StopSelection()
    {
        selection = false;
    }
    private void HandleJump(InputAction.CallbackContext ctx)
    {
        Debug.Log("HandleJump disparado");
        OnJump?.Invoke();
    }
}
