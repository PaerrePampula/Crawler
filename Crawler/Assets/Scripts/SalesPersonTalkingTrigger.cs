using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// A trigger, that makes the seller greet the player character
/// </summary>
public class SalesPersonTalkingTrigger : MonoBehaviour
{
    public delegate void OnPersonApproachTrigger();
    public static event OnPersonApproachTrigger onPersonApproach;
    public static event OnPersonApproachTrigger onPersonLeave;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        onPersonApproach?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            onPersonLeave?.Invoke();
    }
}
