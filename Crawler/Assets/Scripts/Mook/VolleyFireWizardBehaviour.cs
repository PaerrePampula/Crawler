using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VolleyFireWizardBehaviour : WizardBehaviour
{

    [SerializeField] VolleyFireBallAttack VolleyfireBallAttack;

    protected override void Awake()
    {
        fireBallAttack = VolleyfireBallAttack;
        fireBallAttack.onStateComplete += returnToChase;
        base.Awake();
    }

    private void returnToChase()
    {
        _stateMachine.SetState(chaseTarget);
    }
}
