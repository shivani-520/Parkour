using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    public InputMaster inputMaster;

    private float sensX;
    private float sensY;

    [SerializeField] float gamepadSensX;
    [SerializeField] float gamepadSensY;
    [SerializeField] float mouseSensX;
    [SerializeField] float mouseSensY;

    [SerializeField] Transform orientation;
    [SerializeField] Transform cameraHolder;

    float xRotation;
    float yRotation;

    float inputX;
    float inputY;

    bool isGamepad;

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(isGamepad)
        {
            sensX = gamepadSensX;
            sensY = gamepadSensY;
        }
        else
        {
            sensX = mouseSensX;
            sensY = mouseSensY;
        }

        // Mouse input

        inputX = inputMaster.Player.LookX.ReadValue<float>();
        inputY = inputMaster.Player.LookY.ReadValue<float>();

        float mouseX = inputX * Time.deltaTime * sensX;
        float mouseY = inputY * Time.deltaTime * sensY;


        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Rotate camera and orientation
        cameraHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
    public void FOVChange(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void TiltChange(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }

    public void OnDeviceChange(PlayerInput pi)
    {
        isGamepad = pi.currentControlScheme.Equals("Gamepad") ? true : false;
    }
}
