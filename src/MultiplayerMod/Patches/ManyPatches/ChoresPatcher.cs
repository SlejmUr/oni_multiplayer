using EIV_Common.Coroutines;
using HarmonyLib;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Core.Objects;
using MultiplayerMod.Events;
using MultiplayerMod.Events.Chores;
using MultiplayerMod.Extensions;
using MultiplayerMod.Multiplayer.Controllers;
using System.Reflection.Emit;
using static Mono.Cecil.Mixin;
using static ResearchTypes;
using static STRINGS.DUPLICANTS;

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
        EventManager.TriggerEvent(new ChoreCleanupEvent(__instance));
    }

    [HarmonyPostfix]
    [HarmonyPatch(typeof(StandardChoreBase), MethodType.Constructor,
        [typeof(ChoreType), typeof(IStateMachineTarget), typeof(ChoreProvider), typeof(bool), typeof(Action<Chore>), typeof(Action<Chore>), typeof(Action<Chore>),
        typeof(PriorityScreen.PriorityClass), typeof(int), typeof(bool), typeof(bool), typeof(int), typeof(bool), typeof(ReportManager.ReportType)]
    )]
    internal static void Chore_Ctor_Patch(StandardChoreBase __instance, object[] __args)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        if (!MultiplayerManager.IsMultiplayer())
            return;
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

    private static void OnChoreCreated(StandardChoreBase chore, object[] arguments)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        CoroutineWorkerCustom.StartCoroutine(_ChoreCreateWait(chore, arguments), CoroutineType.Custom, "CreateWait");
    }
    private static void CancelChore(StandardChoreBase chore)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        if (chore == null)
            return;
        CoroutineWorkerCustom.StartCoroutine(_ChoreCancelWait(chore), CoroutineType.Custom, "CancelWait");
    }

    internal static IEnumerator<double> _ChoreCreateWait(StandardChoreBase chore, object[] arguments)
    {
        yield return TimeSpan.FromMilliseconds(10).TotalSeconds;
        StateMachine.Instance smi = null;
        yield return CoroutineWorkerCustom.WaitUntilTrue(() =>
        {
            smi = chore.GetSMI();
            return smi != null;
        });
        yield return 0;
        StateMachine statemachine = smi.stateMachine;
        yield return CoroutineWorkerCustom.WaitUntilTrue(() =>
        {;
            statemachine = smi.stateMachine;
            return statemachine != null;
        });
        var seri = statemachine.serializable;
        var id = chore.Register(persistent: seri == StateMachine.SerializeType.Never);
        Debug.Log("Register Success!");
        EventManager.TriggerEvent(new ChoreCreatedEvent(chore, id, chore.GetType(), arguments));
        yield break;
    }

    internal static IEnumerator<double> _ChoreCancelWait(StandardChoreBase chore)
    {
        yield return TimeSpan.FromMilliseconds(10).TotalSeconds;
        yield return CoroutineWorkerCustom.WaitUntilTrue(() =>
        {
            var smi = chore.GetSMI();
            return smi != null;
        });
        yield return 0;
        Debug.Log("Cancel Chore: " + chore.GetType());
        string reason = $"Chore instantiation of type \"{chore.GetType()}\" is disabled";
        chore.Cancel(reason);
        Debug.Log("Cancel Success!");
        yield break;

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

    private static void BeforeChoreSetCall(ChoreDriver driver,Chore previousChore, ref Chore.Precondition.Context context)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        if (MultiplayerManager.Instance.MultiGame.Mode != Core.Player.PlayerRole.Server)
            return;
        EventManager.TriggerEvent(new BeforeChoreSetEvent(driver, previousChore, ref context));
    }
}
