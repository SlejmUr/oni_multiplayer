namespace MultiplayerMod.ChoreSync;

internal abstract class BaseChoreSync<ChoreStateMachine> : IChoreSync
    where ChoreStateMachine : StateMachine
{
    public abstract Type SyncType { get; }
    public ChoreStateMachine StateMachine { get; internal set; }

    public abstract void Client(StateMachine instance);
    public abstract void Server(StateMachine instance);

    public void Setup(StateMachine __instance)
    {
        StateMachine = __instance as ChoreStateMachine;
    }
}
