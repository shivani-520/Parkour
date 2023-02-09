using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public InputMaster inputMaster;

    public Vector2 move;

    public float xAxis;
    public float yAxis;

    public bool jump;

    public PlayerMovement movement;

    private void Awake()
    {
        inputMaster = new InputMaster();
    }

    private void Update()
    {
        move = inputMaster.Player.Movement.ReadValue<Vector2>();

        xAxis = inputMaster.Player.LookX.ReadValue<float>();
        yAxis = inputMaster.Player.LookY.ReadValue<float>();
    }

    private void OnEnable()
    {
        inputMaster.Enable();
    }

    private void OnDisable()
    {
        inputMaster.Disable();
    }

    void MoveInput()
    {


    }
}
