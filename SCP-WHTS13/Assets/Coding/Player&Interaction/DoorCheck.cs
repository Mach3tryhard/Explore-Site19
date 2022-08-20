using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCheck : MonoBehaviour
{
    public bool opened;
    public float damTimer=2;
    void Update()
    {
        if(damTimer > 0)
        {
            damTimer -= Time.deltaTime;
        }
    }
}
