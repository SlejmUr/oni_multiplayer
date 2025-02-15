using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Network.Common;

namespace MultiplayerMod.ChoreSync.Syncs;

internal class IdleChoreSync : BaseChoreSync<IdleChore.States>
{
    public override Type SyncType => typeof(IdleChore);

    public override void Server(StateMachine instance)
    {
        Setup(instance);
        StateMachine.idle.move.Enter(smi =>
        {
            var cell = smi.GetIdleCell();
            MultiplayerManager.Instance.NetServer.Send(
                new MoveObjectToCellCommand(new ChoreStateMachineResolver(smi.master), cell, StateMachine.idle.move),
                MultiplayerCommandOptions.SkipHost
            );
        });
        StateMachine.idle.move.Exit(smi =>
        {
            MultiplayerManager.Instance.NetServer.Send(
                new GoToStateCommand(new ChoreStateMachineResolver(smi.master), StateMachine.idle),
                MultiplayerCommandOptions.SkipHost
            );
            MultiplayerManager.Instance.NetServer.Send(
                new SynchronizeObjectPositionCommand(smi.gameObject),
                MultiplayerCommandOptions.SkipHost
            );
        });
    }

    public override void Client(StateMachine instance)
    {
        Setup(instance);

        StateMachine.idle.onfloor.ToggleScheduleCallback("", null, null);
        StateMachine.idle.onladder.ToggleScheduleCallback("", null, null);
        StateMachine.idle.ontube.Update("", null, 0, false);
        StateMachine.idle.onsuitmarker.ToggleScheduleCallback("", null, null);
        StateMachine.idle.move.Transition(null, null, 0);
        StateMachine.idle.move.MoveTo(null, null, null, false);

        var targetCell = AddMultiplayerParameter<int, IdleChore.States.IntParameter>(MoveObjectToCellCommand.TargetCell);
        StateMachine.idle.move.MoveTo(targetCell.Get, StateMachine.idle, StateMachine.idle);
    }
}
