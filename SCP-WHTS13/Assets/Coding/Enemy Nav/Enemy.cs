using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float lookRadius = 10f;

    public Animator animator;
    Transform target;
    NavMeshAgent agent;
    AudioSource audioSource;

    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        if(transform.GetChild(0).GetComponent<AudioSource>() != null)
            audioSource = transform.GetChild(0).GetComponent<AudioSource>();
    }
    void Update()
    {
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= lookRadius)
        {
            //animator.SetBool("IsWalking", true);
            agent.SetDestination(target.position);
            if(distance<= 2)
            {
                FaceTarget();
            }
        }
        float velocity = agent.velocity.magnitude;
        if(velocity==0)
        {
            if(transform.GetChild(0).GetComponent<AudioSource>() != null)
                audioSource.mute=true;
            animator.SetBool("IsWalking", false);
        }
        else
        {
            if(transform.GetChild(0).GetComponent<AudioSource>() != null)
                audioSource.mute=false;
            animator.SetBool("IsWalking", true);
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,lookRadius);
    }
}
