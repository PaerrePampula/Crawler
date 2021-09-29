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
        base.Awake();
    }

}
