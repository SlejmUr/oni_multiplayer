using MultiplayerMod.ChoreSync.StateMachines;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.Chores;

[Serializable]
public class MoveObjectToCellCommand : BaseCommandEvent
{
    public static ParameterInfo<int> TargetCell = new(
        "__move_to_target_cell",
        defaultValue: Grid.InvalidCell
    );

    public TypedResolver<StateMachine.Instance> reference { get; }
    public string movingStateName { get; }
    public int cell { get; }

    public MoveObjectToCellCommand(TypedResolver<StateMachine.Instance> reference, int cell, StateMachine.BaseState movingState) :
        this(reference, cell, movingState?.name)
    { }

    public MoveObjectToCellCommand(TypedResolver<StateMachine.Instance> reference,int cell, StateInfo movingStateInfo) :
        this(reference, cell, movingStateInfo?.ReferenceName)
    { }

    public MoveObjectToCellCommand(TypedResolver<StateMachine.Instance> reference, int cell, string movingStateName)
    {
        this.movingStateName = movingStateName;
        this.reference = reference;
        this.cell = cell;
    }
}
