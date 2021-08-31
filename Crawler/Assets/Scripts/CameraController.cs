using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Save the playercontroller as a field to track the character movement better with the camera
    [SerializeField] PlayerController playerController;
    //Target that the camera follows
    [SerializeField] Transform cameraTarget;
    //VEKTORIT
    //The distance vector (as a constant) that the camera is offset from the target
    [SerializeField] Vector3 cameraOffset;

    [SerializeField] float cameraMovementSpeed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 newMovementOffset = playerController.getPlayerMovementVector() / 3f; 
        this.transform.position = Vector3.Lerp(transform.position, cameraTarget.position + cameraOffset + newMovementOffset, Time.deltaTime * cameraMovementSpeed);
    }
}
