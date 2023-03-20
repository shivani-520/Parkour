using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderClimbing : MonoBehaviour
{
    public InputMaster inputMaster;

    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private LayerMask whatIsLadder;

    [Header("Climbing")]
    [SerializeField] private float climbSpeed;
    [SerializeField] private float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("Detection")]
    [SerializeField] private float detectionLength;
    [SerializeField] private float sphereCastRadius;
    [SerializeField] private float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;

    bool ladder;

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

    private void Awake()
    {
        inputMaster = new InputMaster();
    }

    private void Update()
    {
        LadderCheck();
        StateMachine();

        inputMaster.Player.LadderClimb.performed += context => ladder = true;
        inputMaster.Player.LadderClimb.canceled += context => ladder = false;


        if (climbing) ClimbingMovement();
    }

    private void StateMachine()
    {
        // State 1 - climbing
        if(wallFront && ladder == true && wallLookAngle < maxWallLookAngle)
        {
            if (!climbing && climbTimer > 0) StartClimbing();

            //timer
            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer < 0) StopClimbing();
        }

        // state 3 - none
        else
        {
            if (climbing) StopClimbing();
        }
    }

    private void LadderCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsLadder);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        if(playerMovement.grounded)
        {
            climbTimer = maxClimbTime;
        }
    }

    private void StartClimbing()
    {
        climbing = true;

        // camera fov change
    }

    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);

        // sound effect
    }

    private void StopClimbing()
    {
        climbing = false;

        // particle effect
    }
    
}
