using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOnAnimationTrigger : MonoBehaviour
{
    public delegate void TriggerState();
    public event TriggerState onTriggerState;
    public void StartTriggerState()
    {
        onTriggerState?.Invoke();
    }
}
