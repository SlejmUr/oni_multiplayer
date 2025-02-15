using HarmonyLib;
using MultiplayerMod.ChoreSync;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using System.Reflection;

namespace MultiplayerMod.Patches.ManyPatches;

[HarmonyPatch]
internal static class StateMachinesPatcher
{
    internal static IEnumerable<MethodBase> TargetMethods()
    {
        return ChoreSyncList.GetStateMachineTypes().Select(x => x.GetMethod("InitializeStates"));
    }

    [HarmonyPostfix]
    internal static void PostStuff(object __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        Debug.Log("PostFix: " + __instance.GetType());
        Debug.Log("PostFix: " + __instance.GetType().DeclaringType);
        switch (MultiplayerManager.Instance.MultiGame.Mode)
        {
            case Core.Player.PlayerRole.Server:
                ServerPostWork(__instance as StateMachine);
                break;
            case Core.Player.PlayerRole.Client:
                ClientPostWork(__instance as StateMachine);
                break;
            default:
                break;
        }
    }

    internal static void ServerPostWork(StateMachine __instance)
    {
        Type state_type = __instance.GetType().DeclaringType;
        var state = ChoreSyncList.GetSync(state_type);
        if (state == default)
        {
            Debug.Log($"State for Type {state_type} not yet been implemented.");
            return;
        }
        state.Server(__instance);
    }

    internal static void ClientPostWork(StateMachine __instance)
    {
        Type state_type = __instance.GetType().DeclaringType;
        var state = ChoreSyncList.GetSync(state_type);
        if (state == default)
        {
            Debug.Log($"State for Type {state_type} not yet been implemented.");
            return;
        }
        state.Client(__instance);
    }
}
