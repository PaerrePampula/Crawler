using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AiLimitedUseAttackTracker : MonoBehaviour
{
    [SerializeField] WizardBehaviour wizardBehaviour;
    FireBallAttack fireBallAttack;
    [SerializeField] TextMeshProUGUI fireballsLeft;
    [SerializeField] Image fireBallsCircleCoolDown;
    private void OnEnable()
    {
        fireBallAttack = wizardBehaviour.GetFireBallAttack();
        fireBallAttack.onFireballCoolDown += updateTracker;
        fireBallAttack.onAttack += updateText;
    }

    private void updateTracker(float cooldownLength, int maxAttacks)
    {
        StartCoroutine(cooldownCircle(cooldownLength, maxAttacks));
    }

    private void updateText(int attacksLeft)
    {
        fireballsLeft.text = attacksLeft.ToString();
    }


    IEnumerator cooldownCircle(float timeOfCooldown, int maxAttacks)
    {
        float timer = 0;
        while (timer <= timeOfCooldown)
        {
            timer += Time.deltaTime;
            fireBallsCircleCoolDown.fillAmount = timer / timeOfCooldown;
            yield return null;
        }
        fireBallsCircleCoolDown.fillAmount = 0;
        updateText(maxAttacks);
    }

    private void OnDisable()
    {
        fireBallAttack.onFireballCoolDown -= updateTracker;
        fireBallAttack.onAttack -= updateText;
    }

}
