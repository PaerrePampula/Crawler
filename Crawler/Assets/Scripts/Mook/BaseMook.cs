using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Saves all basic information about enemies, like hp and such.
/// </summary>
public class BaseMook : MonoBehaviour, IDamageable
{
    #region events
    public delegate void MookDamaged(float amount, Vector3 location);
    public static event MookDamaged onMookDamaged;
    public delegate void MookDeath();
    public event MookDeath onMookDeath;
    public delegate void MookDeathSpawnItem(Vector3 position);
    public static event MookDeathSpawnItem onMookPossibleItemDrop;
    #endregion

    #region fields
    [Header("VFX")]
    [SerializeField] GameObject damageEffect;
    [SerializeField] GameObject dieEffect;


    [Header("RPG parameters")]
    [SerializeField] float _maxHP = 3;
    float _hp;
    bool isInvulnerable = false;


    #endregion

    public float Hp
    {
        get 
        { 
            return _hp; 
        }
        set
        {
            _hp = value;

            if (_hp <= 0)
            {
                KillCharacter();
            }
        }
    }

    public bool ChangeHp(float damageAmount)
    {
        if (!isInvulnerable)
        {
            onMookDamaged?.Invoke(damageAmount, transform.position);
            Hp += damageAmount;
            Instantiate(damageEffect, transform.position += Vector3.up * 0.5f, transform.rotation = Quaternion.Euler(30, 0, 0));
            return true;
        }
        return false;
    }

    public void KillCharacter()
    {
        //TODO: Animation in own class (some sort of animation manager).
        //TODO: Might also be its own class, loot has nothing to with AI
        onMookDeath?.Invoke();
        onMookPossibleItemDrop?.Invoke(transform.position);
        Instantiate(dieEffect, transform.position += Vector3.up * 0.5f, transform.rotation = Quaternion.Euler(30, 0, 0));
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    private void OnEnable()
    {

        //Set the unset hp to be the max hp of the character
        Hp = _maxHP;

    }
    private void Awake()
    {
        Room room = transform.root.GetComponent<Room>();
        if (room != null)
        {
            room.AddMookToRoom(this);
        }

    }


}
