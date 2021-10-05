using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkSpeedController : MonoBehaviour
{
    Animator animator;
    float currentSpeed = 1;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Player.Singleton.AddDelegateOnStatBuff(StatType.MovementSpeed, changeAnimationSpeed);
    }


    private void changeAnimationSpeed(float speed)
    {
        currentSpeed += speed;
        animator.SetFloat("PlayerRunSpeed", currentSpeed);
    }
}
