using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTimer : MonoBehaviour
{
    public float ElevTimer=0;
    void Update()
    {
        if(ElevTimer>0)
            ElevTimer-=Time.deltaTime;
    }
}
