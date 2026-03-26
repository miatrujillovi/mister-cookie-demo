using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float groundDrag;
    [Space]
    [Header("Jump")]
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float jumpCooldown = 0.25f;
    private bool canJump = true;
    [Space]
    [Header("Dash")]
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing = false;
    [Space]
    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask Ground;
    [Space]
    [Header("Player Orientation")]
    [SerializeField] private Transform orientation;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    private bool grounded;

    public float RayMedition;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        InputManager.Instance.OnJump += TryJump;
    }

    private void Update()
    {
        Vector3 rayOrigin = transform.position + Vector3.down * RayMedition;
        grounded = Physics.Raycast(rayOrigin, Vector3.down, 0.3f, Ground);
        Debug.DrawRay(rayOrigin, Vector3.down * 0.3f, grounded ? Color.green : Color.red);
        MyInput();
        HandleDrag();
    }

    private void FixedUpdate()
    {
        if (!isDashing)
            MovePlayer();
    }

    private void MyInput()
    {
        Vector2 input = InputManager.Instance.MoveInput;

        horizontalInput = input.x;
        verticalInput = input.y;

        // Dash
        if (InputManager.Instance.DashPressed && canDash)
        {
            canDash = false;
            StartCoroutine(DashCoroutine());
            Invoke(nameof(ResetDash), dashCooldown);
        }
    }

    private void HandleDrag()
    {
        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }
    }

    private void MovePlayer()
    {
        //Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        Vector3 velocity = rb.linearVelocity;
        Vector3 targetVelocity = moveDirection.normalized * moveSpeed;

        rb.linearVelocity = new Vector3(targetVelocity.x, velocity.y, targetVelocity.z);
    }

    private void Jump()
    {
        // Resetea velocidad Y antes de saltar para saltos consistentes
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;

        // Direccion del dash — si no hay input dashea hacia el frente
        Vector3 dashDir = moveDirection.normalized;
        if (dashDir == Vector3.zero)
            dashDir = orientation.forward;

        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        rb.AddForce(dashDir * dashForce, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
    }

    private void ResetDash()
    {
        canDash = true;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnJump -= TryJump;
    }

    private void TryJump()
    {
        Debug.Log("TryJump llamado | grounded: " + grounded + " | canJump: " + canJump);
        if (canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
}
