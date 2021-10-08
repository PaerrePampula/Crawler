
using System;
using UnityEngine;

class DebugElement : MonoBehaviour
{
    private void Start()
    {
        Globals.onDebugChanged += changeState;
        gameObject.SetActive(Globals.DebugOn);
    }
    private void OnDestroy()
    {
        Globals.onDebugChanged -= changeState;
    }
    private void changeState(bool state)
    {
        gameObject.SetActive(state);
    }
}