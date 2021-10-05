using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class ChaseTarget : IState
{
    Transform _target;
    NavMeshAgent _chasingAgent;
    public delegate void TargetReachedStateChange(bool state);
    public event TargetReachedStateChange OnTargetReachedStateChange;
    public event StateComplete onStateComplete;
    public Action onCharacterChase;
    public Action onCharacterChaseUpdate;
    public Action onCharacterChaseExit;
    public ChaseTarget(Transform target, NavMeshAgent chasingAgent)
    {
        _target = target;
        _chasingAgent = chasingAgent;
    }
    public void OnStateEnter()
    {
        _chasingAgent.enabled = true;
        OnTargetReachedStateChange?.Invoke(false);
        onCharacterChase?.Invoke();
        //Possible animation system triggering here later on
    }

    public void OnStateExit()
    {
        _chasingAgent.enabled = false;
        OnTargetReachedStateChange?.Invoke(true);
        onCharacterChaseExit?.Invoke();

    }
    public void EndStateManual()
    {
        onStateComplete?.Invoke();
    }
    public void Tick()
    {

        onCharacterChaseUpdate?.Invoke();
        _chasingAgent.SetDestination(new Vector3(_target.position.x, _chasingAgent.transform.position.y, _target.position.z));
    }

    //Can always transistion from moving
    public bool StateReadyToTransistion()
    {
        return true;
    }

}

