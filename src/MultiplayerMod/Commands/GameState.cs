using OniMP.Commands.NetCommands;
using OniMP.Core;
using OniMP.Events.Common;
using OniMP.Events;
using OniMP.Network.Common;
using OniMP.Patches;

namespace OniMP.Commands;

internal class GameState
{
    internal static void PauseGame_Event(PauseGameCommand _)
    {
        // fix doesnt initialize the game itself
        if (SpeedControlScreen.Instance == null)
            return;
        if (!SpeedControlScreen.Instance.IsPaused)
            SpeedControlScreen.Instance.Pause();
    }
    internal static void ResumeGame_Event(ResumeGameCommand _)
    {
        if (TryResume(MultiplayerManager.Instance.MultiGame))
            return;
        EventManager.SubscribeEvent<PlayersReadyEvent>(OnPlayersReady);
        if (SpeedControlScreen.Instance == null)
            return;
        if (!SpeedControlScreen.Instance.IsPaused)
            SpeedControlScreen.Instance.Pause();
    }

    internal static void ChangeGameSpeed_Event(ChangeGameSpeed gameSpeed)
    {
        // fix doesnt initialize the game itself
        if (SpeedControlScreen.Instance == null)
            return;
        SpeedControlScreenPatch.IsSpeedSetByCommand = true;
        SpeedControlScreen.Instance.SetSpeed(gameSpeed.Speed);
    }

    [NoAutoSubscribe]
    [UnsubAfterCall]
    internal static void OnPlayersReady(PlayersReadyEvent _)
    {
        Resume();
    }

    internal static bool TryResume(MultiplayerGame multiplayer)
    {
        if (!multiplayer.Players.Ready)
            return false;
        Resume();
        return true;
    }

    internal static void Resume()
    {
        if (SpeedControlScreen.Instance == null)
            return;
        if (SpeedControlScreen.Instance.IsPaused)
            SpeedControlScreen.Instance.Unpause();
    }
}
