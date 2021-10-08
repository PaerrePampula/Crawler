using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
[System.Serializable]

class SlimeAttack : IState
{
    public delegate void WhiffedAttackOnDodgedPlayer();
    public static event WhiffedAttackOnDodgedPlayer onAttackWhiff;
    public event StateComplete onStateComplete;

    [SerializeField] float chargeDuration = 1f;
    [SerializeField] float chargePower = 2f;
    [SerializeField] float chargeHbRadius = 1f;
    [SerializeField] float meleeDamage = 1f;
    [SerializeField] float actionDelayMinimum = 0.25f;
    [SerializeField] float actionDelayMaximum = 0.85f;
    [SerializeField] AudioClip attackingAudioClip;
    float lastAttackTime = Mathf.Infinity;
    bool playerDodgedAttackSuccessfully = false;
    string _attackName;
    Vector3 chargeDirection;
    CharacterController _controller;
    Transform _transform;
    BaseMook _baseMook;
    Animator _animator;
    LayerMask _playerMask;
    AudioSource _audioSource;
    Coroutine attackRoutine;
    Coroutine chargeRoutine;
    Coroutine windupRoutine;

    bool readyToChangeState = true;
    public void InitializeSlimeAttack(CharacterController controller, Transform transform, BaseMook baseMook, LayerMask playerMask, Animator animator, AudioSource audioSource = null, string attackName = "BasicAttack")
    {
        _controller = controller;
        _playerMask = playerMask;
        _baseMook = baseMook;
        _transform = transform;
        _playerMask = playerMask;
        _animator = animator;
        _audioSource = audioSource;
        _attackName = attackName;
    }

    public void OnStateEnter()
    {
        ReadyAttack();

    }

    private void ReadyAttack()
    {

        readyToChangeState = false;

        _animator.SetTrigger("slime-windup");
        windupRoutine = _baseMook.StartCoroutine(ActionDelayer.actionWait(() => SlimeCharge(), Time.time + UnityEngine.Random.Range(actionDelayMinimum, actionDelayMaximum)));
    }


    public void OnStateExit()
    {
        readyToChangeState = true;
        if (attackRoutine != null) _baseMook.StopCoroutine(attackRoutine);
        if (chargeRoutine != null) _baseMook.StopCoroutine(chargeRoutine);
        if (windupRoutine != null) _baseMook.StopCoroutine(windupRoutine);
        chargeDirection = Vector3.zero;
        _animator.SetTrigger("slime-attack");



    }
    public void SlimeCharge()
    {

        chargeRoutine = _baseMook.StartCoroutine(chargeAttack());
        attackRoutine = _baseMook.StartCoroutine(chargeMove());
    }
    IEnumerator chargeAttack()
    {
        _audioSource?.PlayOneShot(attackingAudioClip);
        _animator.SetTrigger("slime-attack");
        float chargeAttackTimer = 0;

        while (chargeAttackTimer < chargeDuration)
        {
            chargeAttackTimer += Time.deltaTime;
            if (CreateHitBoxAndReturnHitSuccess() == true)
            {
                playerDodgedAttackSuccessfully = false;
                break;
            }
            yield return null;
        }
        if (playerDodgedAttackSuccessfully)
        {
            playerDodgedAttackSuccessfully = false;
            onAttackWhiff?.Invoke();
        }


        readyToChangeState = true;


    }
    IEnumerator chargeMove()
    {
        chargeDirection = new Vector3(PlayerController.Singleton.transform.position.x - _transform.position.x, 0, PlayerController.Singleton.transform.position.z - _transform.position.z);
        float chargeTimer = 0;
        Vector3 reference = chargeDirection;
        while (chargeTimer < chargeDuration)
        {
            Debug.DrawLine(_transform.position, _transform.position+ chargeDirection, Color.red);
            chargeDirection = Vector3.Slerp(reference, Vector3.zero,chargeTimer/chargeDuration);
            chargeTimer += Time.deltaTime;
            yield return null;
        }
        chargeDirection = Vector3.zero;

    }
    public bool CreateHitBoxAndReturnHitSuccess()
    {
        bool hasHit = false;

        Collider[] hitColliders = Physics.OverlapSphere(_transform.position, chargeHbRadius, _playerMask);
        foreach (var hitCollider in hitColliders)
        {
            IDamageable damageable = (IDamageable)hitCollider.GetComponent(typeof(IDamageable));
            hasHit = damageable.ChangeHp(-meleeDamage);
            //Will return true if there are hits that do damage
            if (hasHit) _baseMook.InvokeSuccessfullHit(_attackName);


        }
        if (!hasHit)
        {
            //If the player dashed out of the way of the hitbox,
            if (Vector3.Distance(PlayerController.Singleton.getLastDashPoint(), _transform.position) <= chargeHbRadius / 2f)
            {
                playerDodgedAttackSuccessfully = true;
            }
        }
        //Will return false if there are no hits
        return hasHit;
    }

    public void Tick()
    {
        Vector3 moveDir = new Vector3(chargeDirection.x, _baseMook.Gravity, chargeDirection.z);
        _controller.Move(_transform.TransformDirection(moveDir * Time.deltaTime * chargePower));


    }
    //Needs to have waited the attack delay to transistion
    public bool StateReadyToTransistion()
    {
        return readyToChangeState;
    }

}
