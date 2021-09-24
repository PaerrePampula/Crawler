using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SalesPersonTalkingTrigger : MonoBehaviour
{
    public delegate void OnPersonApproachTrigger();
    public static event OnPersonApproachTrigger onPersonApproach;
    public static event OnPersonApproachTrigger onPersonLeave;
    private void OnTriggerEnter(Collider other)
    {
        onPersonApproach?.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        onPersonLeave?.Invoke();
    }
}
