using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private float startStamina = 50;
    public float currentStamina;
    public Slider sliderStamina;

    private void Start()
    {
        currentStamina = startStamina;
    }

    private void Update()
    {
        currentStamina += 10f;
        sliderStamina.value = currentStamina;

        if(currentStamina >= 50)
        {
            currentStamina = 50;
        }
    }

    public void TakeStamina(float amount)
    {
        currentStamina -= amount;
        sliderStamina.value = currentStamina;
    }
}
