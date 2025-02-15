namespace MultiplayerMod.ChoreSync;

internal interface IChoreSync
{
    public Type SyncType { get; }
    public void Client(StateMachine instance);
    public void Server(StateMachine instance);
}
