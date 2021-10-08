using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpin : MonoBehaviour
{
    [SerializeField] float degreesInSecond;
    [SerializeField] Transform objectToSpin;
    private void Update()
    {
        objectToSpin.transform.Rotate(new Vector3(0,  degreesInSecond * Time.deltaTime, 0));
    }
}
