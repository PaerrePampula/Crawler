using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardFireball : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    Rigidbody rigidbody;
    LayerMask hitLayerMask;
    float _damage;
    float _fireballSpeed;
    public void InitializeFireball(Vector3 fireDir, LayerMask _hitLayerMask, float damage, float fireballSpeed)
    {
        hitLayerMask = _hitLayerMask;
        _damage = damage;
        _fireballSpeed = fireballSpeed;
        rigidbody.AddForce(fireDir * fireballSpeed);
    }
    private void OnEnable()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((hitLayerMask.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Player>().ChangeHp(-_damage);

            }
            if (hitEffect != null)
            {
                GameObject go = Instantiate(hitEffect, transform.position, transform.rotation);
            }

            Destroy(this.gameObject);
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
