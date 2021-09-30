using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[System.Serializable]
class MookMeleeStrike : IState
{

    //The collider attainment method is only passed as a func pointer, making the switching of the way of attaining the hit characters
    //a variable (some attacks might get their hitboxes using squares, some might use circles, so different methods are needed)
    public Func<float,Collider[]> hitBoxAttainMethod;
    public Action attackSpecialEffects;
    public Action attackSpecialEffectsEnd;
    public Action attackSpecialEffectsUpdate;
    Animator _animator;
    AudioSource _audioSource;
    BaseMook _baseMook;
    StateOnAnimationTrigger _animationTrigger;

    [Header("FX")]
    [SerializeField] AudioClip _windupAttackSound;
    [SerializeField] AudioClip _attackHitSuccessSound;
    [SerializeField] string _attackAnimationWindupStateName;
    [SerializeField] GameObject attackEffect;


    [Header("Attack information")]
    [SerializeField] Vector2 hitboxSize;
    [SerializeField] float attackDamage;
    [SerializeField] float attackDelayMinimum;
    [SerializeField] float attackDelayMaximum;

    bool readyToChangeState = true;
    float lastAttackTime = Mathf.Infinity;
    Coroutine attackHitBoxRoutine;
    GameObject attackEffectInstance;
    public Vector2 HitboxSize { get => hitboxSize; set => hitboxSize = value; }

    Coroutine windupRoutine;
    public void InitializeMeleeStrike(Animator animator, AudioSource audioSource, BaseMook baseMook, StateOnAnimationTrigger stateOnAnimationTrigger, Func<float, Collider[]> colliderFunc)
    {
        hitBoxAttainMethod = colliderFunc;
        _animator = animator;
        _audioSource = audioSource;
        _baseMook = baseMook;
        _animationTrigger = stateOnAnimationTrigger;
    }
    public void OnStateEnter()
    {
        readyToChangeState = false;
        if (lastAttackTime == Mathf.Infinity) lastAttackTime = Time.time + attackDelayMinimum;
        windupRoutine = _baseMook.StartCoroutine(AiActionWaiter.actionWait(() => ReadyMeleeStrike(), lastAttackTime));
    }
    void ReadyMeleeStrike()
    {


        lastAttackTime = Time.time + UnityEngine.Random.Range(attackDelayMinimum, attackDelayMaximum);
        _audioSource.PlayOneShot(_windupAttackSound);
        _animator.SetTrigger(_attackAnimationWindupStateName);

        attackEffectInstance = GameObject.Instantiate(attackEffect, _baseMook.transform);
        attackEffectInstance.transform.localPosition = -Orientation.getHeadingVectorFor(_baseMook.transform.position, PlayerController.Singleton.transform.position) + Vector3.up;
        attackSpecialEffects?.Invoke();
        _animationTrigger.onTriggerState += CastAttackHitBox;
        _animationTrigger.onTriggerStateLeave += StopCastingHitBox;
    }

    private void CastAttackHitBox()
    {

        attackHitBoxRoutine = _baseMook.StartCoroutine(castHitBoxLoop());
    }
    IEnumerator castHitBoxLoop()
    {
        Collider[] hitColliders;
        float angleOfStrike = -Orientation.headingAngleFor(_baseMook.transform.position, PlayerController.Singleton.transform.position);

        while (true)
        {

            hitColliders = hitBoxAttainMethod(angleOfStrike);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                IDamageable damageable = (IDamageable)hitColliders[i].GetComponent(typeof(IDamageable));
                damageable.ChangeHp(-attackDamage);
            }
            if (hitColliders.Length > 0)
            {
                StopCastingHitBox();
                break;

            }
            yield return null;
        }
    }
    void StopCastingHitBox()
    {
        if (attackHitBoxRoutine != null)
        {
            _baseMook.StopCoroutine(attackHitBoxRoutine);
            attackHitBoxRoutine = null;

        }
        attackSpecialEffectsEnd?.Invoke();
        readyToChangeState = true;
        _animationTrigger.onTriggerState -= CastAttackHitBox;
        _animationTrigger.onTriggerStateLeave -= StopCastingHitBox;
        GameObject.Destroy(attackEffectInstance);

    }

    public void OnStateExit()
    {
        if (windupRoutine != null) _baseMook.StopCoroutine(windupRoutine);
        //If the casting is still happening on the hitbox, but state changes, reset the hitbox
        StopCastingHitBox();

    }

    public bool StateReadyToTransistion()
    {
        return readyToChangeState;
    }

    public void Tick()
    {
        attackSpecialEffectsUpdate?.Invoke();
    }

}
