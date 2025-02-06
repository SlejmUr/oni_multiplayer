using OniMP.Commands.NetCommands;
using OniMP.Core;
using OniMP.Core.Player;
using OniMP.Events;
using OniMP.Events.Common;
using OniMP.Network.Common;

namespace OniMP.Multiplayer.EventCalls;

internal class MPClientCalls
{
    public static void Registers()
    {
        MultiplayerManager.Instance.NetClient.StateChanged += OnClientStateChanged;
    }

    internal static void OnClientStateChanged(NetStateClient state)
    {
        if (state == NetStateClient.Connected)
        {
            // sending dlc check if we can play on same game.
            MultiplayerManager.Instance.NetClient.Send(new DLC_CheckCommand(DlcManager.GetActiveDLCIds()), MultiplayerCommandOptions.OnlyHost);
        }
        if (state == NetStateClient.Error)
        {
            EventManager.TriggerEvent(new StopMultiplayerEvent());
            EventManager.TriggerEvent(new ConnectionLostEvent());
        }
    }

    internal static void StartClientWhenReady(GameStartedEvent _)
    {
        if (MultiplayerManager.Instance.MultiGame.Mode != PlayerRole.Client)
            return;

        var currentPlayer = MultiplayerManager.Instance.MultiGame.Players.Current;
        MultiplayerManager.Instance.NetClient.Send(new RequestPlayerStateChangeCommand(currentPlayer.Id, PlayerState.Ready), MultiplayerCommandOptions.OnlyHost);
    }

    internal static void OnGameQuit(GameQuitEvent _)
    {
        MultiplayerManager.Instance.NetClient.Send(new RequestPlayerStateChangeCommand(MultiplayerManager.Instance.MultiGame.Players.Current.Id, PlayerState.Leaving), MultiplayerCommandOptions.OnlyHost);
        EventManager.TriggerEvent(new StopMultiplayerEvent());
    }

    internal static void OnStopMultiplayer(StopMultiplayerEvent _)
    {
        MultiplayerManager.Instance.NetClient.Disconnect();
        MultiplayerManager.Instance.MultiGame.Players.Synchronize([]);
    }
}
