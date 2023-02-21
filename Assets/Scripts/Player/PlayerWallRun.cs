using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWallRun : MonoBehaviour
{
    public InputMaster inputMaster;

    [Header("Wall Running")]
    [SerializeField] LayerMask whatIsWall;
    [SerializeField] LayerMask whatIsGround;
    [SerializeField] float wallRunForce;
    [SerializeField] float wallJumpUpForce;
    [SerializeField] float wallJumpSideForce;
    [SerializeField] float wallClimbSpeed;
    [SerializeField] float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    Vector2 move;
    bool jump = false;

    [Header("Detection")]
    [SerializeField] float wallCheckDistance;
    [SerializeField] float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Exiting")]
    private bool exitingWall;
    [SerializeField] float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    [SerializeField] bool useGravity;
    [SerializeField] float gravityCounterForce;

    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] PlayerCamera cam;
    private PlayerMovement movement;
    private Rigidbody rb;

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

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        CheckForWall();
        StateMachine();
        MyInput();
    }

    private void FixedUpdate()
    {
        if(movement.wallRunning)
        {
            WallRunMovement();
        }
    }

    void MyInput()
    {
        move = inputMaster.Player.Movement.ReadValue<Vector2>();

        inputMaster.Player.Jump.performed += context => jump = true;
        inputMaster.Player.Jump.canceled += context => jump = false;
    }

    void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    void StateMachine()
    {
        // wall running
        if((wallLeft || wallRight) && move.y > 0 && AboveGround() && !exitingWall)
        {
            // start wall run
            if(!movement.wallRunning)
            {
                StartWallRun();
            }
            // wallrun timer
            if(wallRunTimer > 0)
            {
                wallRunTimer -= Time.deltaTime;
            }
            if(wallRunTimer <= 0 && movement.wallRunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }
            // wall jump
            if(jump)
            {
                WallJump();
            }
        }
        // exiting
        else if(exitingWall)
        {
            if(movement.wallRunning)
            {
                StopWallRun();
            }
            if(exitWallTimer > 0)
            {
                exitWallTimer -= Time.deltaTime;
            }
            if(exitWallTimer <= 0)
            {
                exitingWall = false;
            }
        }
        // none
        else
        {
            if(movement.wallRunning)
            {
                StopWallRun();
            }
        }
    }

    void StartWallRun()
    {

        movement.wallRunning = true;

        wallRunTimer = maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        // apply camera effects
        cam.FOVChange(70f);
        if (wallLeft) cam.TiltChange(-15f);
        if (wallRight) cam.TiltChange(15f);
    }

    void WallRunMovement()
    {
        rb.useGravity = useGravity;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }

        // forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);

        // push to wall force
        if (!(wallLeft && move.x > 0) && !(wallRight && move.y < 0))
        {
            rb.AddForce(-wallNormal * 100, ForceMode.Force);
        }

        // weaken gravity
        if(useGravity)
        {
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);
        }

    }

    void StopWallRun()
    {

        movement.wallRunning = false;

        // reset camera effects
        cam.FOVChange(60f);
        cam.TiltChange(0f);
    }

    void WallJump()
    {
        // enter exiting wall state
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        // reset y velocity and add force
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

}
