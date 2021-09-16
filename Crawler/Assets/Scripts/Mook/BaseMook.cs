using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Saves all basic information about enemies, like hp, chase behaviour and such.
/// </summary>
public class BaseMook : MonoBehaviour, IDamageable
{
    #region events
    public delegate void MookDamaged(float amount, Vector3 location);
    public static event MookDamaged onMookDamaged;
    public delegate void MookDeath();
    public event MookDeath onMookDeath;
    #endregion

    #region fields
    [Header("VFX")]
    [SerializeField] GameObject damageEffect;
    [SerializeField] GameObject dieEffect;

    NavMeshAgent navAgent;
    Vector3 setTargetPosition;
    Vector3 newTargetPosition;
    [Header("Chase parameters")]
    [SerializeField] float _chaseRange;
    [SerializeField] float _reChaseRange;
    [SerializeField] float _moveSpeed;
    //Has player as target.
    bool hasTarget = false;
    bool hasReachedTarget = false;
    [Header("Attack/RPG parameters")]
    [SerializeField] float _maxHP = 3;
    float _hp;

    float lastActionTime = 0;
    [SerializeField] float actionSpeed = 2f;
    [SerializeField] protected LayerMask playerMask;

    bool isInvulnerable = false;
    public bool canMove = true;

    protected bool newStateTriggered = false;
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
            Instantiate(damageEffect, transform.position, transform.rotation);
            return true;
        }
        return false;
    }

    public void KillCharacter()
    {
        //TODO: Animation in own class (some sort of animation manager).
        //TODO: Might also be its own class, loot has nothing to with AI
        onMookDeath?.Invoke();
        Instantiate(dieEffect, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    public virtual void DoAIThing()
    {
        newStateTriggered = true;
        lastActionTime = Time.time;
        navAgent.ResetPath();
        Debug.Log("Did Ai thing");
        //Do the action, and eventually set the state not to be triggered again.
        //(To allow new triggers of attacking the player, etc.)
    }
    public void ChasePlayer()
    {
        if (canMove)
        {
            //If the bool for has reached target is set to true, first reset it
            hasReachedTarget = false;
            //There is obviously a target for the agent now, so save this as well.
            hasTarget = true;

            //Debug.Log("chasing");
            setTargetPosition = PlayerController.Singleton.transform.position;
            Debug.DrawLine(transform.position, setTargetPosition);
            navAgent.SetDestination(setTargetPosition);
        }

    }
    // Start is called before the first frame update
    private void OnEnable()
    {

        //Mook needs to have a navmeshagent to pathfind on standard unity 3d navmesh.
        navAgent = GetComponent<NavMeshAgent>();
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
    void Start()
    {

        //Chase range can be set to be infinite, meaning the mook wont move to attack player.
        if (_chaseRange == 0)
        {
            _chaseRange = Mathf.Infinity;
        }
    }


    // Update is called once per frame
    void Update()
    {
        newTargetPosition = PlayerController.Singleton.transform.position;
        float distanceBetweenPlayerAndAI = Vector3.Distance(newTargetPosition, transform.position);
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
        //has target, but the target has gained a bunch of distance from previous position
        else if (Vector3.Distance(newTargetPosition, setTargetPosition) > 1)
        {
            ChasePlayer();
        }

        if ((Time.time > lastActionTime + actionSpeed) && newStateTriggered)
        {
            newStateTriggered = false;
        }

        //The ai might have reached the player, so the AI needs to do its action now.
        //The AI might already be attacking, so check if a new state was already triggered.
        if (!newStateTriggered && hasReachedTarget) DoAIThing();
    }
}
