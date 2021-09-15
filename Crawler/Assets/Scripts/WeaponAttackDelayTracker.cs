using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// A script for the ui element tracking how far the player is from being able to attack again
/// </summary>
public class WeaponAttackDelayTracker : MonoBehaviour
{
    PlayerWeapon PlayerWeapon;
    Image image;
    private void OnEnable()
    {
        PlayerWeapon = transform.root.GetComponent<PlayerWeapon>();
        image = GetComponent<Image>();
        PlayerWeapon.onPlayerAttacks += displayDelayOnBar;


    }
    private void OnDisable()
    {
        PlayerWeapon.onPlayerAttacks -= displayDelayOnBar;
    }
    void displayDelayOnBar()
    {
        float newDelay = PlayerWeapon.getCurrentAttackDelay();
        StartCoroutine(fillImageWithDelay(newDelay));
    }
    IEnumerator fillImageWithDelay(float newDelay)
    {
        float timer = 0f;
        while (timer <= newDelay)
        {
            timer += Time.deltaTime;
            image.fillAmount = timer / newDelay;
            yield return null;
        }
        image.fillAmount = 0f;
    }
}
