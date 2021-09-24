interface IState
{
    void Tick();
    void OnStateEnter();
    void OnStateExit();
    bool StateReadyToTransistion();
}

