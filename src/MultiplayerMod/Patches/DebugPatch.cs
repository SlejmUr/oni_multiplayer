using HarmonyLib;
using OniMP.Core.Execution;
using OniMP.Events;
using OniMP.Events.Others;

namespace OniMP.Patches;

[HarmonyPatch]
internal static class DebugPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(SpeedControlScreen), nameof(SpeedControlScreen.DebugStepFrame))]
    private static void DebugStepFramePrefix()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            EventManager.TriggerEvent<DebugGameFrameStep>(new());
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(global::Game), nameof(global::Game.ForceSimStep))]
    private static void ForceSimStepPrefix()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            EventManager.TriggerEvent<DebugSimulationStep>(new());
    }
}
