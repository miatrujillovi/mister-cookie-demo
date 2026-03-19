using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    private PlayerControls playerInputs;

    //MOVEMENT
    private InputAction movement;
    private Vector2 moveInput;
    public Vector2 MoveInput => moveInput; //Public Getter

    private void Awake()
    {
        Instance = this;

        //Access the Player Action Map
        playerInputs = new PlayerControls();
        var gameplayActionMap = playerInputs.Player;

        //MOVEMENT
        movement = gameplayActionMap.Move;
    }

    private void OnEnable()
    {
        //MOVEMENT
        movement.Enable();
        movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        movement.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnDisable()
    {
        movement.Disable();
    }
}
