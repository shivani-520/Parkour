using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterZones : MonoBehaviour
{
    [SerializeField] private GameObject monster;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "MonsterAppearZone")
        {
            monster.SetActive(true);
        }

        if(other.gameObject.tag == "MonsterDisappearZone")
        {
            monster.SetActive(false);
        }
    }

}
