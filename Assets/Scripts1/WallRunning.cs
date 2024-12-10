using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("WallRunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Deteccion")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Referencias")]
    public Transform orientacion;
    private PlayerMovements playerMovements;
    private Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        playerMovements = GetComponent<PlayerMovements>();
    }
    private void Update() {
        CheckForWall();
        StateMachine();
    }
    private void FixedUpdate() {
        if (playerMovements.wallrunning)
            WallRunningMovement();
    }
    
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position,orientacion.right,out rightWallhit,wallCheckDistance,whatIsWall);
        wallLeft = Physics.Raycast(transform.position,-orientacion.right,out leftWallhit,wallCheckDistance,whatIsWall);

    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position,Vector3.down,minJumpHeight,whatIsGround);
    }

    private void StateMachine()
    {
        //******Getting inputs*****////
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        //********Estado 1 - WallRunning************////
        if((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
        {
            if(!playerMovements.wallrunning)
                StartWallRun();
        }
        //***** Estado 3 - None
        else
        {
            if(playerMovements.wallrunning)
                StopWallRun();
        }
    }
    private void StartWallRun()
    {
        playerMovements.wallrunning = true;
    }
    private void WallRunningMovement()
    {
        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);
        Vector3 wallnormal = wallRight ? rightWallhit.normal : leftWallhit.normal;
        Vector3 wallForward = Vector3.Cross(wallnormal,transform.up);

        if((orientacion.forward - wallForward).magnitude > (orientacion.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        //**** Forward force****
        rb.AddForce(wallForward*wallRunForce,ForceMode.Force);

        //***** Subir-Bajar Force*******/////
        if(upwardsRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x,wallClimbSpeed,rb.velocity.z);
        }
        if(downwardsRunning)
        {
            rb.velocity = new Vector3(rb.velocity.x,-wallClimbSpeed,rb.velocity.z);
        }

        //***** push wall force*****////
        if(!(wallLeft && horizontalInput > 0)&& !(wallRight && horizontalInput <0))
            rb.AddForce(-wallnormal*100,ForceMode.Force);

    }
    private void StopWallRun()
    {
        playerMovements.wallrunning = false;
    }
}
