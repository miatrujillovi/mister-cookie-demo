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

    //HOLD CTRL FOR SELECTION MENU
    private InputAction selectionMenu;
    private bool selection;
    public bool Selection => selection; //Public Getter

    private bool menuActive;


    private void Awake()
    {
        Instance = this;

        //Access the Player Action Map
        playerInputs = new PlayerControls();
        var gameplayActionMap = playerInputs.Player;

        //MOVEMENT
        movement = gameplayActionMap.Move;

        //HOLD CTRL FOR SELECTION MENU
        selectionMenu = gameplayActionMap.Selection;
    }

    private void OnEnable()
    {
        //MOVEMENT
        movement.Enable();
        movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        movement.canceled += ctx => moveInput = Vector2.zero;

        //HOLD CTRL FOR SELECTION MENU
        selectionMenu.Enable();
        //selectionMenu.performed += ctx => ActivateSelection;
        //selectionMenu.canceled += ctx => DeactivateSelection;
    }

    private void OnDisable()
    {
        movement.Disable();
    }
}
