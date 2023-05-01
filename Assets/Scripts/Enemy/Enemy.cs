using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Slider sanityMeter;
    [SerializeField] private float maxSanity;
    [SerializeField] private float currentSanity;
    [SerializeField] private float sanityAmount;

    [SerializeField] private Animator sanityEffect;

    public bool onCollison = false;

    [SerializeField] private Camera cam;

    [SerializeField] private AudioClip[] clip;
    [SerializeField] private AudioClip startClip;

    bool hasPlayedSound = false;

    [SerializeField] private GameObject chaseMusic;
    [SerializeField] private GameObject sanityBar;
    [SerializeField] private GameObject jumpscare;
    [SerializeField] private GameObject sounds;


    // Start is called before the first frame update
    void Start()
    {

        currentSanity = 90;
        sanityMeter.maxValue = maxSanity;
        sanityMeter.value = currentSanity;


    }

    // Update is called once per frame
    void Update()
    {

        if(onCollison)
        {
            sanityBar.SetActive(true);
            chaseMusic.SetActive(true);
            currentSanity -= sanityAmount * Time.deltaTime;
            sanityMeter.value = currentSanity;

            if(!hasPlayedSound)
            {
                int randomClipIndex = Random.Range(0, clip.Length);
                SoundManager.instance.PlaySound(clip[randomClipIndex]);

                hasPlayedSound = true;
                StartCoroutine(ResetHasPlayedSound());

                sanityEffect.SetTrigger("Insane");
            }

        }
        else
        {
            sanityBar.SetActive(false);
            chaseMusic.SetActive(false);
            if(currentSanity >= 0)
            {
                currentSanity += 5 * Time.deltaTime;
                sanityMeter.value = currentSanity;

                sanityEffect.SetTrigger("Sane");
            }
        }

        if(currentSanity < 0)
        {
            StartCoroutine(Delay());
            sounds.SetActive(false);
            chaseMusic.SetActive(false);
        }

        if(currentSanity >= 100)
        {
            currentSanity = 100;
        }
    }

                                      
    public void Shake()
    {
        cam.transform.DOShakePosition(1, 0.1f);
    }

    IEnumerator ResetHasPlayedSound()
    {
        yield return new WaitForSeconds(5f);
        hasPlayedSound = false;
    }

    IEnumerator Delay()
    {
        jumpscare.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("RestartScene");
    }

}
