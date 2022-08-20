using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTest : MonoBehaviour
{
    [SerializeField]private AudioSource playerAudioSource = default;
    [SerializeField] private AudioClip[] dmgClips = default;
    [SerializeField] Animator EnemyAnimator;
    public float damTimer;
    public float damageDealt=10;
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
                EnemyAnimator.Play("Zombie Attack", 0, 0);
                playerAudioSource.PlayOneShot(dmgClips[UnityEngine.Random.Range(0, dmgClips.Length - 1)]);
                FirstPersonController.OnTakeDamage(damageDealt);
                damTimer=2;
            }
        }
    }
}
