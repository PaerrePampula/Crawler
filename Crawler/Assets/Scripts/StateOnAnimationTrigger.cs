using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOnAnimationTrigger : MonoBehaviour
{
    public delegate void TriggerState();
    public event TriggerState onTriggerState;
    public event TriggerState onTriggerStateLeave;
    public void StartTriggerState()
    {
        onTriggerState?.Invoke();
    }
    public void StopTriggerState()
    {
        onTriggerStateLeave?.Invoke();
    }
}
