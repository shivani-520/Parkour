using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpscareToRestartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Delay());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("RestartScene");

    }
}
