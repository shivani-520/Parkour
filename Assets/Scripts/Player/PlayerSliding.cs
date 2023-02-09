using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSliding : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerGfx;
    private Rigidbody rb;
    private PlayerMovement movement;

    [Header("Sliding")]
    [SerializeField] float maxSlideTime;
    [SerializeField] float slideForce;
    private float slideTimer;

    [SerializeField] float slideYScale;
    private float startYScale;

    [Header("Input")]
    [SerializeField] KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();

        startYScale = playerGfx.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }
        if(Input.GetKeyUp(slideKey) && movement.sliding)
        {
            StopSlide();
        }
    }

    private void FixedUpdate()
    {
        if(movement.sliding)
        {
            SlidingMovement();
        }
    }

    void StartSlide()
    {
        movement.sliding = true;

        playerGfx.localScale = new Vector3(playerGfx.localScale.x, slideYScale, playerGfx.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // sliding normal
        if(!movement.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }
        // sliding down a slope
        else
        {
            rb.AddForce(movement.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }


        if(slideTimer <= 0)
        {
            StopSlide();
        }
    }

    void StopSlide()
    {
        movement.sliding = false;

        playerGfx.localScale = new Vector3(playerGfx.localScale.x, startYScale, playerGfx.localScale.z);
    }
}
