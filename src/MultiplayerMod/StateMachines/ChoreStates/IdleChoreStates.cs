using MultiplayerMod.StateMachines.BaseStates;

namespace MultiplayerMod.StateMachines.ChoreStates;

internal class IdleChoreStates : IChoreState
{
    public List<IBaseState> States { get; set; } = [];

    public Type ChoreType => typeof(IdleChore);

    public IdleChoreStates()
    {
        //States.Add(new BaseStateOnUpdate(nameof(IdleChore.States.idle.ontube)));
        States.Add(new BaseStateOnExit(StateHelper.GetChainedStateName(nameof(IdleChore.States.idle.ontube))));
        States.Add(new BaseStateOnTransition(StateHelper.GetChainedStateName(nameof(IdleChore.States.idle.move))));
        //States.Add(new BaseStateOnMove(nameof(IdleChore.States.idle.move)));
    }
}
