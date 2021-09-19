using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[System.Serializable]

class SlimeAttack : IState
{
    public delegate void WhiffedAttackOnDodgedPlayer();
    public static event WhiffedAttackOnDodgedPlayer onAttackWhiff;
    [SerializeField] float chargeDuration = 1f;
    [SerializeField] float chargePower = 2f;
    [SerializeField] float chargeHbRadius = 1f;
    [SerializeField] float meleeDamage = 1f;
    [SerializeField] float actionDelay = 0.25f;
    bool playerDodgedAttackSuccessfully = false;
    Vector3 chargeDirection;
    CharacterController _controller;
    Transform _transform;
    BaseMook _baseMook;
    LayerMask _playerMask;
    Coroutine attackRoutine;
    Coroutine chargeRoutine;
    public void InitializeSlimeAttack(CharacterController controller, Transform transform, BaseMook baseMook, LayerMask playerMask)
    {
        _controller = controller;
        _playerMask = playerMask;
        _baseMook = baseMook;
        _transform = transform;
        _playerMask = playerMask;
    }

    public void OnStateEnter()
    {

        SlimeCharge();
    }

    public void OnStateExit()
    {
        if (attackRoutine != null) _baseMook.StopCoroutine(attackRoutine);
        if (chargeRoutine != null) _baseMook.StopCoroutine(chargeRoutine);
    }
    public void SlimeCharge()
    {
        chargeRoutine = _baseMook.StartCoroutine(chargeAttack());
        attackRoutine = _baseMook.StartCoroutine(chargeMove());
    }
    IEnumerator chargeAttack()
    {
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
        float actionDelaytimer = 0;
        while (actionDelaytimer <= actionDelay)
        {
            actionDelaytimer += Time.deltaTime;
            yield return null;
        }

        SlimeCharge();

    }
    IEnumerator chargeMove()
    {
        chargeDirection = new Vector3(PlayerController.Singleton.transform.position.x - _transform.position.x, 0, PlayerController.Singleton.transform.position.z - _transform.position.z);
        float chargeTimer = 0;
        Vector3 reference = chargeDirection;
        while (chargeTimer < chargeDuration)
        {
            chargeDirection = Vector3.Lerp(reference, Vector3.zero,chargeTimer/chargeDuration);
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
        _controller.Move(chargeDirection * Time.deltaTime * chargePower);
        _transform.position = new Vector3(_transform.position.x, 2, _transform.position.z);

    }
}
