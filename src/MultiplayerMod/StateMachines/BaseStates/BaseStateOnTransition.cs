using MultiplayerMod.Commands.Chores;

namespace MultiplayerMod.StateMachines.BaseStates;

internal class BaseStateOnTransition(string stateToMonitor) : IBaseState
{
    public string StateToMonitor => stateToMonitor;
    public virtual void ServerCallback(StateMachine.Instance smi)
    {
        var chore = (Chore) smi.GetMaster();
        var goToStack = StateHelper.GetGoToStack(smi);
        var newState = goToStack.FirstOrDefault();
        new ChoreTransitStateArgs(chore, newState?.name, []);
    }

    public virtual void Client(StateMachine sm)
    {
        var @base = StateHelper.GetMonitoredState(sm, StateToMonitor);
        @base.enterActions.RemoveAll(action => action.name.Contains("Transition"));
        @base.updateActions.RemoveAll(action => action.buckets.Any(bucket => bucket.name.Contains("Transition")));
    }

    public virtual void Server(StateMachine sm)
    {
        var @base = StateHelper.GetMonitoredState(sm, StateToMonitor);
        // Transition being trigger upon state exit.
        var method = @base.GetType().GetMethods().First(m => m.Name == "Exit" && m.GetParameters().Length == 2);

        var dlgt = Delegate.CreateDelegate(
            method.GetParameters()[1].ParameterType,
            GetType().GetMethod(nameof(ServerCallback))
            );
        method.Invoke(@base, ["Trigger Multiplayer event", dlgt]);
    }
}
