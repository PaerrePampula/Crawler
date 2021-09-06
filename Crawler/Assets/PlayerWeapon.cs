using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    float lastAttackTime = 0;
    //secs of delay for attack
    [SerializeField] float attackDelay = 0.3f;
    int currentAttackIndex = 0;
    int maxAttackChain;
    List<PlayerAttack> playerAttacks = new List<PlayerAttack>();
    // Start is called before the first frame update
    void Start()
    {
        maxAttackChain = playerAttacks.Count;
    }

    // Update is called once per frame
    void Update()
    {
        //Basicly if player isnt in a menu etc.
        if (Globals.PlayerCanMove)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (AttackDelayHasPassed())
                {
                    //Set the last attack time to be current passed frames.
                    lastAttackTime = Time.time;
                    //Increment chain by one, with clamping functioning
                    incrementAttackChain();
                }
            }
        }
    }
    void incrementAttackChain()
    {
        if (currentAttackIndex - 1 >= maxAttackChain)
        {
            currentAttackIndex = 0;
        }
        else
        {
            currentAttackIndex++;
        }
    }
    bool AttackDelayHasPassed()
    {
        return (Time.time > lastAttackTime + playerAttacks[currentAttackIndex].Delay);
    }
}
[System.Serializable]
class PlayerAttack
{
    float damage;
    float delay;

    public float Damage { get => damage; set => damage = value; }
    public float Delay { get => delay; set => delay = value; }
}