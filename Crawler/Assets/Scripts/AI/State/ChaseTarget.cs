using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

class ChaseTarget : IState
{
    Transform _target;
    NavMeshAgent _chasingAgent;
    public delegate void TargetReachedStateChange(bool state);
    public event TargetReachedStateChange OnTargetReachedStateChange;
    public ChaseTarget(Transform target, NavMeshAgent chasingAgent)
    {
        _target = target;
        _chasingAgent = chasingAgent;
    }
    public void OnStateEnter()
    {
        _chasingAgent.enabled = true;
        OnTargetReachedStateChange?.Invoke(false);
        //Possible animation system triggering here later on
    }

    public void OnStateExit()
    {
        _chasingAgent.enabled = false;
        OnTargetReachedStateChange?.Invoke(true);
    }

    public void Tick()
    {
        _chasingAgent.SetDestination(_target.position);
    }
}

