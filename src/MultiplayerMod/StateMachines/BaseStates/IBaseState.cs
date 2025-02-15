namespace MultiplayerMod.StateMachines.BaseStates;

internal interface IBaseState
{
    public string StateToMonitor { get; }
    public void Client(StateMachine sm);
    public void Server(StateMachine sm);
}
