using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class RockBoss : MonoBehaviour
{
    [Header("Rock boss chase parameters")]
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
    [SerializeField] TextMeshPro stateText;

    bool gizmo_gameStarted = false;
    ChaseTarget chaseTarget;
    [SerializeField] MookMeleeStrike rightHook;
    [SerializeField] AttainHitBoxWithOverLapBox hitboxAttainmentForBasicAttack;
    [SerializeField] MookMeleeStrike pummelStrike;
    [SerializeField] AttainHitBoxWithCircle hitBoxAttainmentForPummelStrike;
    private void Awake()
    {
        gizmo_gameStarted = true;
        _animator = GetComponentInChildren<Animator>();
        stateOnAnimationTrigger = GetComponentInChildren<StateOnAnimationTrigger>();
        _navAgent = GetComponent<NavMeshAgent>();
        _stateMachine = new StateMachine();
        _baseMook = GetComponent<BaseMook>();
        _audioSource = GetComponent<AudioSource>();


        hitboxAttainmentForBasicAttack.InitializeVariables( _layersToCastAgainstOnAttack, transform);
        hitBoxAttainmentForPummelStrike.InitializeVariables(_layersToCastAgainstOnAttack, transform);
        rightHook.InitializeMeleeStrike(_animator, _audioSource, _baseMook, stateOnAnimationTrigger, hitboxAttainmentForBasicAttack.getHitBoxes());
        pummelStrike.InitializeMeleeStrike(_animator, _audioSource, _baseMook, stateOnAnimationTrigger, hitBoxAttainmentForPummelStrike.getHitBoxes());

        pummelStrike.attackSpecialEffects = hitBoxAttainmentForPummelStrike.displayHitBoxWarning();
        pummelStrike.attackSpecialEffectsUpdate = hitBoxAttainmentForPummelStrike.updateHitBoxWarning();
        pummelStrike.attackSpecialEffectsEnd = hitBoxAttainmentForPummelStrike.stopWarning();
        chaseTarget = new ChaseTarget(PlayerController.Singleton.transform, _navAgent);
        chaseTarget.OnTargetReachedStateChange += updateChaseState;
        _stateMachine.AddTransistion(pummelStrike, chaseTarget, targetReached(PlayerController.Singleton.transform, transform));
        _stateMachine.AddTransistion(chaseTarget, pummelStrike, targetTooFar(PlayerController.Singleton.transform, transform), true);
        _stateMachine.SetState(chaseTarget);
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
            Gizmos.matrix = Matrix4x4.TRS(_baseMook.transform.position, Quaternion.Euler(0, -Orientation.headingAngleFor(_baseMook.transform.position, PlayerController.Singleton.transform.position) +90, 0), _baseMook.transform.lossyScale);
            Gizmos.DrawWireCube(_baseMook.transform.TransformDirection(new Vector3(0, 0, -1 * rightHook.HitboxSize.x / 2f)), new Vector3(rightHook.HitboxSize.x, 1, rightHook.HitboxSize.y));
        }


    }
    Func<bool> targetReached(Transform target, Transform thisTransform) => () => Math.Abs(Vector3.Distance(target.position, thisTransform.position)) < getChaseRange();
    Func<bool> targetTooFar(Transform target, Transform thisTransform) => () => !targetReached(target, thisTransform).Invoke();


}
