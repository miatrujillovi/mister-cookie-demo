using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform player;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Rigidbody rb;
    [Space]
    [SerializeField] private float rotationSpeed;

    private void Start()
    {
        /*Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    private void Update()
    {
        RotateOrientation();
        RotatePlayerBody();
    }

    private void RotateOrientation()
    {
        Vector3 forward = Camera.main.transform.forward;
        forward.y = 0f;
        forward.Normalize();

        orientation.forward = forward;
    }

    private void RotatePlayerBody()
    {
        Vector2 input = InputManager.Instance.MoveInput;
        float horizontalInput = input.y; //Left-Right
        float verticalInput = -input.x; //Forward-Back

        Vector3 inputDir = (orientation.forward * verticalInput) + (orientation.right * horizontalInput);

        if (inputDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(inputDir.normalized);

            playerBody.rotation = Quaternion.Slerp(playerBody.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
