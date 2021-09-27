using System.Collections;
using UnityEngine;

[System.Serializable]
public class FireBallAttack : IState
{
    Transform _target;
    BaseMook _baseMook;
    [SerializeField] LayerMask _hitLayers;
    Animator _animator;
    AudioSource _audioSource;
    //Information to animator
    //SetTrigger can be used to trigger any animation transistion to a state named by the parameter
    //meaning another ranged mook, that isnt necessary a wizard, and has a different animator can possibly run this as well.
    [SerializeField] string attackAnimationTriggerName = "FireBallAttack";
    [SerializeField] GameObject fireballPrefab;
    Coroutine waitForAction;
    [SerializeField] float fireballSpeed;
    [SerializeField] float fireballDamage;
    bool readyToChangeState = true;
    [SerializeField] AudioClip castingAudioClip;
    [SerializeField] AudioClip telegraphingWindupSound;
    [SerializeField] float attackCycleWaitTimeMinimum;
    [SerializeField] float attackCycleWaitTimeMaximum;
    public void InitializeFireBallAttack(Transform target, BaseMook baseMook, Animator animator, AudioSource audioSource = null)
    {
        _target = target;
        _baseMook = baseMook;
        _animator = animator;
        _audioSource = audioSource;

    }
    public void OnStateEnter()
    {
        float randomWait = Random.Range(attackCycleWaitTimeMinimum, attackCycleWaitTimeMaximum);
        waitForAction = _baseMook.StartCoroutine(actionWait(randomWait));
        _animator.gameObject.GetComponent<StateOnAnimationTrigger>().onTriggerState += AttackWithFireBall;
        //Only trigger the animation to play, and also subscribe an event to watch for state change on the animation
        TriggerAttack();

    }

    private void TriggerAttack()
    {
        //only triggers the animator and sound, animation triggers event
        //which this system subscribes to.
        _animator.SetTrigger(attackAnimationTriggerName);
        _audioSource?.PlayOneShot(telegraphingWindupSound);
        readyToChangeState = false;
    }

    private void AttackWithFireBall()
    {
        _audioSource?.PlayOneShot(castingAudioClip);
        Vector3 projectileDirection = (_target.position - _baseMook.transform.position).normalized;
        GameObject go = GameObject.Instantiate(fireballPrefab, _baseMook.transform.position + projectileDirection, Quaternion.identity);
        go.GetComponent<WizardFireball>().InitializeFireball(projectileDirection, _hitLayers, fireballDamage, fireballSpeed);
        readyToChangeState = true;
    }
    IEnumerator actionWait(float timer)
    {
        while (true)
        {
            yield return new WaitForSeconds(timer);

            TriggerAttack();
        }


    }
    public void OnStateExit()
    {
        _animator.gameObject.GetComponent<StateOnAnimationTrigger>().onTriggerState -= AttackWithFireBall;
        readyToChangeState = true;
        _baseMook.StopCoroutine(waitForAction);
    }

    public void Tick()
    {

    }
    //Needs to have waited the attack delay to transistion
    public bool StateReadyToTransistion()
    {
        return readyToChangeState;
    }
}