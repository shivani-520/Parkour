using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    [SerializeField] private GameObject jumpscare;
    [SerializeField] private GameObject sounds;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        sounds.SetActive(false);
        jumpscare.SetActive(true);
        yield return new WaitForSeconds(5f);
        SceneManager.LoadScene("RestartScene");
    }
}
