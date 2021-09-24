﻿using System.Collections;
using UnityEngine;

[System.Serializable]
public class FireBallAttack : IState
{
    Transform _target;
    BaseMook _baseMook;
    [SerializeField] LayerMask _hitLayers;
    Animator _animator;
    //Information to animator
    //SetTrigger can be used to trigger any animation transistion to a state named by the parameter
    //meaning another ranged mook, that isnt necessary a wizard, and has a different animator can possibly run this as well.
    [SerializeField] string attackAnimationTriggerName = "FireBallAttack";
    [SerializeField] GameObject fireballPrefab;
    Coroutine waitForAction;
    [SerializeField] float fireballSpeed;
    [SerializeField] float fireballDamage;
    bool readyToChangeState = true;
    public void InitializeFireBallAttack(Transform target, BaseMook baseMook, Animator animator)
    {
        _target = target;
        _baseMook = baseMook;
        _animator = animator;
    }
    public void OnStateEnter()
    {
        AttackWithFireBall();
        readyToChangeState = false;
        waitForAction = _baseMook.StartCoroutine(actionWait());
    }

    private void AttackWithFireBall()
    {
        _animator.SetTrigger(attackAnimationTriggerName);
        Vector3 projectileDirection = (_target.position - _baseMook.transform.position).normalized;
        GameObject go = GameObject.Instantiate(fireballPrefab, _baseMook.transform.position + projectileDirection, Quaternion.identity);
        go.GetComponent<WizardFireball>().InitializeFireball(projectileDirection, _hitLayers, fireballDamage, fireballSpeed);
    }
    IEnumerator actionWait()
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            readyToChangeState = true;
            AttackWithFireBall();
        }


    }
    public void OnStateExit()
    {
        readyToChangeState = true;
        _baseMook.StopCoroutine(waitForAction);
    }

    public void Tick()
    {

    }
    //Needs to have waited the attack delay to transistion
    public bool StateReadyToTransistion()
    {
        return readyToChangeState;
    }
}