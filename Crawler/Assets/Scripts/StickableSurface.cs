using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickableSurface : MonoBehaviour
{
    [SerializeField] ChildOnTrigger triggerCollider;
    private void Start()
    {
        triggerCollider.predicatesForDelegateTrigger += checkIfPlayer;
        triggerCollider.predicatesForDelegateExitTrigger += checkIfPlayer;
        triggerCollider.delegatesOnTrigger += updateParentToPlayer;
        triggerCollider.delegatesOnTriggerExit += exitParentHoodForPlayer;
    }
    private void OnDestroy()
    {
        triggerCollider.predicatesForDelegateTrigger -= checkIfPlayer;
        triggerCollider.predicatesForDelegateExitTrigger -= checkIfPlayer;
        triggerCollider.delegatesOnTrigger -= updateParentToPlayer;
        triggerCollider.delegatesOnTriggerExit -= exitParentHoodForPlayer;
    }

    private void exitParentHoodForPlayer(Collider obj)
    {
        obj.transform.SetParent(null);
        obj.GetComponent<PlayerController>().IsOnTopOfPlatform = false;
    }

    private bool checkIfPlayer(Collider col)
    {
        return col.gameObject.CompareTag("Player");
    }

    private void updateParentToPlayer(Collider obj)
    {
        obj.transform.SetParent(transform);
        obj.GetComponent<PlayerController>().IsOnTopOfPlatform = true;
    }

}
