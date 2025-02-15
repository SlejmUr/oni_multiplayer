using MultiplayerMod.Commands.Chores;

namespace MultiplayerMod.StateMachines.BaseStates;

internal class BaseStateOnExit(string stateToMonitor, params string[] parameterName) : IBaseState
{
    public string StateToMonitor => stateToMonitor;
    public string[] ParameterNames => parameterName;
    public virtual void ServerCallback(StateMachine.Instance smi)
    {
        var chore = (Chore) smi.GetMaster();
        var goToStack = StateHelper.GetGoToStack(smi);
        var newState = goToStack.FirstOrDefault();
        var args = ParameterNames
            .ToDictionary(
                parameter => StateHelper.GetParameterIndex(smi, parameter),
                parameter => StateHelper.GetParameterValue(smi, parameter)
            );
        new ChoreTransitStateArgs(chore, newState?.name, args);
    }
    public void Client(StateMachine sm)
    {
        var @base = StateHelper.GetMonitoredState(sm, StateToMonitor);
        @base.enterActions.Clear();
        StateHelper.AddAndTransitToWaiStateUponEnter(@base);
    }

    public void Server(StateMachine sm)
    {
        var @base = StateHelper.GetMonitoredState(sm, StateToMonitor);
        var method = @base.GetType().GetMethods().First(m => m.Name == "Exit" && m.GetParameters().Length == 2);

        var dlgt = Delegate.CreateDelegate(
            method.GetParameters()[1].ParameterType,
            GetType().GetMethod(nameof(ServerCallback))
            );
        method.Invoke(@base, ["Trigger Multiplayer event", dlgt]);
    }
}
