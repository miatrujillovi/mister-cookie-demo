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
    [SerializeField] private float airControl = 0.5f;
    private bool canJump = true;
    [Space]
    [Header("Dash")]
    [SerializeField] private float dashForce = 20f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private DashUI dashUI;
    private bool canDash = true;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;
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
    [SerializeField] private Animator animator;

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
        
        // Velocidad horizontal (sin contar Y)
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        float speed = flatVelocity.magnitude;

        // Mandar valores al Animator
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsGrounded", grounded);

        if (!canDash)
        {
            dashCooldownTimer += Time.deltaTime;

            float progress = Mathf.Clamp01(dashCooldownTimer / dashCooldown);
            progress = Mathf.SmoothStep(0, 1, progress);
            dashUI.UpdateDash(progress);

            if (dashCooldownTimer >= dashCooldown)
            {
                dashCooldownTimer = 0f;
                canDash = true;

                dashUI.Hide();
            }
        }
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

            dashCooldownTimer = 0f;
            dashUI.Show();
            dashUI.UpdateDash(0f);

            StartCoroutine(DashCoroutine());
            //Invoke(nameof(ResetDash), dashCooldown);
        }
    }

    private void HandleDrag()
    {
        if (isDashing)
            return;

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

        //Air control multiplier
        float controlMultiplier = grounded ? 1f : airControl;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f * controlMultiplier, ForceMode.Force);

        LimitSpeed();
    }

    private void LimitSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        float maxSpeed = moveSpeed;

        if (flatVelocity.magnitude > maxSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            rb.linearVelocity = new Vector3(limitedVelocity.x, rb.linearVelocity.y, limitedVelocity.z);
        }
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

        //Deactivates groundDrag
        float originalDrag = rb.linearDamping;
        rb.linearDamping = 0f;

        // Direccion del dash � si no hay input dashea hacia el frente
        Vector3 dashDir = moveDirection.normalized;
        if (dashDir == Vector3.zero)
            dashDir = orientation.forward;

        rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        rb.AddForce(dashDir * dashForce, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);

        //Activates groundDrag
        rb.linearDamping = originalDrag;
        isDashing = false;
    }

    /*private void ResetDash()
    {
        canDash = true;
    }*/

    private void OnEnable()
    {
        // Espera a que InputManager exista
        if (InputManager.Instance != null)
            InputManager.Instance.OnJump += TryJump;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.OnJump -= TryJump;
    }

    private void TryJump()
    {
        //Debug.Log("TryJump llamado | grounded: " + grounded + " | canJump: " + canJump);
        if (canJump && grounded)
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
}
