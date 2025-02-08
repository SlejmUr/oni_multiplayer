using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Core;

namespace MultiplayerMod.Patches;

[HarmonyPatch(typeof(JobsTableScreen))]
internal class JobsTableScreenPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(JobsTableScreen.OnAdvancedModeToggleClicked))]
    internal static void OnAdvancedModeToggleClicked()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(new SetPersonalPrioritiesAdvancedCommand(global::Game.Instance.advancedPersonalPriorities));
    }
}
