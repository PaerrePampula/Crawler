using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamageOnHit : MonoBehaviour
{
    [SerializeField] float damage = 10;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<IDamageable>().ChangeHp(-damage + Player.Singleton.getBonusDamage(-damage));
        }
        Destroy(gameObject);
    }
}
