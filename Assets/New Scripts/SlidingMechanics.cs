using UnityEngine;
using System.Collections;

public class SlideMechanics : MonoBehaviour
{
    [Header("Slide Settings")]
    public float slideSpeed = 12f;
    public float slideSpeedMultiplier = 1.5f;
    public float slideForce = 400f;
    public float slideDrag = 0.2f;
    public float minSlideSpeed = 2f;
    public float maxSlideTime = 1f;
    public float slopeSlidePower = 1.5f;
    
    [Header("Slide Controls")]
    public KeyCode slideKey = KeyCode.LeftControl;
    public float slideYScale = 0.5f;
    public float slideTransitionSpeed = 10f;
    
    [Header("Slope Detection")]
    public float maxSlideAngle = 60f;
    public float minSlideAngle = 5f;
    public float groundCheckDistance = 0.2f;
    
    [Header("Camera Effects")]
    public float slideFOVIncrease = 10f;
    public float FOVTransitionSpeed = 10f;
    public float cameraSlideYOffset = -0.5f; // How far down the camera moves when sliding
    
    [Header("References")]
    public Transform orientation;
    public Camera playerCamera;
    public Transform cameraHolder; // Reference to the camera's parent/holder
    
    // References
    private Rigidbody rb;
    private AdvancedMovements moveScript;
    private CapsuleCollider capsuleCollider;
    
    // Original states
    private float originalHeight;
    private float originalYScale;
    private float originalFOV;
    private Vector3 originalCameraPosition;
    private Vector3 originalLocalScale;
    
    // Slide state
    private bool isSliding;
    private float slideTimer;
    private Vector3 slideDirection;
    private bool wasSliding;
    private bool isTransitioning;
    
    // Public property to check slide state
    public bool IsSliding => isSliding;

