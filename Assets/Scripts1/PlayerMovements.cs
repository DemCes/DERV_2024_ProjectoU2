// PlayerMovement.cs
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovements : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float walkSpeed = 7f;
    [SerializeField] private float sprintSpeed = 10f;
    [SerializeField] private float slideSpeed = 10f;
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private float wallRunSpeed = 8f;
    [SerializeField] private float speedIncreaseMultiplier = 1.5f;
    //[SerializeField] private float slopeIncreaseMultiplier = 1.5f;

    [Header("Input Keys")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchSpeed = 3.5f;
    [SerializeField] private float crouchYScale = 0.5f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private float jumpCooldown = 0.25f;
    [SerializeField] private float airMultiplier = 0.4f;

    [Header("Ground Check")]
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private LayerMask whatIsGround;
    
    [Header("Slope Parameters")]
    [SerializeField] private float maxSlopeAngle = 45f;

    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Climbing climbingScript;

    // Private variables
    private Rigidbody rb;
    private float movementSpeed;
    public float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private float startYScale;
    private Vector3 originalScale;
    private bool readyToJump = true;
    public bool grounded;
    private bool exitingSlope;
    private Vector2 moveInput;
    private RaycastHit slopeHit;

    // Base values for scaling
    private float baseWalkSpeed;
    private float baseSprintSpeed;
    private float baseJumpForce;

    // State tracking
    public bool sliding { get; set; }
    public bool climbing { get; set; }
    public bool wallrunning { get; set; }

    public enum MovementState
    {
        Walking,
        Sprinting,
        Climbing,
        Wallrunning,
        Crouching,
        Sliding,
        Air
    }
    public MovementState state;

    private void Start()
    {
        if (!TryInitializeComponents())
            return;

        InitializeBaseValues();
        InitializeMovementParameters();
    }

    private bool TryInitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        if (!rb)
        {
            Debug.LogError("Rigidbody component missing!");
            enabled = false;
            return false;
        }

        if (!orientation)
        {
            Debug.LogError("Orientation reference missing!");       
            enabled = false;
            return false;
        }

        rb.freezeRotation = true;
        return true;
    }

    private void InitializeBaseValues()
    {
        originalScale = transform.localScale;
        startYScale = transform.localScale.y;
        baseWalkSpeed = walkSpeed;
        baseSprintSpeed = sprintSpeed;
        baseJumpForce = jumpForce;
    }

    private void InitializeMovementParameters()
    {
        float scaleFactorY = transform.localScale.y / originalScale.y;
        
        walkSpeed = baseWalkSpeed * Mathf.Sqrt(scaleFactorY);
        sprintSpeed = baseSprintSpeed * Mathf.Sqrt(scaleFactorY);
        jumpForce = baseJumpForce * scaleFactorY;
        
        rb.mass = 1f * scaleFactorY;
        playerHeight = startYScale * scaleFactorY;
        
        desiredMoveSpeed = walkSpeed;
        movementSpeed = walkSpeed;
        lastDesiredMoveSpeed = walkSpeed;
    }

    private void Update()
    {
        CheckGround();
        ProcessInput();
        UpdateState();
        SpeedControl();
        
        rb.drag = grounded ? groundDrag : 0f;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void CheckGround()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
    }

    private void ProcessInput()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            ChangeScale(0.5f);
        }

        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            ChangeScale(1.0f);
        }
    }

    public void UpdateState()
    {
        if (wallrunning)
        {
            state = MovementState.Wallrunning;
            desiredMoveSpeed = wallRunSpeed;
        }
        else if (climbing)
        {
            state = MovementState.Climbing;
            desiredMoveSpeed = climbSpeed;
        }
        else if (sliding)
        {
            state = MovementState.Sliding;
            desiredMoveSpeed = OnSlope() && rb.velocity.y < 0.1f ? slideSpeed : sprintSpeed;
        }
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.Crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.Sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.Walking;
            desiredMoveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.Air;
        }

        if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 4f && movementSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            movementSpeed = desiredMoveSpeed;
        }
        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private void MovePlayer()
    {
        if (climbingScript && climbingScript.exitingWall) return;

        Vector3 moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;
        moveDirection = moveDirection.normalized;

        float forceMultiplier = 10f * Mathf.Sqrt(transform.localScale.y / originalScale.y);

        if (OnSlope() && !exitingSlope)
        {
            Vector3 slopeDirection = GetSlopeMoveDirection(moveDirection);
            rb.AddForce(slopeDirection * movementSpeed * forceMultiplier, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                float downForce = 80f * (transform.localScale.y / originalScale.y);
                rb.AddForce(Vector3.down * downForce, ForceMode.Force);
            }
        }
        else if (grounded)
        {
            rb.AddForce(moveDirection * movementSpeed * forceMultiplier, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection * movementSpeed * airMultiplier * forceMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        float speedLimit = movementSpeed * Mathf.Sqrt(transform.localScale.y / originalScale.y);

        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > speedLimit)
                rb.velocity = rb.velocity.normalized * speedLimit;
        }
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (flatVelocity.magnitude > speedLimit)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * speedLimit;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - movementSpeed);
        float startValue = movementSpeed;

        while (time < difference)
        {
            movementSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);
                time += Time.deltaTime * speedIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }
        movementSpeed = desiredMoveSpeed;
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        float scaledJumpForce = jumpForce * Mathf.Sqrt(transform.localScale.y / originalScale.y);
        rb.AddForce(transform.up * scaledJumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }

    public void OnScaleChanged()
    {
        InitializeMovementParameters();
    }

    public void ChangeScale(float newScale)
    {
        if (newScale <= 0)
        {
            Debug.LogWarning("Invalid scale value");
            return;
        }

        transform.localScale = new Vector3(newScale, newScale, newScale);
        OnScaleChanged();

        Sliding sliding = GetComponent<Sliding>();
        if (sliding != null)
        {
            sliding.OnScaleChanged();
        }
    }
}