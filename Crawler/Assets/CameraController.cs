using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //Asia, jota kamera seuraa.
    [SerializeField] Transform cameraTarget;
    //VEKTORIT
    //Et�isyys, jolla kamera seuraa kameran targettia.
    [SerializeField] Vector3 cameraOffset;
    //YKSIARVOISET KENT�T
    [SerializeField] float cameraMovementSpeed = 2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.position = Vector3.Lerp(transform.position, cameraTarget.position + cameraOffset, Time.deltaTime * cameraMovementSpeed);
    }
}
