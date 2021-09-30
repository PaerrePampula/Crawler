using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PercentHpTracker : MonoBehaviour
{
    [SerializeField] BaseMook _baseMook;
    [SerializeField] TextMeshProUGUI _percentageText;
    float maxValue;
    private void Start()
    {
        maxValue = _baseMook.MaxHP;
    }
    private void OnEnable()
    {
        _baseMook.onMookInstanceDamaged += updateHP;
    }

    private void updateHP(float newHP)
    {
        _percentageText.text = string.Format("{0:.##}%", ((newHP / maxValue) * 100));   
    }

    private void OnDisable()
    {
        _baseMook.onMookInstanceDamaged += updateHP;
    }
}
