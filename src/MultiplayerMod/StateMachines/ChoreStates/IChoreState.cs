using MultiplayerMod.StateMachines.BaseStates;

namespace MultiplayerMod.StateMachines.ChoreStates;

internal interface IChoreState
{
    public Type ChoreType { get; }

    public List<IBaseState> States { get; set; }
}
