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

    public Action attackReady;
    public Action attackHitEnd;
    public Action updateAttack;
    Animator _animator;
    AudioSource _audioSource;
    BaseMook _baseMook;
    StateOnAnimationTrigger _animationTrigger;

    [Header("FX")]
    [SerializeField] AudioClip[] _windupAttackSounds;
    [SerializeField] AudioClip _attackHitSuccessSound;
    [SerializeField] string _attackAnimationWindupStateName;
    [SerializeField] GameObject attackEffect;


    [Header("Attack information")]
    [SerializeField] Vector2 hitboxSize;
    [SerializeField] float attackDamage;
    [SerializeField] float attackDelayMinimum;
    [SerializeField] float attackDelayMaximum;

    string _attackName;
    bool readyToChangeState = true;
    float lastAttackTime = Mathf.Infinity;
    Coroutine attackHitBoxRoutine;
    GameObject attackEffectInstance;
    public Vector2 HitboxSize { get => hitboxSize; set => hitboxSize = value; }

    Coroutine windupRoutine;

    public event StateComplete onStateComplete;

    public void InitializeMeleeStrike(Animator animator, AudioSource audioSource, BaseMook baseMook, StateOnAnimationTrigger stateOnAnimationTrigger, Func<float, Collider[]> colliderFunc, string attackName = "BasicAttack")
    {
        hitBoxAttainMethod = colliderFunc;
        _animator = animator;
        _audioSource = audioSource;
        _baseMook = baseMook;
        _animationTrigger = stateOnAnimationTrigger;
        _attackName = attackName;
    }
    public void OnStateEnter()
    {
        windupRoutine = _baseMook.StartCoroutine(ActionDelayer.actionWait(() => ReadyMeleeStrike(), Time.time + UnityEngine.Random.Range(attackDelayMinimum, attackDelayMaximum)));

    }
    void ReadyMeleeStrike()
    {
        readyToChangeState = false;
        if (_windupAttackSounds.Length > 0)
        {
            int randomWindupSoundIndex = UnityEngine.Random.Range(0, _windupAttackSounds.Length);
            _audioSource.PlayOneShot(_windupAttackSounds[randomWindupSoundIndex]);
        }

        _animator.SetTrigger(_attackAnimationWindupStateName);
        if (attackEffect)
        {
            attackEffectInstance = GameObject.Instantiate(attackEffect, _baseMook.transform);
            attackEffectInstance.transform.localPosition = -Orientation.getHeadingVectorFor(_baseMook.transform.position, PlayerController.Singleton.transform.position) + Vector3.up;
        }

        attackReady?.Invoke();
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
                bool hasHit = damageable.ChangeHp(-attackDamage);
                if (hasHit) _baseMook.InvokeSuccessfullHit(_attackName);
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
        _animationTrigger.onTriggerState -= CastAttackHitBox;
        _animationTrigger.onTriggerStateLeave -= StopCastingHitBox;
        if (attackHitBoxRoutine != null)
        {
            _baseMook.StopCoroutine(attackHitBoxRoutine);
            attackHitBoxRoutine = null;

        }

        readyToChangeState = true;

        GameObject.Destroy(attackEffectInstance);
        attackHitEnd?.Invoke();
        onStateComplete?.Invoke();
    }

    public void OnStateExit()
    {
        if (windupRoutine != null) _baseMook.StopCoroutine(windupRoutine);


    }

    public bool StateReadyToTransistion()
    {
        return readyToChangeState;
    }

    public void Tick()
    {
        updateAttack?.Invoke();
    }


}
