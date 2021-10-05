using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Sends the player back if player falls below the scene e.g from drops.
/// </summary>
public class FallDownHurtBox : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseMook>())
        {
            if (other.GetComponent<RockBoss>()) return;
            other.GetComponent<BaseMook>().ChangeHp(-10000);
        }
        if (other.GetComponent<TransformTracker>() != null)
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
