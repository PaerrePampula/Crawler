using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Simply controls the Health ui with event driven logic.
/// </summary>
public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentHpText;
    [SerializeField] TextMeshProUGUI maxHPText;
    [SerializeField] Image radialVisualSlider;
    [SerializeField] Gradient radialSliderColor;
    float maxSliderValue;
    Coroutine sliderChangeCoroutine;

    private void OnEnable()
    {
        Player.onCurrentHpChanged += updateHP;
        Player.onMaxHPChanged += updateMaxHP;
    }
    private void OnDisable()
    {
        Player.onCurrentHpChanged -= updateHP;
        Player.onMaxHPChanged -= updateMaxHP;
    }

    private void updateMaxHP(float newHP, float changeAmount)
    {
        maxSliderValue = newHP;
        maxHPText.text = newHP.ToString();
    }

    private void updateHP(float newHP, float changeAmount)
    {
        currentHpText.text =  newHP.ToString("0.##");

        if (sliderChangeCoroutine != null) StopCoroutine(sliderChangeCoroutine);
        StartCoroutine(changeSliderValue(newHP / maxSliderValue));
    }
    IEnumerator changeSliderValue(float newValue)
    {
        float oldReferenceValue = radialVisualSlider.fillAmount;
        float timer = 0;
        while (timer < 0.5f)
        {
            timer += Time.deltaTime;
            radialVisualSlider.fillAmount = Mathf.Lerp(oldReferenceValue, newValue, timer / 0.5f);
            radialVisualSlider.color = radialSliderColor.Evaluate(radialVisualSlider.fillAmount);
            yield return null;
        }
        radialVisualSlider.fillAmount = newValue;
    }
}
