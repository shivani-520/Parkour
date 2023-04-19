using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Slider sanityMeter;
    [SerializeField] private float maxSanity;
    [SerializeField] private float currentSanity;
    [SerializeField] private float sanityAmount;

    [SerializeField] private Animator sanityEffect;

    [SerializeField] private float speed;
    [SerializeField] private float delayTime = 3.0f; // the amount of time the enemy will stay behind after the player exits the collider
    [SerializeField] private AudioClip clip;

    private bool nearPlayer;
    private bool isDelayed;
    private float delayTimer;
    [SerializeField] private bool onCollison = false;

    // Start is called before the first frame update
    void Start()
    {
        currentSanity = 0;
        sanityMeter.maxValue = maxSanity;
        sanityMeter.value = currentSanity;
    }

    // Update is called once per frame
    void Update()
    {
        if (nearPlayer == false && isDelayed == false)
        {
            transform.Translate(Vector3.forward * speed);
        }
        else if (isDelayed == true)
        {
            delayTimer += Time.deltaTime;
            if (delayTimer >= delayTime)
            {
                delayTimer = 0f;
                isDelayed = false;
            }
        }

        if(onCollison)
        {
            currentSanity += sanityAmount * Time.deltaTime;
            sanityMeter.value = currentSanity;
        }
        else
        {
            if(currentSanity >= 0)
            {
                currentSanity -= 3 * Time.deltaTime;
                sanityMeter.value = currentSanity;
            }
        }

        if(currentSanity >= maxSanity)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onCollison = true;

            sanityEffect.SetTrigger("sanity");

            SoundManager.instance.PlaySound(clip);
            nearPlayer = true;
            isDelayed = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onCollison = false;

            sanityEffect.SetTrigger("sane");

            nearPlayer = false;
            isDelayed = true;
        }
    }

}
