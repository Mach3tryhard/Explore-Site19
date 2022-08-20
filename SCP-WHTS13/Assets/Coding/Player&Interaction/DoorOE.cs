using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOE : MonoBehaviour
{
    [SerializeField]private AudioSource playerAudioSource = default;
    //[SerializeField] private AudioClip[] ScanSounds = default;
    [SerializeField] private AudioClip[] doorClipsopen = default;
    [SerializeField] private Animator myDoor1 = null;
    [SerializeField] private Animator myDoor2 = null;

    void Start()
    {
        playerAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            if(transform.gameObject.GetComponent<DoorCheck>().damTimer<=0 && transform.gameObject.GetComponent<DoorCheck>().opened == false)
            {
                //playerAudioSource.PlayOneShot(ScanSounds[UnityEngine.Random.Range(0, ScanSounds.Length - 1)]);
                myDoor1.Play("door_open1", 0, 0.0f);
                myDoor2.Play("door_open2", 0, 0.0f);
                transform.gameObject.GetComponent<DoorCheck>().opened=true;
                transform.gameObject.GetComponent<DoorCheck>().damTimer=2;
                playerAudioSource.PlayOneShot(doorClipsopen[UnityEngine.Random.Range(0, doorClipsopen.Length - 1)]);
            }
        }
    }
}
