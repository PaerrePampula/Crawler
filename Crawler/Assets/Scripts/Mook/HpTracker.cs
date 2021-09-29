using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpTracker : MonoBehaviour
{
    [SerializeField] Transform hpBarParent;
    [SerializeField] Image hpImage;
    [SerializeField] BaseMook baseMook;
    float maxHpValue;
    private void OnEnable()
    {
        baseMook.onMookInstanceDamaged += changeHpBarValue;
    }
    private void OnDisable()
    {
        baseMook.onMookInstanceDamaged -= changeHpBarValue;
    }
    private void Start()
    {
        hpImage.fillAmount = 1;
        maxHpValue = baseMook.MaxHP;
    }

    private void changeHpBarValue(float newHP)
    {
        if (hpBarParent.gameObject.activeSelf == false) hpBarParent.gameObject.SetActive(true);
        hpImage.fillAmount = newHP / maxHpValue;
    }
}
