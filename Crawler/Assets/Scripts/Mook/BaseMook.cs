using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseMook : MonoBehaviour, IDamageable
{
    //TODO: CHASE
    public delegate void MookDamaged(float amount, Vector3 location);
    public static event MookDamaged onMookDamaged;
    NavMeshAgent navAgent;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _maxHP = 3;
    float _hp;
    bool isInvulnerable = false;
    [SerializeField] float _chaseRange;
    [SerializeField] float _reChaseRange;
    //Has player as target.
    bool hasTarget = false;
    bool hasReachedTarget = false;
    bool newStateTriggered = false;
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

    public void ChangeHp(float damageAmount)
    {
        if (!isInvulnerable)
        {
            onMookDamaged?.Invoke(damageAmount, transform.position);
            Hp += damageAmount;
        }
    }

    public void KillCharacter()
    {
        //TODO: Animation in own class (some sort of animation manager).
        //TODO: Might also be its own class, loot has nothing to with AI
        Destroy(gameObject);
    }
    public virtual void DoAIThing()
    {
        //Do the action, and eventually set the state not to be triggered again.
        //(To allow new triggers of attacking the player, etc.)
    }
    public void ChasePlayer()
    {
        //If the bool for has reached target is set to true, first reset it
        hasReachedTarget = false;
        //There is obviously a target for the agent now, so save this as well.
        hasTarget = true;

        //Debug.Log("chasing");
        Debug.DrawLine(this.transform.position, PlayerController.Singleton.transform.position);
        navAgent.SetDestination(PlayerController.Singleton.transform.position);
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        Hp = _maxHP;
    }
    void Start()
    {
        //Mook needs to have a navmeshagent to pathfind on standard unity 3d navmesh.
        navAgent = GetComponent<NavMeshAgent>();
        //Chase range can be set to be infinite, meaning the mook wont move to attack player.
        if (_chaseRange == 0)
        {
            _chaseRange = Mathf.Infinity;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float distanceBetweenPlayerAndAI = Vector3.Distance(PlayerController.Singleton.transform.position, transform.position);
        //So basicly, the required current max distance until a new chase is going to be the rechase distance, if the player has reached
        //the player before, but hasnt started chasing the player again.
        //If the AI has not chased the player before, the distance will be _chaseRange
        float requiredMaxDistance = (hasReachedTarget == true) ? _reChaseRange : _chaseRange;
        //AI has no player as target at the moment (as pathfinding target).
        //No target case, generate target for ai if ai is too far
        if (!hasTarget)
        {
            if (distanceBetweenPlayerAndAI > requiredMaxDistance)
            {
                ChasePlayer();
            }


        }
        //has target case, check if the AI is close enough and reset related booleans
        else if (distanceBetweenPlayerAndAI < requiredMaxDistance)
        {
            hasTarget = false;
            hasReachedTarget = true;
        }

        //The ai might have reached the player, so the AI needs to do its action now.
        //The AI might already be attacking, so check if a new state was already triggered.
        if (!newStateTriggered && hasReachedTarget) DoAIThing();


    }
}
