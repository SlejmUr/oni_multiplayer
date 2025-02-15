using MultiplayerMod.StateMachines.BaseStates;

namespace MultiplayerMod.StateMachines.ChoreStates;

internal class MingleChoreStates : IChoreState
{
    public List<IBaseState> States { get; set; } = [];

    public Type ChoreType => typeof(MingleChore);

    public MingleChoreStates()
    {
        States.Add(new BaseStateOnTransition(nameof(MingleChore.States.mingle)));
        States.Add(new BaseStateOnTransition(nameof(MingleChore.States.move)));
        States.Add(new BaseStateOnTransition(nameof(MingleChore.States.walk)));
    }
}
