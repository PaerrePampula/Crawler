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

    ChooseAttackState attackChooser;

    //Basic attack class + its delegates
    [SerializeField] MookMeleeStrike rightHook;
    [SerializeField] AttainHitBoxWithOverLapBox hitboxAttainmentForBasicAttack;
    [SerializeField] StateActionCooldown cooldownForBasicAttack;
    //"Ground smash" attack + its delegates
    [SerializeField] MookMeleeStrike pummelStrike;
    [SerializeField] AttainHitBoxWithCircle hitBoxAttainmentForPummelStrike;
    [SerializeField] StateActionCooldown cooldownForPummelStrike;
    //Tornado attack and its delegates
    ChaseTarget tornadoAttackChaseTarget;
    [SerializeField] AoEPassiveAttack tornadoAoE;
    [SerializeField] AttainHitBoxWithCircle tornadoHitBoxCircle;
    [SerializeField] StateActionCooldown cooldownForTornadoAttack;
    //Range attack and its delegates
    [SerializeField] VolleyFireBallAttack volleyFireBallAttack;
    [SerializeField] StateActionCooldown cooldownForFireballAttack;

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
        tornadoAttackChaseTarget = new ChaseTarget(PlayerController.Singleton.transform, _navAgent, () => _baseMook.isCharacterGrounded());

        //Initialize variables for delegates
        hitboxAttainmentForBasicAttack.InitializeVariables( _layersToCastAgainstOnAttack, transform);
        hitBoxAttainmentForPummelStrike.InitializeVariables(_layersToCastAgainstOnAttack, transform);
        tornadoHitBoxCircle.InitializeVariables(_layersToCastAgainstOnAttack, transform);


        //Initialize the actual attacks
        volleyFireBallAttack.InitializeFireBallAttack(PlayerController.Singleton.transform, _baseMook, _animator, _audioSource);
        rightHook.InitializeMeleeStrike(_animator, _audioSource, _baseMook, stateOnAnimationTrigger, hitboxAttainmentForBasicAttack.getHitBoxes());
        pummelStrike.InitializeMeleeStrike(_animator, _audioSource, _baseMook, stateOnAnimationTrigger, hitBoxAttainmentForPummelStrike.getHitBoxes(), "AoE");
        tornadoAoE.InitializeAoEPassiveAttack(tornadoHitBoxCircle.getHitBoxes(),tornadoAttackChaseTarget.EndStateManual ,  _baseMook, tornadoAttackChaseTarget, "Tornado");

        //Add in delegates for special attacks
        pummelStrike.attackReady = hitBoxAttainmentForPummelStrike.displayHitBoxWarning();
        pummelStrike.updateAttack = hitBoxAttainmentForPummelStrike.updateHitBoxWarning();
        pummelStrike.attackHitEnd += hitBoxAttainmentForPummelStrike.stopWarning();
        pummelStrike.attackHitEnd += cooldownForPummelStrike.setCooldown();
        tornadoAttackChaseTarget.onCharacterChase += cooldownForTornadoAttack.setCooldown();
        tornadoAttackChaseTarget.onCharacterChase += () => tornadoAoE.StartAOE();
        volleyFireBallAttack.OnAttackStart +=  cooldownForFireballAttack.setCooldown();
        volleyFireBallAttack.stateEnterAction += () => _animator.SetTrigger("Transform");
        tornadoAttackChaseTarget.onCharacterChase += () => _animator.SetTrigger("Transform");
        tornadoAttackChaseTarget.onCharacterChaseExit += () => _animator.SetTrigger("EndTransform");


        rightHook.attackHitEnd += cooldownForBasicAttack.setCooldown();

        //Initialize the attack chooser class
        List<(IState, StateActionCooldown)> attacksForAttackChooser = new List<(IState, StateActionCooldown)>();

        attacksForAttackChooser.Add((pummelStrike, cooldownForPummelStrike));
        attacksForAttackChooser.Add((tornadoAttackChaseTarget, cooldownForTornadoAttack));
        attacksForAttackChooser.Add((volleyFireBallAttack, cooldownForFireballAttack));
        attacksForAttackChooser.Add((rightHook, cooldownForBasicAttack));

        attackChooser = new ChooseAttackState(ref _stateMachine, attacksForAttackChooser, _baseMook);


        //Make transistions to different states
        chaseTarget = new ChaseTarget(PlayerController.Singleton.transform, _navAgent, () => _baseMook.isCharacterGrounded());
        chaseTarget.OnTargetReachedStateChange += updateChaseState;
        chaseTarget.onCharacterChaseUpdate += () => _animator.SetFloat("CharacterSpeed", _navAgent.velocity.magnitude);
        chaseTarget.onCharacterChaseExit += () => _animator.SetFloat("CharacterSpeed", 0);

        _stateMachine.AddTransistion(attackChooser, chaseTarget, targetReached(PlayerController.Singleton.transform, transform));
        _stateMachine.AddTransistionFromAnyState(chaseTarget, targetTooFar(PlayerController.Singleton.transform, transform), true);
        _stateMachine.SetState(attackChooser);
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
