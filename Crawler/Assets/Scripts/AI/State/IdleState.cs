using System.Collections;
using UnityEngine;


public class IdleState : IState
{
    public event StateComplete onStateComplete;

    public void OnStateEnter()
    {

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

    }

}
