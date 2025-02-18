using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Network.Common;

namespace MultiplayerMod.ChoreSync.Syncs;

internal class EatChoreSync : BaseChoreSync<EatChore.States>
{
    public override Type SyncType => typeof(EatChore);

    public override void Client(StateMachine instance)
    {
        SM.root.enterActions.Clear();
        SM.root.enterActions.Add(new("Transit to Empty State", (EatChore.StatesInstance smi) =>
        {
            smi.GoTo(EmptyChore.States.EmptyState);
        }));
        SM.eatonfloorstate.enterActions.Clear();
        SM.eatonfloorstate.enterActions.Add(new("Transit to Empty State", (EatChore.StatesInstance smi) =>
        {
            smi.GoTo(EmptyChore.States.EmptyState);
        }));
    }

    public override void Server(StateMachine instance)
    {
        Setup(instance);
        SM.root.Enter("Trigger Multiplayer Enter event", (smi) =>
        {
            Debug.Log("(EatChoreSync) Server enter root.");
            MultiplayerManager.Instance.NetServer.Send(
                new SynchronizeObjectPositionCommand(smi.gameObject),
                MultiplayerCommandOptions.SkipHost
            );
            MultiplayerManager.Instance.NetServer.Send(
                new SetParameterValueCommand(smi, SM.messstation, SM.messstation.Get(smi)),
                MultiplayerCommandOptions.SkipHost
            );
        });
        SM.eatonfloorstate.Enter("Trigger Multiplayer Enter event", (smi) =>
        {
            Debug.Log("(EatChoreSync) Server enter eatonfloorstate.");
            var stack = GetGoToStack(smi);
            Debug.Log($"Stack: {string.Join(", ", stack)}");
            MultiplayerManager.Instance.NetServer.Send(
                new SetParameterValueCommand(smi, SM.locator, SM.locator.Get(smi)),
                MultiplayerCommandOptions.SkipHost
            );
        });
    }
}
