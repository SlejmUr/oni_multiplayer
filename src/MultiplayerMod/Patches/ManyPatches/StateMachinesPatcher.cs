using HarmonyLib;
using MultiplayerMod.ChoreSync;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Extensions;
using System.Reflection;

namespace MultiplayerMod.Patches.ManyPatches;

[HarmonyPatch]
internal static class StateMachinesPatcher
{
    internal static IEnumerable<MethodBase> TargetMethods()
    {
        var chore_list = typeof(Chore).Assembly.GetTypes().Where(x => typeof(StandardChoreBase).IsAssignableFrom(x) && !x.IsGenericType ).ToList();
        Debug.Log("ChoreList");
        chore_list.ForEach(x=> Debug.Log(x.FullName));
        var where_nested_type_and_state = chore_list.Where(x => x.GetNestedTypes().Any(y=>y.Name.Contains("States"))).Select(x => x.GetNestedTypes()).ToList();
        Debug.Log("States");
        where_nested_type_and_state.ForEach(x =>  x.ForEach(y => Debug.Log(y)));
        var state_list = where_nested_type_and_state.Select(x => x.FirstOrDefault(y => y.Name.Contains("States") && !y.Name.Contains("Instance") )).ToList();
        Debug.Log("States Type");
        state_list.ForEach(x => Debug.Log(x.FullName));
        return state_list.Where(x => x.GetMethod("InitializeStates") != null).Select(x => x.GetMethod("InitializeStates"));
    }

    [HarmonyPostfix]
    internal static void PostStuff(object __instance)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
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
        Debug.Log("PostFix: " + __instance.GetType());
        Debug.Log("PostFix: " + __instance.GetType().DeclaringType);
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
