using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecomSystem : MonoBehaviour
{
    [SerializeField] private AudioSource playerAudioSource = default;
    [SerializeField] private AudioClip[] doorClipsClose = default;
    [SerializeField] private AudioClip[] DecomClips = default;
    [SerializeField] private Animator myDoor1 = null;
    [SerializeField] private Animator myDoor2 = null;
    [SerializeField] private GameObject myDoorSystem1 = null;
    [SerializeField] private GameObject myDoorSystem2 = null;
    public bool leave=true;

    void Start()
    {
        playerAudioSource = GameObject.FindWithTag("Player").GetComponent<AudioSource>();
    }

    IEnumerator ExampleCoroutine()
    {
        Debug.Log("Started Coroutine at timestamp : " + Time.time);
        yield return new WaitForSeconds(3);
        Debug.Log("Finished Coroutine at timestamp : " + Time.time);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && leave==false)
        {
            playerAudioSource.PlayOneShot(DecomClips[0]);
            playerAudioSource.PlayOneShot(DecomClips[1]);
            int x=0;
            if(myDoorSystem1.GetComponent<DoorCheck>().opened==true)
            {
                x++;
                myDoorSystem1.GetComponent<DoorCheck>().opened=false;
                myDoor1.Play("dor_close", 0, 0.5f);
            }
            if(myDoorSystem2.GetComponent<DoorCheck>().opened==true)
            {
                x++;
                myDoorSystem2.GetComponent<DoorCheck>().opened=false;
                myDoor2.Play("dor_close", 0, 0.5f);
            }
            if(x>0)
            {
                playerAudioSource.PlayOneShot(doorClipsClose[UnityEngine.Random.Range(0, doorClipsClose.Length - 1)]);
                StartCoroutine(ExampleCoroutine());

            }
            myDoorSystem1.GetComponent<DoorCheck>().damTimer=5;
            myDoorSystem2.GetComponent<DoorCheck>().damTimer=5;
        }
    }
}
