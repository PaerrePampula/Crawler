using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] float explosionDamage = 10;
    [SerializeField] float explosionRadius = 2.5f;
    [SerializeField] float explosionTimer = 0.8f;
    [SerializeField] LayerMask mookLayer;
    [SerializeField] GameObject explosionEffect;
    [SerializeField] AudioClip explosionSound;
    void DoExplosionHits()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRadius, mookLayer);
        //The damage is calculated using the bomb damage + 30% of player bonus damage
        float damage = (explosionDamage + Player.Singleton.getBonusDamage(explosionDamage)*0.3f);
        for (int i = 0; i < hits.Length; i++)
        {
            hits[i].GetComponent<BaseMook>().ChangeHp(-damage);
        }
    }
    private void OnEnable()
    {
        StartCoroutine(startBombCountdown());
    }
    IEnumerator startBombCountdown()
    {
        float timer = 0;
        while (timer <= explosionTimer)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        DoExplosionHits();
        GameObject effect = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        if (explosionSound != null)
        {
            effect.GetComponent<AudioSource>().PlayOneShot(explosionSound);
        }

        Destroy(gameObject);
    }
}
