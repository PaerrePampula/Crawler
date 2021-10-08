using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{

    //The current state is cached, only one state is active for an AI
    IState _currentState;
    //All transistions for all registered states are cached here, similar to the animator tab transistions
    Dictionary<IState, List<Transistion>> _allTransistions = new Dictionary<IState, List<Transistion>>();
    //All the transistions for state _currentState shall be cached here
    List<Transistion> _currentTransistions = new List<Transistion>();
    //All the transistions that can be transistioned from ANY state are cached here
    List<Transistion> _transistionsFromAnyState = new List<Transistion>();
    //A list of no transistions for optimization purposes of emptying the current transistions
    static List<Transistion> EmptyTransistionList = new List<Transistion>(0);
    /// <summary>
    /// Poll the "update" of the current state. If a transistion is needed, one is made, and the state is changed
    /// to another state, according to the transistion listings.
    /// </summary>
    public void Tick()
    {
        Transistion transistion = GetTransistion();
        if (transistion != null) SetState(transistion.To);

        _currentState?.Tick();
    }
    public void SetState(IState state)
    {
        //Is in this state, makes no sense to re set it, bail out!
        if (state == _currentState) return;
        if (_currentState != null)
        if (Globals.LogStates) Debug.Log("Exiting state " + _currentState.GetType().ToString());
        //Exit the currently set state
        _currentState?.OnStateExit();
        //Then set the new state as the current state
        _currentState = state;
        //Figure out the new transistion states, if there are none, set the transistions to be none
        _allTransistions.TryGetValue(_currentState, out _currentTransistions);
        //No transistions were found for this type of state, so lets set the transistions to be empty
        if (_currentTransistions == null) _currentTransistions = EmptyTransistionList;
        //New state was entered, so the enter method must be ran
        _currentState.OnStateEnter();
        if (Globals.LogStates) Debug.Log("Entering state" + _currentState.GetType().ToString());
    }

    /// <param name="to">
    /// The state from where the state transistions</param>
    /// <param name="from">
    /// The state where the transistion leads</param>
    /// <param name="predicate">
    /// The parameters (in a method form) that trigger this state change</param>
    public void AddTransistion(IState to, IState from, Func<bool> predicate, bool stateReadinessRequired = false)
    {
        //So no transistions were found in the all transistions, add this as a new list of transistions for
        //this type of states
        if (_allTransistions.TryGetValue(from, out List<Transistion> transistions) == false)
        {
            transistions = new List<Transistion>();
            _allTransistions[from] = transistions;
        }

        transistions.Add(new Transistion(to, predicate, stateReadinessRequired, from));
    }
    /// <summary>
    /// Add a transistion from any state to any other state e.g a low hp character might ALWAYS try to run,
    /// try to heal, etc over every other transistion
    /// </summary>
    public void AddTransistionFromAnyState(IState state , Func<bool> predicate, bool stateReadinessRequired = false)
    {
        _transistionsFromAnyState.Add(new Transistion(state, predicate, stateReadinessRequired));
    }

    Transistion GetTransistion()
    {
        //A matching transistion was found in transistions in any state
        foreach (Transistion transistion in _transistionsFromAnyState)
        {
            if (transistion.ConditionIsMet)
            {
                if (transistion.StateReadinessConditionIsNeeded)
                {
                    if (_currentState.StateReadyToTransistion())
                    {
                        return transistion;
                    }
                    else
                    {
                        continue;
                    }
                }
                return transistion;
            }
        }
        //A matching transistion was found in the other, current state transistions
        foreach (Transistion transistion in _currentTransistions)
        {
            if (transistion.ConditionIsMet)
            {
                if (transistion.StateReadinessConditionIsNeeded)
                {
                    if (transistion.From.StateReadyToTransistion())
                    {
                        return transistion;
                    }
                    else
                    {
                        continue;
                    }
                }

                return transistion;

            }
        }
        //No transistion was found
        return null;
    }
    public IState getCurrentState()
    {
        return _currentState;
    }
    class Transistion
    {
        public bool ConditionIsMet { get => Condition(); }
        bool stateReadinessConditionIsNeeded;
        Func<bool> Condition { get; }
        public IState To { get; }
        public IState From { get; }
        public bool StateReadinessConditionIsNeeded { get => stateReadinessConditionIsNeeded; set => stateReadinessConditionIsNeeded = value; }

        public Transistion(IState to,Func<bool> condition, bool readinessNeeded = false, IState from = null)
        {
            StateReadinessConditionIsNeeded = readinessNeeded;
            Condition = condition;
            To = to;
            From = from;
        }
    }
}

