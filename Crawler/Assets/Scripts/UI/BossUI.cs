using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bossNameText;
    [SerializeField] Slider bossHealthSlider;
    [SerializeField] TextMeshProUGUI bossHealthText;
    BossMook boss;
    BaseMook bossBase;
    public void InitializeBossUI(BossMook bossMook)
    {
        boss = bossMook;
        //Set initial values for slider, textfield, etc and subscribe to boss losing hp for updating
        bossHealthSlider.maxValue = boss.GetBossHP();
        bossHealthSlider.value = bossHealthSlider.maxValue;
        bossNameText.text = bossMook.GetBossName();
        bossBase = boss.GetComponent<BaseMook>();
        bossBase.onMookInstanceDamaged += updateHPSlider;
        updateBossHealthText();
    }

    private void updateBossHealthText()
    {
        //Format to max of one decimal place
        bossHealthText.text = string.Format("{0:.#}/{1:.#}", bossHealthSlider.value, bossHealthSlider.maxValue);
    }

    private void OnDisable()
    {
        bossBase.onMookInstanceDamaged -= updateHPSlider;
    }

    private void updateHPSlider(float newHP)
    {
        bossHealthSlider.value = newHP;
        updateBossHealthText();
    }
}
