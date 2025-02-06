using HarmonyLib;
using OniMP.Core;
using OniMP.Core.Execution;
using OniMP.Events;
using OniMP.Events.Common;

namespace OniMP.Patches;

[HarmonyPatch(typeof(PauseScreen))]
internal static class PauseScreenPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(PauseScreen.TriggerQuitGame))]
    private static void TriggerQuitGame()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        if (!MultiplayerManager.IsMultiplayer())
            return;
        EventManager.TriggerEvent(new GameQuitEvent());
    }
}
