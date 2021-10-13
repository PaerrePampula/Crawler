using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
/// <summary>
/// Displays player damage ingame as a floating text.
/// </summary>
public class DamageDisplayer : MonoBehaviour
{
    [SerializeField] GameObject damageDisplayPrefab;
    private void OnEnable()
    {
        BaseMook.onMookDamaged += displayDamage;
    }
    private void OnDisable()
    {
        //Events need to be unsubscribed to prevent errors after decommissioning of gameobjects
        BaseMook.onMookDamaged -= displayDamage;
    }

    private void displayDamage(float amount, Vector3 location, bool wasCritical = false)
    {
        float randomRotation = UnityEngine.Random.Range(-17f, 17f);
        GameObject textClone = Instantiate(damageDisplayPrefab, location, Quaternion.identity);
        textClone.transform.rotation = Quaternion.Euler(0, 0, randomRotation);
        textClone.GetComponentInChildren<TextMeshPro>().text = Math.Abs(amount).ToString("0");
        if (wasCritical) textClone.GetComponent<Animator>().Play("Critical");
    }
}
