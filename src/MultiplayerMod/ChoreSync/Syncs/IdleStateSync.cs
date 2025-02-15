using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;
using MultiplayerMod.Network.Common;

namespace MultiplayerMod.ChoreSync.Syncs;

internal class IdleStateSync : BaseChoreSync<IdleStates>
{
    public override Type SyncType => typeof(IdleStates);

    public override void Client(StateMachine instance)
    {
        Setup(instance);
        StateMachine.loop.ToggleScheduleCallback(null, null, null);
        StateMachine.move.Enter(null);

        var targetCell = AddMultiplayerParameter<int, IdleStates.IntParameter>(MoveObjectToCellCommand.TargetCell);
        StateMachine.move.Enter(smi => {
            var navigator = smi.GetComponent<Navigator>();
            navigator.GoTo(targetCell.Get(smi));
        });
    }

    public override void Server(StateMachine instance)
    {
        Setup(instance);
        StateMachine.move.Enter(smi => {
            var target = smi.GetComponent<Navigator>().targetLocator;
            var cell = Grid.PosToCell(target);

            // TODO: Remove after critters sync (WorldGenSpawner.Spawnable + new critters)
            if (MultiplayerManager.Instance.MPObjects.Get(smi.master.gameObject) == null)
                return;

            MultiplayerManager.Instance.NetServer.Send(
                new MoveObjectToCellCommand(new StateMachineResolver(smi.controller.GetComponentResolver(), smi.GetType()), cell, StateMachine.move),
                MultiplayerCommandOptions.SkipHost
            );
        });
        StateMachine.move.Exit(smi => {

            // TODO: Remove after critters sync (WorldGenSpawner.Spawnable + new critters)
            if (MultiplayerManager.Instance.MPObjects.Get(smi.master.gameObject) == null)
                return;

            MultiplayerManager.Instance.NetServer.Send(
                new GoToStateCommand(new StateMachineResolver(smi.controller.GetComponentResolver(), smi.GetType()), StateMachine.loop),
                MultiplayerCommandOptions.SkipHost
            );
            MultiplayerManager.Instance.NetServer.Send(
                new SynchronizeObjectPositionCommand(smi.gameObject),
                MultiplayerCommandOptions.SkipHost
            );
        });
    }
}
