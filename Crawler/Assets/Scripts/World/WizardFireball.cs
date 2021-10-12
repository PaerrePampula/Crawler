using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Controls the actual fireball shot by the wizards
/// </summary>
public class WizardFireball : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    Rigidbody rigidbody;
    LayerMask hitLayerMask;
    float _damage;

    public void InitializeFireball(Vector3 fireDir, LayerMask _hitLayerMask, float damage, float fireballSpeed)
    {
        hitLayerMask = _hitLayerMask;
        _damage = damage;
        rigidbody.AddForce(fireDir * fireballSpeed);
    }
    private void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, 10);
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((hitLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {

            if (other.GetComponent(typeof(IDamageable)) != null)
            {
                IDamageable damageable = (IDamageable)other.GetComponent(typeof(IDamageable));
                if (other.CompareTag("Player"))
                {
                    Player player = other.GetComponent<Player>();
                    if (!player.isInvurnerable())
                    {
                        damageable.ChangeHp(-_damage);
                        CreateHitEffects();
                    }
                }
                else
                {

                    damageable.ChangeHp(-_damage);
                    CreateHitEffects();
                }


            }

            else
            {
                CreateHitEffects();
            }

        }

    }

    private void CreateHitEffects()
    {
        if (hitEffect != null)
        {
            GameObject go = Instantiate(hitEffect, transform.position, transform.rotation);
        }
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
