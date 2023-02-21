using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    public Image fillImage; // Reference to the fill image of the stamina bar
    public float maxStamina = 100f; // Maximum stamina
    public float staminaDecreasePerSecond = 20f; // Stamina decrease per second when sprinting
    public float staminaRegenPerSecond = 10f; // Stamina regeneration per second when not sprinting

    private float currentStamina; // Current stamina

    [SerializeField] private PlayerMovement movement;

    public static Stamina instance;

    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        currentStamina = maxStamina;
    }


    public void UseStamina()
    {
        if (movement.sprint) // If sprinting
        {
            currentStamina -= staminaDecreasePerSecond * Time.deltaTime; // Decrease stamina
        }
        else // If not sprinting
        {
            currentStamina += staminaRegenPerSecond * Time.deltaTime; // Regenerate stamina
        }

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina); // Clamp current stamina between 0 and max stamina

        fillImage.fillAmount = currentStamina / maxStamina; // Update fill image of the stamina bar
    }

}
