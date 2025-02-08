using HarmonyLib;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.NetCommands.Args;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Patches;

[HarmonyPatch(typeof(CounterSideScreen))]
internal static class CounterSideScreenPatch
{

    [HarmonyPostfix]
    [HarmonyPatch(nameof(CounterSideScreen.SetMaxCount))]
    private static void SetMaxCount(CounterSideScreen __instance) => TriggerEvent(__instance);

    [HarmonyPostfix]
    [HarmonyPatch(nameof(CounterSideScreen.ToggleAdvanced))]
    private static void ToggleAdvanced(CounterSideScreen __instance) => TriggerEvent(__instance);

    private static void TriggerEvent(CounterSideScreen instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            return;
        MultiplayerManager.Instance.NetClient.Send(new UpdateLogicCounterCommand(new CounterSideScreenEventArgs(
            instance.targetLogicCounter.GetComponentResolver(),
            instance.targetLogicCounter.currentCount,
            instance.targetLogicCounter.maxCount,
            instance.targetLogicCounter.advancedMode
        )));
    }
}
