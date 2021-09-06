using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlopper : MonoBehaviour
{
    [SerializeField] NeighborType doorLocation;
    [SerializeField] Room otherRoom;
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CurrentRoomManager.Singleton.setNewRoom(otherRoom, doorLocation);
        }
    }
}
