using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    //Target that the camera follows
    [SerializeField] Transform cameraTarget;
    //VEKTORIT
    //The distance vector (as a constant) that the camera is offset from the target
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] float cameraMovementSpeed = 2;
    //Need to cache the current main camera to prevent pointless finding objects with tag on updates (Camera.main does that)
    Camera camera;
    Vector3 middleOfScreenInWorldSpace;
    Vector3 mousePos;
    Vector3 distanceBetweenMiddleofScreenAndCursor;

    //A multiplier for how much the camera can move away from the player when the player moves the mouse nearer the window border
    [SerializeField] float mouseMovementPanMultiplier;
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Need to include the camera clip plane to prevent the only vector given being the camera position
        mousePos = camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y,camera.nearClipPlane));

        middleOfScreenInWorldSpace = camera.ScreenToWorldPoint(Vector3.zero);
        distanceBetweenMiddleofScreenAndCursor = (new Vector3(mousePos.x, 0,mousePos.z) - new Vector3(middleOfScreenInWorldSpace.x, 0, middleOfScreenInWorldSpace.z));
        Vector3 mousePanVector = distanceBetweenMiddleofScreenAndCursor;

        this.transform.position = cameraTarget.position + cameraOffset + mousePanVector*mouseMovementPanMultiplier;
    }
}
