using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndInteractiveObjectTrigger : MonoBehaviour
{
    [SerializeField] BaseMook roomBoss;
    [SerializeField] Transform activateOnEnd;
    private void OnEnable()
    {
        activateOnEnd.gameObject.SetActive(false);
        roomBoss.onMookDeath += startEndSequence;
    }
    private void OnDisable()
    {
        roomBoss.onMookDeath -= startEndSequence;
    }

    private void startEndSequence()
    {
        activateOnEnd.gameObject.SetActive(true);
    }
}
