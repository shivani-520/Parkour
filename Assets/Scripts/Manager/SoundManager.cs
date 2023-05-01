using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource musicSource, effectSource;

    public AudioSource loopEffectSource;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip)
    {
        effectSource.PlayOneShot(clip);
    }

    public void PlayLoopSound(AudioClip clip)
    {
        loopEffectSource.PlayOneShot(clip);
    }
}
