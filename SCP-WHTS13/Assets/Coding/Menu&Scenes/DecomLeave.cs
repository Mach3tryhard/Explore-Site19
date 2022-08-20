using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecomLeave : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.transform.parent.GetComponent<DecomSystem>().leave=true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            gameObject.transform.parent.GetComponent<DecomSystem>().leave=false;
        }
    }
}
