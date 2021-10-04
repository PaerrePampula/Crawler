using System.Collections;
using UnityEngine;
[System.Serializable]
class VolleyFireBallAttack : FireBallAttack
{
    [SerializeField] int volleyCount;
    [SerializeField] float delayInVolley = 0.2f;
    [SerializeField] float aimRandomnessMin;
    [SerializeField] float aimRandomnessMax;
    protected override void AttackWithFireBall()
    {
        _baseMook.StartCoroutine(doVolley());
    }

    private void FireBallAttack()
    {
        float randomInAccuracyAmount = Random.Range(aimRandomnessMin, aimRandomnessMax);
        _audioSource?.PlayOneShot(castingAudioClip);
        Vector3 projectileDirection = ((_target.position - _baseMook.transform.position) + new Vector3(randomInAccuracyAmount, 0, randomInAccuracyAmount)).normalized;
        GameObject go = GameObject.Instantiate(fireballPrefab, _baseMook.transform.position + projectileDirection, Quaternion.identity);
        go.GetComponent<WizardFireball>().InitializeFireball(projectileDirection, _hitLayers, fireballDamage, fireballSpeed);
        readyToChangeState = true;
        _animator.gameObject.GetComponent<StateOnAnimationTrigger>().onTriggerState -= AttackWithFireBall;
        
    }

    IEnumerator doVolley()
    {
        int shotVolleys = 0;
        while (shotVolleys < volleyCount)
        {

            FireBallAttack();
            yield return new WaitForSeconds(delayInVolley);
            shotVolleys++;
        }
        InvokeStateComplete();

    }
}

