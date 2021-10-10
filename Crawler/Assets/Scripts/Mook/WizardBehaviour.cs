using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class WizardBehaviour : MonoBehaviour
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
    protected StateMachine _stateMachine;
    //Moving AI, so needs a navAgent
    NavMeshAgent _navAgent;
    Animator _animator;
    //Needed to play back audio on attacking.
    AudioSource _audioSource;
    [SerializeField] TextMeshPro stateText;
    [SerializeField] protected FireBallAttack fireBallAttack;
    IdleState _idleState;

    protected ChaseTarget chaseTarget;

    protected virtual void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _navAgent = GetComponent<NavMeshAgent>();
        _stateMachine = new StateMachine();
        _baseMook = GetComponent<BaseMook>();
        _audioSource = GetComponent<AudioSource>();
        _idleState = new IdleState();
        fireBallAttack.InitializeFireBallAttack(PlayerController.Singleton.transform, _baseMook, _animator, _audioSource);
        chaseTarget = new ChaseTarget(PlayerController.Singleton.transform, _navAgent, () => _baseMook.isCharacterGrounded());
        chaseTarget.OnTargetReachedStateChange += updateChaseState;

        //Add transistions for statemachine
        _stateMachine.AddTransistion(fireBallAttack, chaseTarget, targetReached(PlayerController.Singleton.transform, transform));
        _stateMachine.AddTransistion(chaseTarget,fireBallAttack, targetTooFar(PlayerController.Singleton.transform, transform), true);
        _stateMachine.AddTransistion(chaseTarget, _idleState, GenericStateFuncs.metAggroRange(_baseMook.AggroRange, PlayerController.Singleton.transform, transform));
        //Set default state to chase player in state machine
        _stateMachine.SetState(_idleState);
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
    public FireBallAttack GetFireBallAttack()
    {
        return fireBallAttack;
    }
    Func<bool> targetReached(Transform target, Transform thisTransform) => () => Math.Abs(Vector3.Distance(target.position, thisTransform.position)) < getChaseRange();
    Func<bool> targetTooFar(Transform target, Transform thisTransform) => () => !targetReached(target, thisTransform).Invoke();
    Func<bool> wanderComplete(RandomWander wander) => () => wander.WanderIsDone();

}

