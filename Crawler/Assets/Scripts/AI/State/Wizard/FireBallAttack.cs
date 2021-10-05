using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class FireBallAttack : ActionDelayer, IState
{
    protected Transform _target;
    protected BaseMook _baseMook;
    [SerializeField] protected LayerMask _hitLayers;
    protected Animator _animator;
    protected AudioSource _audioSource;
    //Information to animator
    //SetTrigger can be used to trigger any animation transistion to a state named by the parameter
    //meaning another ranged mook, that isnt necessary a wizard, and has a different animator can possibly run this as well.
    [SerializeField] string attackAnimationTriggerName = "FireBallAttack";
    [SerializeField] protected GameObject fireballPrefab;
    Coroutine waitForAction;
    Coroutine coolDownForAttack;
    [SerializeField] protected float fireballSpeed;
    [SerializeField] protected float fireballDamage;
    protected bool readyToChangeState = true;
    [SerializeField] protected  AudioClip castingAudioClip;
    [SerializeField] AudioClip telegraphingWindupSound;
    [SerializeField] float attackCycleWaitTimeMinimum;
    [SerializeField] float attackCycleWaitTimeMaximum;
    [SerializeField] int maxAttackUses;
    int currentAttackUses;
    [SerializeField] float attacksUsedCoolDown = 2f;
    float lastAttackTime = Mathf.Infinity;
    //Delegates
    public delegate void OnFireballAttackCoolDownStart(float cooldownLength, int maxAttacks);
    public event OnFireballAttackCoolDownStart onFireballCoolDown;
    public delegate void OnAttack(int attacksLeft);
    public event OnAttack onAttack;
    public Action OnAttackStart;
    public Action stateEnterAction;
    public event StateComplete onStateComplete;

    public void InitializeFireBallAttack(Transform target, BaseMook baseMook, Animator animator, AudioSource audioSource = null)
    {
        _target = target;
        _baseMook = baseMook;
        _animator = animator;
        _audioSource = audioSource;
        currentAttackUses = maxAttackUses;

    }
    public void OnStateEnter()
    {
        stateEnterAction?.Invoke();
        DoAttack();

    }

    protected void DoAttack()
    {
        //The wizard can only attack set number of times, make sure that the AI can attack, 
        //If the AI cannot attack, reset the attacks to the cooldown amount
        if (currentAttackUses <= 0)
        {
            //If the AI re-enters this state, the AI might try to start "cooldowning" again
            if (coolDownForAttack == null)
            {
                onFireballCoolDown?.Invoke(attacksUsedCoolDown, maxAttackUses);
                coolDownForAttack = _baseMook.StartCoroutine(actionWait(() => ResetUses(), Time.time + attacksUsedCoolDown));
            }

        }
        else
        {
            //If the character can attack, the attack action is ran through a coroutine, that adds the attack delay
            //if the chracter attacked before
            if (lastAttackTime == Mathf.Infinity) lastAttackTime = Time.time;
            if (waitForAction != null) _baseMook.StopCoroutine(waitForAction);
            waitForAction = _baseMook.StartCoroutine(actionWait(() => TriggerAttack(), lastAttackTime));
        }


    }
    void ResetUses()
    {
        //Reset all attack uses to the initial state
        currentAttackUses = maxAttackUses;
        lastAttackTime = Time.time + UnityEngine.Random.Range(attackCycleWaitTimeMinimum, attackCycleWaitTimeMaximum);
        //Pretend the player attacked, this is added on top of the cooldown, to make the AI even less brutal
        DoAttack();
        //Now that the cooldown has been reset, make sure to null the coroutine, to allow new cooldowns
        coolDownForAttack = null;
    }

    private void TriggerAttack()
    {
        OnAttackStart?.Invoke();
        lastAttackTime = Time.time + UnityEngine.Random.Range(attackCycleWaitTimeMinimum, attackCycleWaitTimeMaximum);
        _animator.gameObject.GetComponent<StateOnAnimationTrigger>().onTriggerState += AttackWithFireBall;
        //only triggers the animator and sound, animation triggers event
        //which this system subscribes to.
        _animator.SetTrigger(attackAnimationTriggerName);
        _audioSource?.PlayOneShot(telegraphingWindupSound);
        readyToChangeState = false;

    }

    protected virtual void AttackWithFireBall()
    {
        _audioSource?.PlayOneShot(castingAudioClip);
        Vector3 projectileDirection = (_target.position - _baseMook.transform.position).normalized;
        //The projectiles sometimes go down in very weird and janky directions, so reset the fireball to always travel on the same level on the Y as the character itself
        //(By setting the y to be 0 for the rigid body force apply)
        projectileDirection.y = 0;
        GameObject go = GameObject.Instantiate(fireballPrefab, _baseMook.transform.position + projectileDirection, Quaternion.identity);
        go.GetComponent<WizardFireball>().InitializeFireball(projectileDirection, _hitLayers, fireballDamage, fireballSpeed);
        readyToChangeState = true;
        _animator.gameObject.GetComponent<StateOnAnimationTrigger>().onTriggerState -= AttackWithFireBall;
        currentAttackUses--;
        onAttack?.Invoke(currentAttackUses);
        DoAttack();
    }

    public void OnStateExit()
    {

        readyToChangeState = true;
        if (waitForAction != null) _baseMook.StopCoroutine(waitForAction);
    }
    protected void InvokeStateComplete()
    {
        onStateComplete?.Invoke();
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