    private void Start()
    {
        // Get component references
        rb = GetComponent<Rigidbody>();
        moveScript = GetComponent<AdvancedMovements>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        
        // If camera reference is not set, try to find it
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null)
            {
                Debug.LogError("No camera assigned to SlideMechanics and couldn't find main camera!");
            }
        }

        // Try to find camera holder if not assigned
        if (cameraHolder == null && playerCamera != null)
        {
            cameraHolder = playerCamera.transform.parent;
        }
        
        // If orientation is not set, try to get it from moveScript
        if (orientation == null && moveScript != null)
        {
            orientation = moveScript.orientation;
        }

        // Store original values
        originalHeight = capsuleCollider.height;
        originalYScale = transform.localScale.y;
        originalLocalScale = transform.localScale;
        if (playerCamera != null && cameraHolder != null)
        {
            originalFOV = playerCamera.fieldOfView;
            originalCameraPosition = cameraHolder.localPosition;
        }
    }

    private void Update()
    {
        CheckSlideInput();
        HandleSlideState();
        if (playerCamera != null)
        {
            HandleCameraEffects();
        }
    }

    private void FixedUpdate()
    {
        if (isSliding)
        {
            HandleSlideMovement();
        }
    }

    private void CheckSlideInput()
    {
        if (!isTransitioning)
        {
            if (Input.GetKeyDown(slideKey) && moveScript.IsGrounded && !isSliding && rb.velocity.magnitude > minSlideSpeed)
            {
                StartSlide();
            }
            
            if (Input.GetKeyUp(slideKey) && isSliding)
            {
                StopSlide();
            }
        }
    }

    private void StartSlide()
    {
        if (isTransitioning) return;
        
        isSliding = true;
        slideTimer = maxSlideTime;
        wasSliding = false;

        slideDirection = orientation != null ? orientation.forward : transform.forward;
        rb.AddForce(slideDirection * slideForce, ForceMode.Impulse);
        
        StartCoroutine(SmoothlyChangeScale(slideYScale));
        StartCoroutine(SmoothlyMoveCamera(true));
        
        rb.drag = slideDrag;
    }

    private void StopSlide()
    {
        if (!isSliding || isTransitioning) return;
        
        isSliding = false;
        wasSliding = true;
        
        StartCoroutine(SmoothlyChangeScale(originalYScale));
        StartCoroutine(SmoothlyMoveCamera(false));
        
        rb.drag = moveScript.groundDrag;
    }

    private IEnumerator SmoothlyChangeScale(float targetYScale)
    {
        isTransitioning = true;
        
        Vector3 initialScale = transform.localScale;
        Vector3 targetScale = new Vector3(originalLocalScale.x, targetYScale, originalLocalScale.z);
        float initialHeight = capsuleCollider.height;
        float targetHeight = originalHeight * (targetYScale / originalYScale);
        
        float elapsedTime = 0f;
        
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * slideTransitionSpeed;
            float t = Mathf.Clamp01(elapsedTime);
            
            transform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            capsuleCollider.height = Mathf.Lerp(initialHeight, targetHeight, t);
            capsuleCollider.center = new Vector3(0, capsuleCollider.height / 2f, 0);
            
            yield return null;
        }
        
        // Ensure we reach exact target values
        transform.localScale = targetScale;
        capsuleCollider.height = targetHeight;
        capsuleCollider.center = new Vector3(0, capsuleCollider.height / 2f, 0);
        
        isTransitioning = false;
    }

    private IEnumerator SmoothlyMoveCamera(bool toSlidePosition)
    {
        if (cameraHolder == null) yield break;

        Vector3 startPos = cameraHolder.localPosition;
        Vector3 targetPos = toSlidePosition ? 
            originalCameraPosition + new Vector3(0, cameraSlideYOffset, 0) : 
            originalCameraPosition;

        float elapsedTime = 0f;
        
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * slideTransitionSpeed;
            float t = Mathf.Clamp01(elapsedTime);
            
            cameraHolder.localPosition = Vector3.Lerp(startPos, targetPos, t);
            
            yield return null;
        }
        
        // Ensure we reach exact target position
        cameraHolder.localPosition = targetPos;
    }

    private void HandleSlideState()
    {
        if (isSliding)
        {
            slideTimer -= Time.deltaTime;
            
            if (slideTimer <= 0 || rb.velocity.magnitude < minSlideSpeed)
            {
                StopSlide();
            }
            
            CheckSlideObstacles();
        }
    }

    private void HandleSlideMovement()
    {
        if (orientation == null) return;
        
        RaycastHit hit;
        bool isOnSlope = Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance);
        
        if (isOnSlope)
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            float currentSlideSpeed = slideSpeed;
            
            if (slopeAngle > minSlideAngle && slopeAngle < maxSlideAngle)
            {
                Vector3 slopeDirection = Vector3.ProjectOnPlane(slideDirection, hit.normal).normalized;
                currentSlideSpeed *= slideSpeedMultiplier;
                
                float slopeFactor = Mathf.Pow(slopeAngle / maxSlideAngle, slopeSlidePower);
                rb.AddForce(slopeDirection * currentSlideSpeed * slopeFactor, ForceMode.Force);
            }
            
            rb.AddForce(slideDirection * currentSlideSpeed, ForceMode.Force);
        }
        
        if (rb.velocity.magnitude > slideSpeed * slideSpeedMultiplier)
        {
            rb.velocity = rb.velocity.normalized * slideSpeed * slideSpeedMultiplier;
        }
    }

    private void HandleCameraEffects()
    {
        if (playerCamera == null) return;
        
        float targetFOV = isSliding ? originalFOV + slideFOVIncrease : originalFOV;
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * FOVTransitionSpeed);
    }

    private void CheckSlideObstacles()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, slideDirection, out hit, capsuleCollider.radius + 0.2f))
        {
            StopSlide();
        }
    }

    public void ForceStopSlide()
    {
        if (isSliding)
        {
            StopSlide();
        }
    }

    private void OnDisable()
    {
        // Ensure everything is reset when the script is disabled
        if (isSliding)
        {
            StopSlide();
        }
        
        // Force reset important values
        if (cameraHolder != null)
        {
            cameraHolder.localPosition = originalCameraPosition;
        }
        transform.localScale = originalLocalScale;
        if (capsuleCollider != null)
        {
            capsuleCollider.height = originalHeight;
            capsuleCollider.center = new Vector3(0, originalHeight / 2f, 0);
        }
    }
}