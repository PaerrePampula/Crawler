using System;
public delegate void StateComplete();
public interface IState
{
    void Tick();
    void OnStateEnter();
    void OnStateExit();
    event StateComplete onStateComplete;
    bool StateReadyToTransistion();
}

