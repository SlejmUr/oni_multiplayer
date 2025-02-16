using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Core;
using MultiplayerMod.Events.Chores;

namespace MultiplayerMod.Multiplayer.Chores;

internal static class BindChoreSend
{
    internal static void ChoreCreatedEvent_Call(ChoreCreatedEvent @event)
    {
        MultiplayerManager.Instance.NetServer.Send(new CreateChoreCommand(@event.Id, @event.Type, @event.Arguments), Network.Common.MultiplayerCommandOptions.SkipHost);
    }
}
