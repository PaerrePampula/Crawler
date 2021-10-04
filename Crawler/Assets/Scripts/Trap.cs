using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    bool timerBased = true;
    [SerializeField] float timeBetweenNewTriggers;
    Animator animator;
    [SerializeField] Collider damageCollider;
    [SerializeField] ChildOnTrigger onTrigger;
    private void Awake()
    {

        animator = GetComponent<Animator>();
        if (onTrigger != null)
        {
            onTrigger.delegatesOnTrigger += DoDamageOnTrap();
        }
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
        if (timerBased)
        {
            StartCoroutine(AiActionWaiter.actionWait(() => TriggerTrap(), Time.time + timeBetweenNewTriggers));
        }
    }
    private void TriggerTrap()
    {
        animator.SetTrigger("TriggerTrap");
    }
    //For traps with triggers on parent
    private void OnTriggerEnter(Collider other)
    {
        TriggerPlayerHPLose(other);
    }
    //For traps with triggers on child
    private Action<Collider> DoDamageOnTrap() => (other) =>
    {
        TriggerPlayerHPLose(other);
    };

    private static void TriggerPlayerHPLose(Collider other)
    {
        Player player = other.GetComponent<Player>();
        if (other.GetComponent<Player>() != null)
        {
            player.ChangeHp(-1f);
        }
    }
}
