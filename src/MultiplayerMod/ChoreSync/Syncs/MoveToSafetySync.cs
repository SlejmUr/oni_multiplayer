using MultiplayerMod.ChoreSync.StateMachines;
using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Network.Common;

namespace MultiplayerMod.ChoreSync.Syncs;

internal class MoveToSafetySync : BaseChoreSync<MoveToSafetyChore.States>
{
    public override Type SyncType => typeof(MoveToSafetyChore);

    readonly StateInfo movingStateInfo = new (name: "__moving");
    public override void Client(StateMachine instance)
    {
        var waiting = AddState<MoveToSafetyChore.States.State, MoveToSafetyChore.States.State>(StateMachine.root, "__waiting" , this.StateMachine.BindState);
        var moving = AddState<MoveToSafetyChore.States.State, MoveToSafetyChore.States.State>(StateMachine.root, movingStateInfo, this.StateMachine.BindState);

        var targetCell = AddMultiplayerParameter<int, MoveToSafetyChore.States.IntParameter>(MoveObjectToCellCommand.TargetCell);

        moving.MoveTo(targetCell.Get, waiting, waiting, true);

        StateMachine.root.Enter(smi => smi.GoTo(waiting));
    }

    public override void Server(StateMachine instance)
    {
        StateMachine.move.Enter(smi => {
            MultiplayerManager.Instance.NetServer.Send(
                new MoveObjectToCellCommand(new ChoreStateMachineResolver(smi.master), smi.targetCell, movingStateInfo),
                MultiplayerCommandOptions.SkipHost
            );
        });

        StateMachine.move.Update((smi, _) => {
            MultiplayerManager.Instance.NetServer.Send(
                new MoveObjectToCellCommand(new ChoreStateMachineResolver(smi.master), smi.targetCell, movingStateInfo),
                MultiplayerCommandOptions.SkipHost
            );
        });

        StateMachine.move.Exit(smi => {
            MultiplayerManager.Instance.NetServer.Send(
                new GoToStateCommand(new ChoreStateMachineResolver(smi.master), (StateMachine.BaseState?) null),
                MultiplayerCommandOptions.SkipHost
            );
            MultiplayerManager.Instance.NetServer.Send(
                new SynchronizeObjectPositionCommand(smi.gameObject),
                MultiplayerCommandOptions.SkipHost
            );
        });
    }
}
