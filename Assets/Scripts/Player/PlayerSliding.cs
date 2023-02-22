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

    [SerializeField] private PlayerCamera cam;


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
        slideTimer = maxSlideTime;
    }

    void MyInput()
    {
        move = inputMaster.Player.Movement.ReadValue<Vector2>();

        inputMaster.Player.Slide.started += context => slide = true;
        inputMaster.Player.Slide.canceled += context => slide = false;
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

            slideTimer -= Time.fixedDeltaTime;
            if (slideTimer <= 0)
            {
                StopSlide();
                Invoke(nameof(CanSlideAgain), 3f);
            }
        }
    }

    void StartSlide()
    {
        movement.sliding = true;
        slide = true;
        playerGfx.localScale = new Vector3(playerGfx.localScale.x, slideYScale, playerGfx.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

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
    }

    void StopSlide()
    {
        movement.sliding = false;
        slide = false;
        playerGfx.localScale = new Vector3(playerGfx.localScale.x, startYScale, playerGfx.localScale.z);
        slideTimer = maxSlideTime;
    }

    private void CanSlideAgain()
    {
        slideTimer = maxSlideTime;
    }

}
