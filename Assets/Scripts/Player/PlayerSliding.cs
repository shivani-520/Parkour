using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSliding : MonoBehaviour
{
    public InputMaster inputMaster;

    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform playerGfx;
    private Rigidbody rb;
    private PlayerMovement movement;

    [Header("Sliding")]
    [SerializeField] float maxSlideTime;
    [SerializeField] float slideForce;
    [SerializeField] float slideTimer;

    [SerializeField] float slideYScale;
    private float startYScale;

    [Header("Input")]
    Vector2 move;
    public bool slide = false;


    private void Awake()
    {
        inputMaster = new InputMaster();
    }

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        movement = GetComponent<PlayerMovement>();

        startYScale = playerGfx.localScale.y;
    }

    void MyInput()
    {
        move = inputMaster.Player.Movement.ReadValue<Vector2>();

        inputMaster.Player.Slide.started += context => slide = true;
        inputMaster.Player.Slide.performed += context => slide = false;
    }

    private void Update()
    {
        MyInput();

        if(slide && (move.x != 0 || move.y != 0))
        {
            StartSlide();
        }
        if(!slide && movement.sliding)
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
        Vector3 inputDirection = orientation.forward * move.y + orientation.right * move.x;

        // sliding normal
        if(!movement.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.fixedDeltaTime;
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
