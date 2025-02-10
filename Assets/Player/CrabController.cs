using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CrabMovement : MonoBehaviour
{
    [Header("Movement Speeds")]
    [Tooltip("Forward/back movement speed.")]
    public float forwardSpeed = 3f;

    [Tooltip("Left/right (crab) movement speed.")]
    public float sideSpeed = 10f;

    [Header("Jump")]
    [Tooltip("Upward velocity applied when jumping.")]
    public float jumpForce = 7f;

    [Header("Camera Reference")]
    [Tooltip("Assign the main camera's transform here for camera-relative movement.")]
    public Transform cameraTransform;

    // Internal
    private Rigidbody rb;
    private Vector2 inputDir;
    private bool jumpRequested = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Use Unity's built-in gravity
        rb.useGravity = true;
        // Prevent the crab from tipping/rolling
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    // Called by new Input System "Move" action
    public void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>(); // (x, y) from WASD or joystick
    }

    // Called by new Input System "Jump" action
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpRequested = true;
        }
    }

    private void FixedUpdate()
    {
        bool grounded = IsGrounded();

        // Break down camera directions (only horizontal)
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // Desired horizontal velocity
        float moveForward = inputDir.y * forwardSpeed;
        float moveSide = inputDir.x * sideSpeed;
        Vector3 desiredHorizontalVel = camForward * moveForward + camRight * moveSide;

        // Keep the existing Y velocity (gravity / jump)
        float currentY = rb.linearVelocity.y;

        // Combine into final velocity
        Vector3 newVelocity = desiredHorizontalVel;
        newVelocity.y = currentY;

        rb.linearVelocity = newVelocity;

        // Handle Jump
        if (jumpRequested && grounded)
        {
            // Overwrite vertical velocity with jump force
            newVelocity.y = jumpForce;
            rb.linearVelocity = newVelocity;
        }

        jumpRequested = false; // reset
    }

    private bool IsGrounded()
    {
        // Simple ground check via Raycast
        float checkDistance = 1.1f;
        return Physics.Raycast(transform.position, Vector3.down, checkDistance);
    }
}
