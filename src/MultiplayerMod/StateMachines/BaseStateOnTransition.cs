
namespace MultiplayerMod.StateMachines;

internal abstract class BaseStateOnTransition
{
    public virtual void ServerCallback(StateMachine.Instance smi)
    {
        var chore = (Chore) smi.GetMaster();
        var goToStack = HelperForStates.GetGoToStack(smi);
        var newState = goToStack.FirstOrDefault();
    }

    public virtual void Client(StateMachine.BaseState @base)
    {
        @base.enterActions.RemoveAll(action => action.name.Contains("Transition"));
        @base.updateActions.RemoveAll(action => action.buckets.Any(bucket => bucket.name.Contains("Transition")));
    }

    public virtual void Server(StateMachine sm, string StateToMonitorName)
    {
        var @base = HelperForStates.GetMonitoredState(sm, StateToMonitorName);
        // Transition being trigger upon state exit.
        var method = @base.GetType().GetMethods().First(m => m.Name == "Exit" && m.GetParameters().Length == 2);

        var dlgt = Delegate.CreateDelegate(
            method.GetParameters()[1].ParameterType,
            this.GetType().GetMethod(nameof(ServerCallback))
            );
        method.Invoke(@base, ["Trigger Multiplayer event", dlgt]);
    }
}
