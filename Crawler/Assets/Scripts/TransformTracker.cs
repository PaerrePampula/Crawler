using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTracker : MonoBehaviour
{

    Vector3 locationWhereTransformTouchedGround;
    PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponent<PlayerController>();
    }
    private void FixedUpdate()
    {
        if (playerController.isGrounded)
        {
            locationWhereTransformTouchedGround = transform.position;

        }

    }
    public void returnPlayerToSafety()
    {

        transform.position = locationWhereTransformTouchedGround;
        Physics.SyncTransforms();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
