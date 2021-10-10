using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
[Serializable]
class AoEPassiveAttack
{
    Coroutine aoeDamageRoutine;
    public Func<float, Collider[]> hitBoxAttainMethod;
    Action hitBoxEndFunction;
    BaseMook _baseMook;
    [SerializeField] float attackLength = 3f;
    IState baseState;
    [SerializeField] float attackDamage;
    //TODO: REPLACE WITH ANIMATION
    [SerializeField] string attackSpriteTrigger;
    string _attackName;
    Animator _animator;
    public void InitializeAoEPassiveAttack(Func<float, Collider[]> hitBoxAttainMethod, Action endFunction ,BaseMook baseMook, IState baseState, string attackName)
    {
        this.hitBoxAttainMethod = hitBoxAttainMethod;
        _baseMook = baseMook;
        this.baseState = baseState;
        this.hitBoxEndFunction = endFunction;
        _animator = _baseMook.GetComponentInChildren<Animator>();
        _attackName = attackName;
    }

    public void StartAOE()
    {
        _animator.SetTrigger(attackSpriteTrigger);
        aoeDamageRoutine = _baseMook.StartCoroutine(checkAoEForDamage());
    }
    IEnumerator checkAoEForDamage()
    {
        Collider[] hitColliders;
        float angleOfStrike = -Orientation.headingAngleFor(_baseMook.transform.position, PlayerController.Singleton.transform.position);
        float timer = 0;
        while (timer <= attackLength)
        {

            hitColliders = hitBoxAttainMethod(angleOfStrike);
            for (int i = 0; i < hitColliders.Length; i++)
            {
                IDamageable damageable = (IDamageable)hitColliders[i].GetComponent(typeof(IDamageable));
                bool success = damageable.ChangeHp(-attackDamage);
                if (success) _baseMook.InvokeSuccessfullHit(_attackName);
            }

            timer += Time.deltaTime;
            yield return null;
        }
        StopCastingHitBox();

    }
    void StopCastingHitBox()
    {
        _animator.SetTrigger(attackSpriteTrigger);
        _baseMook.StopAllCoroutines();
        hitBoxEndFunction?.Invoke();
    }
}


