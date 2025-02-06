using OniMP.Commands.NetCommands;
using OniMP.Core;
using OniMP.Events;
using OniMP.Events.Others;

namespace OniMP.Commands;

internal class UICommands
{
    public static void UpdatePlayerCursorPositionCommand_Event(UpdatePlayerCursorPositionCommand command)
    {
        var player = MultiplayerManager.Instance.MultiGame.Players[command.PlayerId];
        EventManager.TriggerEvent(new PlayerCursorPositionUpdatedEvent(player, command.EventArgs));

    }
}
