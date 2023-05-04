using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseMusicStarts : MonoBehaviour
{
    [SerializeField] private AudioClip chaseMusic;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SoundManager.instance.PlayLoopSound(chaseMusic);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            SoundManager.instance.StopPlayingLoopSound();
        }
    }
}
