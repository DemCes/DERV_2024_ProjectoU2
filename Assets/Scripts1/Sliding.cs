using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(PlayerMovements))]
public class Sliding : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Transform playerObj;

    [Header("Sliding Parameters")]
    [SerializeField] private float maxSlideTime = 1f;
    [SerializeField] private float slideForce = 200f;
    [SerializeField] private float slideYScale = 0.5f;
    [SerializeField] private KeyCode slideKey = KeyCode.LeftControl;

    private Rigidbody rb;
    private PlayerMovements playerMovement;
    private float slideTimer;
    private float startYScale;
    private Vector3 originalScale;
    private float baseSlideForce;
    private Vector2 moveInput;

    private void Start()
    {
        if (!TryInitializeComponents()) return;
        InitializeValues();
        InitializeSlidingParameters();
    }

    private bool TryInitializeComponents()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovements>();

        if (!orientation || !playerObj || !rb || !playerMovement)
        {
            Debug.LogError($"Missing references in {GetType().Name}");
            enabled = false;
            return false;
        }
        return true;
    }

    private void InitializeValues()
    {
        startYScale = playerObj.localScale.y;
        originalScale = playerObj.localScale;
        baseSlideForce = slideForce;
    }

    private void InitializeSlidingParameters()
    {
        if (originalScale.y <= 0)
        {
            Debug.LogError("Invalid original scale");
            return;
        }

        slideYScale = originalScale.y * 0.5f; // Adjust slide height to 50% of original height
    }

    private void Update()
    {
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        HandleSlideInput();
    }

    private void HandleSlideInput()
    {
        if (Input.GetKeyDown(slideKey) && moveInput.sqrMagnitude > 0 && playerMovement.grounded)
        {
            StartSlide();
        }

        if (Input.GetKeyUp(slideKey) && playerMovement.sliding)
        {
            StopSliding();
        }
    }

    private void FixedUpdate() 
    {
        if (playerMovement.sliding)
        {
            SlidingMovement();
        }
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        if (!playerMovement.OnSlope() || rb.velocity.y > -0.1f) // Flat ground sliding
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
        }
        else // Sliding down a slope
        {
            rb.AddForce(playerMovement.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        slideTimer -= Time.deltaTime;
        if (slideTimer <= 0)
        {
            StopSliding();
        }
    }

    private void StartSlide()
    {
        playerMovement.sliding = true;
        playerMovement.state = PlayerMovements.MovementState.Sliding;

        // Adjust player scale for sliding
        float targetSlideHeight = slideYScale * (playerObj.localScale.y / originalScale.y);
        playerObj.localScale = new Vector3(
            playerObj.localScale.x,
            targetSlideHeight,
            playerObj.localScale.z
        );

        // Apply downward force to simulate sliding start
        float scaleFactor = Mathf.Sqrt(playerObj.localScale.y / originalScale.y);
        rb.AddForce(Vector3.down * 5f * scaleFactor, ForceMode.Impulse);

        // Set timer for how long the slide lasts
        slideTimer = maxSlideTime;

        // Adjust the speed for sliding
        playerMovement.desiredMoveSpeed = slideForce; // Modify this to control slide speed
    }

    private void StopSliding()
    {
        playerMovement.sliding = false;

        // Reset player scale back to normal
        playerObj.localScale = new Vector3(
            playerObj.localScale.x,
            startYScale,
            playerObj.localScale.z
        );

        // Reset the move speed back to normal based on movement state
        playerMovement.UpdateState();
    }

    public void OnScaleChanged()
    {
        // Recalculate sliding parameters when scale changes
        InitializeSlidingParameters();
    }
}
