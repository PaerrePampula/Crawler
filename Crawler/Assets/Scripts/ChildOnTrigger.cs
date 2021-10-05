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
    private void OnTriggerEnter(Collider other)
    {
        delegatesOnTrigger.Invoke(other);
    }
}
