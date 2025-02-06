using OniMP.Core;
using OniMP.Core.Execution;
using OniMP.Core.Player;
using OniMP.Events.Common;
using OniMP.Events.MainMenu;

namespace OniMP.Multiplayer.EventCalls;

internal class ExecutionCalls
{
    internal static void OnSinglePlayerModeSelected(SinglePlayerModeSelectedEvent _)
    {
        Debug.Log("Execution set to System");
        ExecutionManager.CurrentLevel = ExecutionLevel.System;
    }

    internal static void OnMultiplayerModeSelected(MultiplayerModeSelectedEvent _)
    {
        Debug.Log("Execution set to Multiplayer");
        ExecutionManager.CurrentLevel = ExecutionLevel.Multiplayer;
    }

    internal static void OnPlayerStateChangedEvent(PlayerStateChangedEvent @event)
    {
        if (MultiplayerManager.Instance.MultiGame.Players.Current == @event.Player && @event.Player.State == PlayerState.Ready)
        {
            Debug.Log("Execution set to Game");
            ExecutionManager.CurrentLevel = ExecutionLevel.Game;
        }
            
    }

    internal static void OnStopMultiplayer(StopMultiplayerEvent _)
    {
        Debug.Log("Execution set to System");
        ExecutionManager.CurrentLevel = ExecutionLevel.System;
    }
}
