using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClimbing : MonoBehaviour
{
    [Header("References")]
    private PlayerMovement movement;
    [SerializeField] Transform orientation;
    [SerializeField] Transform cam;
    private Rigidbody rb;

    [Header("Ledge Grabbing")]
    [SerializeField] float moveToLedgeSpeed;
    [SerializeField] float maxLedgeGrabDistance;

    [SerializeField] float minTimeOnLedge;
    private float timeOnLedge;

    [SerializeField] bool holding;

    [Header("Ledge Jumping")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] float ledgeJumpForwardForce;
    [SerializeField] float ledgeJumpUpwardsForce;

    [Header("Ledge Detection")]
    [SerializeField] float ledgeDetectionLength;
    [SerializeField] float ledgeSphereCastRadius;
    [SerializeField] LayerMask whatIsLedge;

    private Transform lastLedge;
    private Transform currentLedge;

    private RaycastHit ledgeHit;

    [Header("Exiting")]
    [SerializeField] bool exitingLedge;
    [SerializeField] float exitLedgeTime;
    private float exitLedgeTimer;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        LedgeDetection();
        SubStateMachine();
    }

    void SubStateMachine()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool anyInputKeyPressed = horizontalInput != 0 || verticalInput != 0;

        // holding onto ledge
        if(holding)
        {
            FreezeRigidbodyOnLedge();

            timeOnLedge += Time.deltaTime;

            if (timeOnLedge > minTimeOnLedge && anyInputKeyPressed) ExitLedgeHold();

            if (Input.GetKeyDown(jumpKey)) LedgeJump();
        }

        // exiting
        else if (exitingLedge)
        {
            if (exitLedgeTimer > 0) exitLedgeTimer -= Time.deltaTime;
            else exitingLedge = false;
        }
    }

    void LedgeDetection()
    {
        bool ledgeDetected = Physics.SphereCast(transform.position, ledgeSphereCastRadius, cam.forward, out ledgeHit, ledgeDetectionLength, whatIsLedge);

        if (!ledgeDetected) return;

        float distanceToLedge = Vector3.Distance(transform.position, ledgeHit.transform.position);

        if (ledgeHit.transform == lastLedge) return;

        if (distanceToLedge < maxLedgeGrabDistance & !holding) EnterLedgeHold();
    }

    void LedgeJump()
    {
        ExitLedgeHold();

        Invoke(nameof(DelayJumpForce), 0.05f);
    }

    void DelayJumpForce()
    {
        Vector3 forceToAdd = cam.forward * ledgeJumpForwardForce + orientation.up * ledgeJumpUpwardsForce;
        rb.velocity = Vector3.zero;
        rb.AddForce(forceToAdd, ForceMode.Impulse);
    }

    void EnterLedgeHold()
    {
        holding = true;

        movement.unlimited = true;
        movement.restricted = true;

        currentLedge = ledgeHit.transform;
        lastLedge = ledgeHit.transform;

        rb.useGravity = false;
        rb.velocity = Vector3.zero;
    }

    void FreezeRigidbodyOnLedge()
    {
        rb.useGravity = false;

        Vector3 directionToLedge = currentLedge.position - transform.position;
        float distanceToLedge = Vector3.Distance(transform.position, currentLedge.position);

        // move player towards ledge
        if(distanceToLedge > 1f)
        {
            if(rb.velocity.magnitude < moveToLedgeSpeed)
            {
                rb.AddForce(directionToLedge.normalized * moveToLedgeSpeed * 1000f * Time.deltaTime);
            }
        }

        // hold onto ledge
        else
        {
            if (!movement.freeze) movement.freeze = true;
            if (movement.unlimited) movement.unlimited = false;
        }

        // exiting if something goes wrong
        if (distanceToLedge > maxLedgeGrabDistance) ExitLedgeHold();
    }

    void ExitLedgeHold()
    {
        exitingLedge = true;
        exitLedgeTimer = exitLedgeTime;

        holding = false;
        timeOnLedge = 0f;

        movement.restricted = false;
        movement.freeze = false;

        rb.useGravity = true;

        StopAllCoroutines();
        Invoke(nameof(ResetLastLedge), 1f);
    }

    void ResetLastLedge()
    {
        lastLedge = null;
    }
}
