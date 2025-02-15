namespace MultiplayerMod.StateMachines.States;

internal class ContinuationState<StateMachineType, StateMachineInstanceType, MasterType, DefType> : StateMachine<StateMachineType,
    StateMachineInstanceType, MasterType, DefType>.State
    where StateMachineInstanceType : StateMachine.Instance
    where MasterType : IStateMachineTarget
{
    public ContinuationState(StateMachine sm, StateMachine.BaseState original)
    {
        // enter and update actions must be synced differently.
        exitActions = original.exitActions;
        defaultState = original.defaultState;

        var stateMachineType = sm.GetType();
        // we are getting the root State here
        var root = stateMachineType.GetField(nameof(StateMachineMemberReference.root)).GetValue(sm);
        // calling BindState with root_state as root, state as this state and name of this state
        stateMachineType.GetMethod(nameof(StateMachineMemberReference.BindState))!.Invoke(sm, [root, this, "ContinuationState"]);
    }
}
