using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

class SlimeBehaviour : MonoBehaviour
{
    #region Fields and properties

    [Header("Slime AI chase parameters")]
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
    //AI behaviours
    //AI for attack sequence
    [SerializeField] SlimeAttack slimeAttack;
    //Ai for chase sequence (player too far etc.)
    ChaseTarget chaseTarget;
    #endregion

    void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _stateMachine = new StateMachine();
        _baseMook = GetComponent<BaseMook>();

        chaseTarget = new ChaseTarget(PlayerController.Singleton.transform, _navAgent);
        chaseTarget.OnTargetReachedStateChange += updateChaseState;
        slimeAttack.InitializeSlimeAttack(GetComponent<CharacterController>(), transform, _baseMook, _layersToCastAgainstOnAttack);
        //Add transistions for statemachine
        _stateMachine.AddTransistion(slimeAttack, chaseTarget, targetReached(PlayerController.Singleton.transform, transform));
        _stateMachine.AddTransistion(chaseTarget, slimeAttack, targetTooFar(PlayerController.Singleton.transform, transform), true);
        //Set default state to chase player in state machine
        _stateMachine.SetState(chaseTarget);
    }
    private void OnEnable()
    {

    }
    private void OnDisable()
    {
        chaseTarget.OnTargetReachedStateChange -= updateChaseState;
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

