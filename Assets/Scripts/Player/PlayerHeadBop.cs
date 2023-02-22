using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeadBop : MonoBehaviour
{
    [SerializeField] private bool canUseHeadBop = true;

    [SerializeField] private float walkBopSpeed = 10f;
    [SerializeField] private float walkBopAmount = 0.05f;

    private float defaultYPos = 0;
    private float timer;

    [SerializeField] Transform cam;

    private PlayerMovement movement;

    private void Awake()
    {
        movement = GetComponent<PlayerMovement>();

        defaultYPos = cam.transform.localPosition.y;
    }

    private void Update()
    {
        if(canUseHeadBop)
        {
            HandleHeadBop();
        }
    }

    void HandleHeadBop()
    {
        if(!movement.grounded && movement.sliding)
        {
            return;
        }

        if(Mathf.Abs(movement.moveDirection.x) > 0.1f || Mathf.Abs(movement.moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * walkBopSpeed;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, defaultYPos + Mathf.Sin(timer) * walkBopAmount, cam.transform.localPosition.z);
        }
    }

}
