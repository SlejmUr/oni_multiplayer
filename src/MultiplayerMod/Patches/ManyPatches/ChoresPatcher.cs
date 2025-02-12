using HarmonyLib;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Chores;
using MultiplayerMod.Extensions;
using MultiplayerMod.Multiplayer.Controllers;

namespace MultiplayerMod.Patches.ManyPatches;

[HarmonyPatch(typeof(Chore))]
internal static class ChoresPatcher
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Chore))]
    [HarmonyPatch(MethodType.Constructor)]
    internal static void OnlyClientPredictionAdd(Chore __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        if (MultiplayerManager.Instance.MultiGame.Mode != Core.Player.PlayerRole.Client)
            return;
        Debug.Log(__instance.GetType().FullName);
        __instance.AddPrecondition(ChoresController.IsDriverBusy);
        __instance.AddPrecondition(ChoresController.IsMultiplayerChore);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Chore))]
    [HarmonyPatch(MethodType.Constructor)]
    internal static void Chore_CCTOR_Patch(Chore __instance, object[] __args)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        if (MultiplayerManager.Instance.MultiGame.Mode != Core.Player.PlayerRole.Client)
            return;
        Debug.Log(__instance.GetType().FullName);
        switch (MultiplayerManager.Instance.MultiGame.Mode)
        {
            case Core.Player.PlayerRole.Server:
                OnChoreCreated(__instance, __args);
                break;
            case Core.Player.PlayerRole.Client:
                CancelChore(__instance);
                break;
        }
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(Chore), nameof(Chore.Cleanup))]
    private static void ChoreCleanup(Chore __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        MultiplayerManager.Instance.MultiGame.Objects.RemoveObject(__instance);
        EventManager.TriggerEvent(new ChoreCleanupEvent(__instance));
    }

    private static void OnChoreCreated(Chore chore, object[] arguments)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        var serializable = chore.GetSMI().stateMachine.serializable;
        var id = chore.Register(persistent: serializable == StateMachine.SerializeType.Never);
        EventManager.TriggerEvent(new ChoreCreatedEvent(chore, id, chore.GetType(), arguments));
    }
    private static void CancelChore(Chore chore)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        chore.Cancel($"Chore instantiation of type \"{chore.GetType()}\" is disabled");
    }
}
