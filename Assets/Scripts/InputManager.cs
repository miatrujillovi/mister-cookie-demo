using UnityEngine;
using UnityEngine.InputSystem;

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
}
