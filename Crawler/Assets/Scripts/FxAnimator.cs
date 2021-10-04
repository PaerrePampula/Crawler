using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FxAnimator : MonoBehaviour
{
    [SerializeField] VisualEffect visualEffect;
    [SerializeField] string floatName;

    public void SetVFXFloat(float value)
    {
        visualEffect.SetFloat(floatName, value);
    }
}
