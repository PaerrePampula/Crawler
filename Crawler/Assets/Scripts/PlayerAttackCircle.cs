using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCircle : MonoBehaviour
{
    Animator anim;
    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        PlayerWeapon.onPlayerAttacks += doAttackFX;
    }
    private void OnDisable()
    {
        PlayerWeapon.onPlayerAttacks -= doAttackFX;
    }
    private void doAttackFX()
    {
        anim.Play("AttackCircle-LightAttack");
    }
}
