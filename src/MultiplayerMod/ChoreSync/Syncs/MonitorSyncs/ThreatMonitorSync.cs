namespace MultiplayerMod.ChoreSync.Syncs.MonitorSyncs;

internal class ThreatMonitorSync : BaseChoreSync<ThreatMonitor>
{
    public override Type SyncType => typeof(ThreatMonitor);

    public override void Client(StateMachine instance)
    {
        Setup(instance);
        StateMachine.threatened.duplicant.ShouldFight.ToggleChore(null, null);
    }

    public override void Server(StateMachine instance)
    {

    }
}
