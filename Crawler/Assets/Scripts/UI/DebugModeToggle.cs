using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugModeToggle : MonoBehaviour
{
    Toggle _toggle;
    private void Start()
    {
        _toggle = GetComponent<Toggle>();
    }
    public void ChangeDebug(string toggletype)
    {
        if (toggletype == "Debug") Globals.DebugOn = _toggle.isOn;
        if (toggletype == "States") Globals.LogStates = _toggle.isOn;

    }
}