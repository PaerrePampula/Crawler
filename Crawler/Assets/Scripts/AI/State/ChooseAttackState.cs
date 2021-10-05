using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ChooseAttackState : IState
{
    //This class is made for bosses, and these bosses work with the smallest elements
    //in the list getting the highest priority, not by random.
    //the smallest members get chosen if the cooldowns are no more.
    //otherwise the largest member (basic attack) is chosen.
    List<(IState, StateActionCooldown)> statesToChooseFrom = new List<(IState, StateActionCooldown)>();
    StateMachine _stateMachine;
    IState chosenState;
    BaseMook _baseMook;

    public event StateComplete onStateComplete;

    public ChooseAttackState(ref StateMachine stateMachine, List<(IState, StateActionCooldown)> iStates, BaseMook baseMook)
    {
        _stateMachine = stateMachine;
        statesToChooseFrom = iStates;
        _baseMook = baseMook;
    }

    public void OnStateEnter()
    {
        chosenState = this;
    }

    private void ChooseNewState()
    {
        if (chosenState != null && chosenState != this)
        {
            chosenState.onStateComplete -= ReturnToThisState;
        }

        //get new attack based on cooldowns (with priority to smaller elements)
        for (int i = 0; i < statesToChooseFrom.Count; i++)
        {
            if ((statesToChooseFrom[i].Item2 == null) || statesToChooseFrom[i].Item2.CooldownPassed().Invoke() == true)
            {
                chosenState = statesToChooseFrom[i].Item1;
                //Add a delay to attacks, to give the player a bigger chance of winning the boss fight
                _baseMook.StartCoroutine(ActionDelayer.actionWait(() => SetNewState(), Time.time+ 1.2f));
                break;
            }

        }


    }

    private void SetNewState()
    {
        //set the state to the new attack
        _stateMachine.SetState(chosenState);
        //Unsubscribe old state attack end

        //Subscribe to the attack end to choose new attack (or to transistion to something else during this new state)
        chosenState.onStateComplete += ReturnToThisState;
    }

    void ReturnToThisState()
    {
        chosenState = this;
        _stateMachine.SetState(this);
    }
    public void OnStateExit()
    {
        
    }

    public bool StateReadyToTransistion()
    {
        return true;
    }

    public void Tick()
    {
        if (chosenState == this) ChooseNewState();
    }


} 

