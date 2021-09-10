using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDownHurtBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<TransformTracker>().returnPlayerToSafety();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
