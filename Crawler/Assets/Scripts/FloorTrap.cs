using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTrap : MonoBehaviour
{
    bool timerBased;
    [SerializeField] float timeBetweenNewTriggers;
    Animator animator;
    Collider damageCollider;
    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        if (timerBased)
        {
            StartCoroutine(AiActionWaiter.actionWait(() => TriggerTrap(), Time.time + timeBetweenNewTriggers));
        }
    }
    public void StartTrap()
    {
        damageCollider.enabled = true;
    }
    public void StopTrap()
    {
        damageCollider.enabled = false;
    }
    private void TriggerTrap()
    {
        animator.SetTrigger("TriggerTrap");
    }
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (other.GetComponent<Player>() != null)
        {
            player.ChangeHp(-1f);
        }
    }
}
