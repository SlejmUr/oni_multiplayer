using OniMP.Commands.NetCommands;
using OniMP.Core;
using OniMP.Core.Player;
using OniMP.Events.Common;

namespace OniMP.Multiplayer.EventCalls;

internal class MPBothCalls
{
    internal static void OnCurrentPlayerInitialized(CurrentPlayerInitializedEvent @event)
    {
        MultiplayerManager.Instance.NetClient.Send(new RequestPlayerStateChangeCommand(@event.Player.Id, PlayerState.Loading), Network.Common.MultiplayerCommandOptions.OnlyHost);
        if (@event.Player.Role == PlayerRole.Server)
            MultiplayerManager.Instance.NetServer.Send(new ChangePlayerStateCommand(@event.Player.Id, PlayerState.Ready));
        else
            MultiplayerManager.Instance.NetClient.Send(new RequestWorldSyncCommand(), Network.Common.MultiplayerCommandOptions.OnlyHost);
    }
}
