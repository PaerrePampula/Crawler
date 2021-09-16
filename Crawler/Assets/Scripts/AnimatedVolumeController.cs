using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Adds subscriptions to animated volume controller,
/// which adds some post processing effects to the camera based
/// on game events e.g player death, player damage, player hit, etc.
/// </summary>
public class AnimatedVolumeController : MonoBehaviour
{
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Player.onPlayerDamaged += playPlayerHitEffect;
    }
    private void OnDisable()
    {
        Player.onPlayerDamaged -= playPlayerHitEffect;
    }

    private void playPlayerHitEffect()
    {
        animator.Play("PlayerHit-VolumeEffect");
    }
}
