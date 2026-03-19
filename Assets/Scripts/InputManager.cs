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

    private void Awake()
    {
        Instance = this;

        //Access the Player Action Map
        playerInputs = new PlayerControls();
        var gameplayActionMap = playerInputs.Player;

        //MOVEMENT
        movement = gameplayActionMap.Move;
        launch = gameplayActionMap.Launch;
    }

    private void OnEnable()
    {
        //MOVEMENT
        movement.Enable();
        movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        movement.canceled += ctx => moveInput = Vector2.zero;
        launch.Enable();
    }

    private void OnDisable()
    {
        movement.Disable();
        launch.Disable();
    }
}
