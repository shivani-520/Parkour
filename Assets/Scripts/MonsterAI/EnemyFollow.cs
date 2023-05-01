using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    [SerializeField] private float lookRadius;
    [SerializeField] private float sanityRadius;

    [SerializeField] private Transform player;

    private NavMeshAgent agent;

    private Animator anim;

    [SerializeField] private Enemy sanityLogic;

    [SerializeField] private GameObject jumpscare;

    private void Start()
    {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance <= lookRadius)
        {
            anim.SetFloat("Speed", agent.velocity.magnitude);
            agent.SetDestination(player.position);

            if (distance <= agent.stoppingDistance)
            {
                FaceTarget();
            }

            if(distance <= sanityRadius)
            {
                sanityLogic.onCollison = true;
            }
            else
            {
                sanityLogic.onCollison = false;
            }
        }


    }

    void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion LookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, LookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sanityRadius);
    }


}
