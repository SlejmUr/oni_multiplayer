using HarmonyLib;
using MultiplayerMod.Core.Execution;
using System.Reflection;

namespace MultiplayerMod.Patches;

[HarmonyPatch]
internal static class MethodPatchesDisabler
{
    private static readonly PatchTargetResolver targets = new PatchTargetResolver.Builder()
        .AddMethods(typeof(MinionIdentity), nameof(MinionIdentity.OnSpawn))
        .AddMethods(typeof(MinionStartingStats), nameof(MinionStartingStats.Deliver))
        .AddMethods(typeof(SaveLoader), nameof(SaveLoader.InitialSave))
        .AddMethods(typeof(MinionStorage), nameof(MinionStorage.CopyMinion))
        .Build();

    // ReSharper disable once UnusedMember.Local
    [HarmonyTargetMethods]
    private static IEnumerable<MethodBase> TargetMethods() => targets.Resolve();

    // ReSharper disable once UnusedMember.Local
    [HarmonyPrefix]
    private static void BeforeMethod()
    {
        if (ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            ExecutionManager.EnterOverrideSection(ExecutionLevel.Component);
    }

    // ReSharper disable once UnusedMember.Local
    [HarmonyPostfix]
    private static void AfterMethod()
    {
        if (ExecutionManager.LevelIsActive(ExecutionLevel.Component))
            ExecutionManager.LeaveOverrideSection();
    }
}
