using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float maxTime;
    [SerializeField] private float maxDistance;
    private float timer;

    [SerializeField] private float sanityRadius;
    [SerializeField] private float jumpscareRadius;

    [SerializeField] private Transform player;

    private NavMeshAgent agent;

    private Animator anim;

    [SerializeField] private SanityManager sanityLogic;

    [SerializeField] private GameObject jumpscare;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {

        timer -= Time.deltaTime;
        if(timer < 0f)
        {
            float sqDistance = (player.transform.position - agent.destination).sqrMagnitude;

            if(sqDistance > maxDistance*maxDistance)
            {
                agent.SetDestination(player.position);
            }

            timer = maxTime;
        }

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= sanityRadius)
        {
            sanityLogic.onCollison = true;
        }
        else
        {
            sanityLogic.onCollison = false;

        }

        if(distance <= jumpscareRadius)
        {
            sanityLogic.StartCoroutine("Jumpscare");
        }
        anim.SetFloat("Speed", agent.velocity.magnitude);

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sanityRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, jumpscareRadius);

    }


}
