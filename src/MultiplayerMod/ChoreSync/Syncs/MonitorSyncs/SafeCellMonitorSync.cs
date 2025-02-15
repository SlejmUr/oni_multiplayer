namespace MultiplayerMod.ChoreSync.Syncs.MonitorSyncs;

internal class SafeCellMonitorSync : BaseChoreSync<SafeCellMonitor>
{
    public override Type SyncType => typeof(SafeCellMonitor);

    public override void Client(StateMachine instance)
    {
        Setup(instance);
        StateMachine.danger.ToggleChore(null, null);
    }

    public override void Server(StateMachine instance)
    {

    }
}
