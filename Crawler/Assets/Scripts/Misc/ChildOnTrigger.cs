using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Class is used to use triggers on child of transforms to do actions on the parent gameobject
/// </summary>
public class ChildOnTrigger : MonoBehaviour
{
    public Action<Collider> delegatesOnTrigger;
    public Action<Collider> delegatesOnTriggerExit;
    public Func<Collider, bool> predicatesForDelegateTrigger;
    public Func<Collider, bool> predicatesForDelegateExitTrigger;
    private void OnTriggerEnter(Collider other)
    {
        //Can either be triggered any time, or by predicates set by parent (as func)
        if (predicatesForDelegateTrigger != null)
        {
            if (predicatesForDelegateTrigger.Invoke(other)) delegatesOnTrigger?.Invoke(other);
        }
        else
        {
            delegatesOnTrigger?.Invoke(other);
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (predicatesForDelegateExitTrigger != null)
        {
            if (predicatesForDelegateTrigger.Invoke(other)) delegatesOnTriggerExit?.Invoke(other);
        }
        else
        {
            delegatesOnTriggerExit?.Invoke(other);
        }
    }
}
