using OniMP.Commands.NetCommands;
using OniMP.Core;
using OniMP.Events.Common;
using OniMP.Events;
using OniMP.Multiplayer.UI.Overlays;
using OniMP.Network.Common;

namespace OniMP.Commands;

internal class DLC_Commands
{
    internal static void DLC_CheckCommand_Event(DLC_CheckCommand command)
    {
        var game_dlcs = SaveLoader.Instance.GameInfo.dlcIds;
        var client_dlc_not_compatible = !command.ClientDLCs.Any(x=> !game_dlcs.Contains(x));
        MultiplayerManager.Instance.NetServer.Send(command.ClientId, new DLC_ResultCommand(client_dlc_not_compatible));
    }

    internal static void DLC_ResultCommand_Event(DLC_ResultCommand command)
    {
        if (command.IsOk)
        {
            MultiplayerManager.Instance.NetClient.Send(new InitializeClientCommand(MultiplayerManager.Instance.PlayerProfileProvider.GetPlayerProfile()), MultiplayerCommandOptions.OnlyHost);
        }
        else
        {
            EventManager.TriggerEvent(new StopMultiplayerEvent());
            MultiplayerStatusOverlay.Show("Server is not compatible with your DLC list.");
        }
    }
}
