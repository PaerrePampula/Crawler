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
    public void ChangeDebug()
    {
        Globals.DebugOn = _toggle.isOn;
    }
}