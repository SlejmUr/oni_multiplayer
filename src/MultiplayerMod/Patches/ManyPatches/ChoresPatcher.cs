using EIV_Common.Coroutines;
using HarmonyLib;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Chores;
using MultiplayerMod.Extensions;
using MultiplayerMod.Multiplayer.Controllers;
using System.Reflection.Emit;

namespace MultiplayerMod.Patches.ManyPatches;

[HarmonyPatch]
internal static class ChoresPatcher
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(Chore), MethodType.Constructor)]
    internal static void OnlyClientPredictionAdd(Chore __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        if (MultiplayerManager.Instance.MultiGame.Mode != Core.Player.PlayerRole.Client)
            return;
        __instance.AddPrecondition(ChoresController.IsDriverBusy);
        __instance.AddPrecondition(ChoresController.IsMultiplayerChore);
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(StandardChoreBase), nameof(StandardChoreBase.Cleanup))]
    private static void ChoreCleanup(Chore __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        MultiplayerManager.Instance.MultiGame.Objects.RemoveObject(__instance);
        Debug.Log($"ChoreCleanup: Clear Chore: {__instance}");
        EventManager.TriggerEvent(new ChoreCleanupEvent(__instance));
    }
    

    [HarmonyTranspiler]
    [HarmonyPatch(typeof(ChoreDriver),nameof(ChoreDriver.SetChore))]
    private static IEnumerable<CodeInstruction> ChoreDriverSetChoreTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        var contextField = AccessTools.Field(typeof(ChoreDriver), nameof(ChoreDriver.context));
        var beforeChoreSetMethod = AccessTools.Method(typeof(ChoresPatcher), nameof(BeforeChoreSetCall));

        List<CodeInstruction> newInstructions = instructions.ToList();

        int index = newInstructions.FindIndex(x => x.opcode == OpCodes.Stfld);
        newInstructions.InsertRange(index,
        [
            new CodeInstruction(OpCodes.Ldarg_0), // this
            new CodeInstruction(OpCodes.Ldloc_0), // currentChore
            new CodeInstruction(OpCodes.Ldarga, 1), // ref context
            new CodeInstruction(OpCodes.Call, beforeChoreSetMethod), // Call ChoresPatcher.BeforeChoreSet
            // ChoresPatcher.BeforeChoreSet(this, currentChore, ref context)
        ]);
        for (int z = 0; z < newInstructions.Count; z++)
            yield return newInstructions[z];
    }

    private static void BeforeChoreSetCall(ChoreDriver driver, Chore previousChore, ref Chore.Precondition.Context context)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        if (MultiplayerManager.Instance.MultiGame.Mode != Core.Player.PlayerRole.Server)
            return;

    }

    internal static IEnumerator<double> _ChoreCreateWait(ChoreDriver driver, Chore previousChore, Chore.Precondition.Context context)
    {
        yield return TimeSpan.FromMilliseconds(10).TotalSeconds;
        StateMachine.Instance smi = null;
        yield return CoroutineWorkerCustom.WaitUntilTrue(() =>
        {
            Debug.Log($"Create Wait Chore! {context.chore.GetType()}");
            smi = context.chore.GetSMI_Ext();
            return smi != null;
        });
        Debug.Log($"BeforeChoreSetCall: Driver: {driver} Prev Chore: {previousChore} contect chore: {context.chore}");
        EventManager.TriggerEvent(new BeforeChoreSetEvent(driver, previousChore, ref context));
        yield break;
    }
}
