using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeThroughCircleController : MonoBehaviour
{
    static int PosID = Shader.PropertyToID("_PlayerPosition");
    static int SizeID = Shader.PropertyToID("_Size");
    [SerializeField] Material ObstacleMaterial;
    Camera camera;
    [SerializeField] LayerMask Mask;
    private void Start()
    {
        camera = Camera.main;

    }
    private void Update()
    {
        Vector3 directionToCamera = camera.transform.position - transform.position;
        Ray ray = new Ray(transform.position, directionToCamera.normalized);
        if (Physics.Raycast(ray, 5000, Mask))
        {
            ObstacleMaterial.SetFloat(SizeID, 1);
        }
        else
        {
            ObstacleMaterial.SetFloat(SizeID, 0);
        }

        Vector3 view = camera.WorldToViewportPoint(transform.position);
        ObstacleMaterial.SetVector(PosID, view);
    }
}
