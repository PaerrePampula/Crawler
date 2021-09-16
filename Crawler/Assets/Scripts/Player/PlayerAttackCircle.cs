using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Tracks the direction of the player mouse to make it more obvious in the UI where the player attacks
/// </summary>
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
