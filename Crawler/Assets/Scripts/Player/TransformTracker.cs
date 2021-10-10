using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Tracks the player location, and returns the player to the location tracked when triggered
/// </summary>
public class TransformTracker : MonoBehaviour
{
    Vector3 locationWhereTransformTouchedGround;
    PlayerController playerController;
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void FixedUpdate()
    {
        //The player might endlessly fall into the void if the platform where the player stands moves, dont save this as a safe location
        if (playerController.isGrounded && !playerController.IsOnTopOfPlatform)
        {
            locationWhereTransformTouchedGround = transform.position;

        }

    }
    public void returnPlayerToSafety()
    {

        transform.position = locationWhereTransformTouchedGround;
        Physics.SyncTransforms();
    }
}
