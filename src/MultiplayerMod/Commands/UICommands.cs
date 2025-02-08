using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Others;
using MultiplayerMod.Patches.ScreenPatches;

namespace MultiplayerMod.Commands;

internal class UICommands
{
    public static void UpdatePlayerCursorPositionCommand_Event(UpdatePlayerCursorPositionCommand command)
    {
        var player = MultiplayerManager.Instance.MultiGame.Players[command.PlayerId];
        EventManager.TriggerEvent(new PlayerCursorPositionUpdatedEvent(player, command.EventArgs));

    }

    public static void InitializeImmigrationCommand_Event(InitializeImmigrationCommand command)
    {
        ImmigrantScreenPatch.Deliverables = command.Deliverables;
    }
}
