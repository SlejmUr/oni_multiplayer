using EIV_Common.Coroutines;
using HarmonyLib;
using KMod;
using OniMP.Core;
using OniMP.Events;
using System.Reflection;

namespace OniMP;

/// <summary>
/// Mod Entry Point
/// </summary>
public class ModLoad : UserMod2
{
    /// <summary>
    /// Loading Events, Commands, Initializing
    /// </summary>
    /// <param name="harmony">The Harmony class</param>
    public override void OnLoad(Harmony harmony)
    {
        base.OnLoad(harmony);
        var version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
        Debug.Log("OniMP Version: " + version);
        Debug.Log("OniMP GetPatchedMethods: " + harmony.GetPatchedMethods().Count());
        CoroutineWorkerCustom.Instance.Start();
        EventManager.LoadMain(assembly);
        MultiplayerManager.Instance.Init();
    }
}
