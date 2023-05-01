using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartSceneSounds : MonoBehaviour
{
    [SerializeField] private AudioClip[] clip;

    private void Start()
    {
        int randomClipIndex = Random.Range(0, clip.Length);
        SoundManager.instance.PlaySound(clip[randomClipIndex]);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
