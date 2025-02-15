using MultiplayerMod.Network.Common;

namespace MultiplayerMod.ChoreSync;

internal class IdleChoreSync : BaseChoreSync<IdleChore.States>
{
    public override Type SyncType => typeof(IdleChore);

    public override void Server(StateMachine instance)
    {
        Setup(instance);

    }

    public override void Client(StateMachine instance)
    {
        Setup(instance);
        StateMachine.idle.move.Enter(smi =>
        {
            var cell = smi.GetIdleCell();
            /*
            server.Send(
                new MoveObjectToCell(new ChoreStateMachineReference(smi.master), cell, StateMachine.idle.move),
                MultiplayerCommandOptions.SkipHost
            );
            */
        });
    }
}
