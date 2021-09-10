using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTracker : MonoBehaviour
{
    int locationHistoryAmount = 50;
    int locationIndex = 0;
    Vector3[] locationsWhereTransformTouchedGround = new Vector3[50];
    PlayerController playerController;
    public int LocationIndex
    {
        get 
        { 
            return locationIndex; 
        }
        set
        {
            if (value > locationHistoryAmount - 1)
            {
                value = 0;
            }
            locationIndex = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void FixedUpdate()
    {
        if (playerController.isGrounded)
        {
            locationsWhereTransformTouchedGround[LocationIndex] = transform.position;
            LocationIndex++;
        }

    }
    public void returnPlayerToSafety()
    {

        transform.position = locationsWhereTransformTouchedGround[currentIndexReverseBy(3)];
        Physics.SyncTransforms();
    }
    //The edge case for player only touching ground for 5 frames is extremely unlikely, so lets 
    //not worry about that
    int currentIndexReverseBy(int toReverse)
    {
        int newIndex = locationIndex-toReverse;
        if (newIndex < 0)
        {
            newIndex = locationHistoryAmount - 1 - newIndex;
        }
        return newIndex;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
