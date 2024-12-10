using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{

    [Header("Referencias")]
    public Transform orientacion;
    public Rigidbody rb;
    public PlayerMovements playerMovements;
    public LayerMask queEsPared;


    [Header("Climbing")]
    public float climbspeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("Wall Jumping")]
    public float climbJumpUpForce;
    public float climbJumpBackForce;
    public KeyCode jumpkey = KeyCode.Space;
    public int climbJumps;
    private int climbJumpsLeft;

    [Header("Deteccion")]
    public float deteccionlength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool isWallFront;

    private  Transform lastWall;
    private Vector3 lastWallNormal;
    public float minWallNormalAngleChange;

    [Header("Saliendo Pared")]
    public bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;


private void Update()
{
    WallCheck();
    StateMachine();

    if(climbing && !exitingWall) ClimbingMovement();
}
private void StateMachine()
{
    //****** Estado 1 - CLimbing****//
    if(isWallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle && !exitingWall)
    {
        if(!climbing && climbTimer > 0) StartCLimbing();
        //*****Timer////
        if(climbTimer > 0) climbTimer -= Time.deltaTime;
        if(climbTimer < 0) StopCLimbing();
    }   
    //**** Estado 2 - Saliendo *******/////
    else if(exitingWall)
    {
        if(climbing) StopCLimbing();

        if(exitWallTimer > 0) exitWallTimer -= Time.deltaTime;

        if(exitWallTimer<0) exitingWall = false;
    }

    //****** Estado 3 - None*****////
    else
    {
        if(climbing) StopCLimbing();
    }
    if(isWallFront && Input.GetKeyDown(jumpkey) && climbJumpsLeft > 0) ClimbJump();
}
    private void WallCheck()
    {
        isWallFront = Physics.SphereCast(transform.position,sphereCastRadius,orientacion.forward,out frontWallHit,deteccionlength,queEsPared);
        wallLookAngle = Vector3.Angle(orientacion.forward,-frontWallHit.normal);
        bool newWall = frontWallHit.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal,frontWallHit.normal)) > minWallNormalAngleChange;
        if( (isWallFront && newWall) || playerMovements.grounded)
        {
            climbTimer = maxClimbTime;
            climbJumpsLeft = climbJumps;
        }
    }

    private void StartCLimbing()
    {
        climbing = true;
        playerMovements.climbing = true;
        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;
        // TODO: Camara FOV
    }
    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x,climbspeed,rb.velocity.z);
        // TODO: Efectos de sonido
    }
    private void StopCLimbing()
    {
        climbing = false;
        playerMovements.climbing = false;
        // TODO: Efectos de sonido
    }

    private void ClimbJump()
    {
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 forceToApply = transform.up * climbJumpUpForce + frontWallHit.normal * climbJumpBackForce;
        rb.velocity = new Vector3(rb.velocity.x,0f,rb.velocity.z);
        rb.AddForce(forceToApply,ForceMode.Impulse);
        climbJumpsLeft--;
    }

}
