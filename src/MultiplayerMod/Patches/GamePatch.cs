using HarmonyLib;
using OniMP.Core.Execution;
using OniMP.Events;
using OniMP.Events.Common;
using UnityEngine;

namespace OniMP.Patches;

[HarmonyPatch]
internal static class GamePatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(global::Game), nameof(global::Game.SetGameStarted))]
    private static void SetGameStarted()
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;
        EventManager.TriggerEvent(new GameStartedNoArgsEvent());
    }

    [HarmonyPostfix]
    [HarmonyPatch(
        typeof(SaveLoadRoot),
        nameof(SaveLoadRoot.Load),
        typeof(GameObject),
        typeof(Vector3),
        typeof(Quaternion),
        typeof(Vector3),
        typeof(IReader)
    )]
    private static void SaveLoad(SaveLoadRoot __result)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;

        if (__result != null)
            EventManager.TriggerEvent(new GameObjectCreated(__result.gameObject));
    }

    [HarmonyPostfix]
    [HarmonyPatch(
        typeof(Util),
        nameof(Util.KInstantiate),
        typeof(GameObject),
        typeof(Vector3),
        typeof(Quaternion),
        typeof(GameObject),
        typeof(string),
        typeof(bool),
        typeof(int)
    )]
    private static void InstantiatePostfix(GameObject __result, bool initialize_id)
    {
        if (!ExecutionManager.LevelIsActive(ExecutionLevel.Multiplayer))
            return;

        if (!initialize_id)
            return;

        var kPrefabId = __result.GetComponent<KPrefabID>();
        if (kPrefabId == null)
            return;

        EventManager.TriggerEvent(new GameObjectCreated(__result.gameObject));
    }
}
