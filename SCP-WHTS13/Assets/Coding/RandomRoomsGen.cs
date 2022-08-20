using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomRoomsGen : MonoBehaviour
{
    //public NavMeshSurface surface;
    public List<Vector3> RoomPositions = new List<Vector3>();
    public List<Quaternion> RoomRotation = new List<Quaternion>();
    public List<GameObject> RoomVector = new List<GameObject>();
    public List<int> NumberOfRooms = new List<int>();
    void Start()
    {
        for(int i=0;i<RoomPositions.Count;i++)
        {
            int j=UnityEngine.Random.Range(0,RoomVector.Count);
            GameObject CurrentRoom;
            CurrentRoom = Instantiate(RoomVector[j], RoomPositions[i], RoomRotation[i]);
            /// Trying to static and batch the rooms generated
            /*CurrentRoom.isStatic=true;
            int children = CurrentRoom.transform.childCount;
            for(int k=0;k<children;k++)
            {
                GameObject caca;
                caca=CurrentRoom.transform.GetChild(k).gameObject;
                if(caca.name!="Doorway"){caca.isStatic=true;}
            }
            CurrentRoom.transform.parent = transform;
            StaticBatchingUtility.Combine(CurrentRoom);*/
            /// Trying to static and batch the rooms generated
            NumberOfRooms[j]--;
            if(NumberOfRooms[j]==0)
            {
                RoomVector.RemoveAt(j);
                NumberOfRooms.RemoveAt(j);
            }
        }
        //surface.BuildNavMesh();
    }
}
