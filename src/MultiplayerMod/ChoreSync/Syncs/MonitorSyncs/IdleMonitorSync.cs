namespace MultiplayerMod.ChoreSync.Syncs.MonitorSyncs;

internal class IdleMonitorSync : BaseChoreSync<IdleMonitor>
{
    public override Type SyncType => typeof(IdleMonitor);

    public override void Client(StateMachine instance)
    {
        Setup(instance);
        StateMachine.idle.ToggleChore(null, null);
    }

    public override void Server(StateMachine instance)
    {

    }
}
