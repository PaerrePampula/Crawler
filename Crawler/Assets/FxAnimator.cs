using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FxAnimator : MonoBehaviour
{
    VisualEffect visualEffect;
    [SerializeField] string floatName;
    private void Awake()
    {
        visualEffect = GetComponentInChildren<VisualEffect>();
    }
    public void SetVFXFloat(float value)
    {
        visualEffect.SetFloat(floatName, value);
    }
}
