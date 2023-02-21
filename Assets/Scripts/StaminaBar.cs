using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    [SerializeField] private Slider staminaBar;
    [SerializeField] private GameObject slider;

    private int maxStamina = 200;
    private int currentStamina;

    private WaitForSeconds regenTick = new WaitForSeconds(0.1f);
    private Coroutine regen;

    public static StaminaBar instance;
    [SerializeField] private PlayerMovement movement;

    private void Awake()
    {
        instance = this;

    }

    private void Start()
    {
        currentStamina = maxStamina;
        staminaBar.maxValue = maxStamina;
        staminaBar.value = currentStamina;
    }

    private void Update()
    {
        if(currentStamina >= maxStamina)
        {
            slider.SetActive(false);
        }
    }

    public void UseStamina(int amount)
    {
        if(currentStamina - amount >= 0)
        {
            currentStamina -= amount;
            staminaBar.value = currentStamina;

            if(regen != null)
            {
                StopCoroutine(regen);
            }

            regen = StartCoroutine(RegenStamina());
            movement.sprint = true;
        }
        else
        {
            movement.sprint = false;
            Debug.Log("Not enough stamina");
        }
        slider.SetActive(true);
    }

    private IEnumerator RegenStamina()
    {
        yield return new WaitForSeconds(2);

        while(currentStamina <= maxStamina)
        {
            currentStamina += 3;
            staminaBar.value = currentStamina;
            yield return regenTick;
        }

        regen = null;
    }
}
