﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

class WizardBehaviour : MonoBehaviour
{
    [Header("Wizard AI chase parameters")]
    [SerializeField] float _chaseRange;
    [SerializeField] float _reChaseRange;
    [SerializeField] float _moveSpeed;
    [Header("Other")]
    [SerializeField] LayerMask _layersToCastAgainstOnAttack;

    bool hasReachedTargetAndHasNotYetStartedChasingAgain = false;

    //For controlling some events, tracking hp maybe.
    BaseMook _baseMook;
    //Behaviour script, so needs a state machine
    StateMachine _stateMachine;
    //Moving AI, so needs a navAgent
    NavMeshAgent _navAgent;
    [SerializeField] Animator animator;
    //Needed to play back audio on attacking.
    AudioSource _audioSource;

    [SerializeField] FireBallAttack fireBallAttack;
    ChaseTarget chaseTarget;
    void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _stateMachine = new StateMachine();
        _baseMook = GetComponent<BaseMook>();
        _audioSource = GetComponent<AudioSource>();
        fireBallAttack.InitializeFireBallAttack(PlayerController.Singleton.transform, _baseMook, animator, _audioSource);
        chaseTarget = new ChaseTarget(PlayerController.Singleton.transform, _navAgent);
        chaseTarget.OnTargetReachedStateChange += updateChaseState;

        //Add transistions for statemachine
        _stateMachine.AddTransistion(fireBallAttack, chaseTarget, targetReached(PlayerController.Singleton.transform, transform));
        _stateMachine.AddTransistion(chaseTarget,fireBallAttack, targetTooFar(PlayerController.Singleton.transform, transform), true);
        //Set default state to chase player in state machine
        _stateMachine.SetState(chaseTarget);
    }
    private void updateChaseState(bool state)
    {
        hasReachedTargetAndHasNotYetStartedChasingAgain = state;
    }
    private void Update()
    {
        _stateMachine.Tick();
    }
    float getChaseRange()
    {
        return (hasReachedTargetAndHasNotYetStartedChasingAgain == true) ? _reChaseRange : _chaseRange;
    }

    Func<bool> targetReached(Transform target, Transform thisTransform) => () => Math.Abs(Vector3.Distance(target.position, thisTransform.position)) < getChaseRange();
    Func<bool> targetTooFar(Transform target, Transform thisTransform) => () => !targetReached(target, thisTransform).Invoke();
}

