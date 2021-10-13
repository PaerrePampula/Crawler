using System;
using System.Collections.Generic;
using TMPro;
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
    AudioSource _audioSource;
    //Ai for chase sequence (player too far etc.)
    ChaseTarget chaseTarget;
    RandomWander attackWander;
    IdleState _idleState;
    [SerializeField] TextMeshPro stateText;
    #endregion

    void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _stateMachine = new StateMachine();
        _baseMook = GetComponent<BaseMook>();
        _audioSource = GetComponent<AudioSource>();
        chaseTarget = new ChaseTarget(PlayerController.Singleton.transform, _navAgent, () => _baseMook.isCharacterGrounded());
        attackWander = new RandomWander(GetComponent<CharacterController>(), _baseMook);
        _idleState = new IdleState();
        chaseTarget.OnTargetReachedStateChange += updateChaseState;
        slimeAttack.InitializeSlimeAttack(GetComponent<CharacterController>(), transform, _baseMook, _layersToCastAgainstOnAttack,GetComponentInChildren<Animator>() , _audioSource);
        //Add transistions for statemachine
        _stateMachine.AddTransistion(slimeAttack, chaseTarget, targetReached(PlayerController.Singleton.transform, transform));
        _stateMachine.AddTransistion(chaseTarget, slimeAttack, targetTooFar(PlayerController.Singleton.transform, transform));
        _stateMachine.AddTransistion(attackWander, slimeAttack, targetReached(PlayerController.Singleton.transform, transform), true);
        _stateMachine.AddTransistion(chaseTarget, attackWander, wanderComplete(attackWander));
        _stateMachine.AddTransistion(chaseTarget, _idleState, GenericStateFuncs.metAggroRange(_baseMook.AggroRange, PlayerController.Singleton.transform, transform));
        //Set default state to chase player in state machine
        _stateMachine.SetState(_idleState);
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
        stateText.text = _stateMachine.getCurrentState().GetType().ToString();
    }
    float getChaseRange()
    {
        return (hasReachedTargetAndHasNotYetStartedChasingAgain == true) ? _reChaseRange : _chaseRange;
    }

    Func<bool> targetReached(Transform target, Transform thisTransform) => () => Math.Abs(Vector3.Distance(target.position, thisTransform.position)) < getChaseRange();
    Func<bool> targetTooFar(Transform target, Transform thisTransform) => () => !targetReached(target, thisTransform).Invoke();
    Func<bool> wanderComplete(RandomWander wander) => () => wander.WanderIsDone();
}

