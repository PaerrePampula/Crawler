using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class MrSkellingtonBehaviour : MonoBehaviour
{
    [Header("Mr skeletor man chase parameters")]
    [SerializeField] float _chaseRange;
    [SerializeField] float _reChaseRange;
    [SerializeField] float _moveSpeed;
    [Header("Other")]
    [SerializeField] LayerMask _layersToCastAgainstOnAttack;
    bool hasReachedTargetAndHasNotYetStartedChasingAgain = false;


    BaseMook _baseMook;
    StateMachine _stateMachine;
    NavMeshAgent _navAgent;
    Animator _animator;
    AudioSource _audioSource;
    StateOnAnimationTrigger stateOnAnimationTrigger;
    CharacterController _characterController;
    [SerializeField] TextMeshPro stateText;

    bool gizmo_gameStarted = false;
    ChaseTarget chaseTarget;
    RandomWander randomWander;
    IdleState _idleState;
   [SerializeField] MookMeleeStrike skellingtonStrike;
    [SerializeField] AttainHitBoxWithOverLapBox hitboxAttainmentForBasicAttack;

    private void Awake()
    {
        //get all required components
        gizmo_gameStarted = true;
        _animator = GetComponentInChildren<Animator>();
        stateOnAnimationTrigger = GetComponentInChildren<StateOnAnimationTrigger>();
        _navAgent = GetComponent<NavMeshAgent>();
        _stateMachine = new StateMachine();
        _baseMook = GetComponent<BaseMook>();
        _audioSource = GetComponent<AudioSource>();
        _characterController = GetComponent<CharacterController>();
        randomWander = new RandomWander(_characterController, _baseMook);
        _idleState = new IdleState();
        hitboxAttainmentForBasicAttack.InitializeVariables(_layersToCastAgainstOnAttack, transform);
        skellingtonStrike.InitializeMeleeStrike(_animator, _audioSource, _baseMook, stateOnAnimationTrigger, hitboxAttainmentForBasicAttack.getHitBoxes());

        chaseTarget = new ChaseTarget(PlayerController.Singleton.transform, _navAgent, () => _baseMook.isCharacterGrounded());
        chaseTarget.OnTargetReachedStateChange += updateChaseState;
        skellingtonStrike.onStateComplete += ResetState;
        _stateMachine.AddTransistion(skellingtonStrike, chaseTarget, targetReached(PlayerController.Singleton.transform, transform));
        _stateMachine.AddTransistionFromAnyState(chaseTarget, targetTooFar(PlayerController.Singleton.transform, transform), true);
        _stateMachine.AddTransistion(chaseTarget, _idleState, GenericStateFuncs.metAggroRange(_baseMook.AggroRange, PlayerController.Singleton.transform, transform));
        _stateMachine.SetState(_idleState);
    }

    private void ResetState()
    {
        _stateMachine.SetState(randomWander);
    }

    private void updateChaseState(bool state)
    {
        hasReachedTargetAndHasNotYetStartedChasingAgain = state;
    }
    float getChaseRange()
    {
        return (hasReachedTargetAndHasNotYetStartedChasingAgain == true) ? _reChaseRange : _chaseRange;
    }
    private void Update()
    {
        _stateMachine.Tick();
        stateText.text = _stateMachine.getCurrentState().GetType().ToString();

    }
    void OnDrawGizmos()
    {
        if (gizmo_gameStarted == true)
        {
            Gizmos.color = Color.red;

            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.matrix = Matrix4x4.TRS(_baseMook.transform.position, Quaternion.Euler(0, -Orientation.headingAngleFor(_baseMook.transform.position, PlayerController.Singleton.transform.position) + 90, 0), _baseMook.transform.lossyScale);
            Gizmos.DrawWireCube(_baseMook.transform.TransformDirection(new Vector3(0, 0, -1 * skellingtonStrike.HitboxSize.x / 2f)), new Vector3(skellingtonStrike.HitboxSize.x, 1, skellingtonStrike.HitboxSize.y));
        }


    }
    Func<bool> targetReached(Transform target, Transform thisTransform) => () => Math.Abs(Vector3.Distance(target.position, thisTransform.position)) < getChaseRange();
    Func<bool> targetTooFar(Transform target, Transform thisTransform) => () => !targetReached(target, thisTransform).Invoke();
}
