using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCP1874 : MonoBehaviour
{
    [SerializeField]private AudioSource playerAudioSource = default;
    [SerializeField] private AudioClip[] dmgClips = default;
    [SerializeField] Animator EnemyAnimator;
    [SerializeField] GameObject TheGO;
    public float damTimer;
    public float damageDealt=100;

    void Start()
    {
        playerAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
    }

    void Update()
    {
        if(damTimer > 0)
        {
            damTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            if(damTimer<=0)
            {
                GameObject.FindWithTag("SCP/1874").AddComponent<TraumaInducer>();
                //EnemyAnimator.SetBool("IsAttacking", true);
                playerAudioSource.PlayOneShot(dmgClips[UnityEngine.Random.Range(0, dmgClips.Length - 1)]);
                FirstPersonController.OnTakeDamage(damageDealt);
                damTimer=5;
            }
        }
    }
}
