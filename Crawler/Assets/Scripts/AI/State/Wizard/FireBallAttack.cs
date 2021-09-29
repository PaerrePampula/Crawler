using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class FireBallAttack : AiActionWaiter, IState
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
    float attacksUsedCoolDown = 2f;
    float lastAttackTime = Mathf.Infinity;
    public delegate void OnFireballAttackCoolDownStart(float cooldownLength, int maxAttacks);
    public event OnFireballAttackCoolDownStart onFireballCoolDown;
    public delegate void OnAttack(int attacksLeft);
    public event OnAttack onAttack;
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
        DoAttack();

    }

    protected void DoAttack()
    {
        if (currentAttackUses <= 0)
        {
            onFireballCoolDown?.Invoke(attacksUsedCoolDown, maxAttackUses);
            coolDownForAttack = _baseMook.StartCoroutine(actionWait(() => ResetUses(), Time.time + attacksUsedCoolDown));
        }
        else
        {
            if (lastAttackTime == Mathf.Infinity) lastAttackTime = Time.time;
            if (waitForAction != null) _baseMook.StopCoroutine(waitForAction);
            waitForAction = _baseMook.StartCoroutine(actionWait(() => TriggerAttack(), lastAttackTime));
        }


    }
    void ResetUses()
    {
        currentAttackUses = maxAttackUses;
        lastAttackTime = Time.time + UnityEngine.Random.Range(attackCycleWaitTimeMinimum, attackCycleWaitTimeMaximum);
        DoAttack();
    }

    private void TriggerAttack()
    {
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

    public void Tick()
    {

    }
    //Needs to have waited the attack delay to transistion
    public bool StateReadyToTransistion()
    {
        return readyToChangeState;
    }
}