using HarmonyLib;
using MultiplayerMod.Core.Execution;
using MultiplayerMod.Events;
using MultiplayerMod.Events.EventArgs;
using MultiplayerMod.Events.Others;
using System.Reflection;

namespace MultiplayerMod.Patches.ManyPatches;

[HarmonyPatch]
internal static class ManyObjectEventPatch
{
    internal static readonly PatchTargetResolver targets = new PatchTargetResolver.Builder()
        .AddMethods(typeof(Filterable), nameof(Filterable.SelectedTag))
        .AddMethods(
            typeof(TreeFilterable),
            nameof(TreeFilterable.AddTagToFilter),
            nameof(TreeFilterable.RemoveTagFromFilter)
        )
        .AddMethods(typeof(Storage), nameof(Storage.SetOnlyFetchMarkedItems))
        .AddMethods(typeof(Door), nameof(Door.QueueStateChange), nameof(Door.OrderUnseal))
        .AddMethods(
            typeof(ComplexFabricator),
            nameof(ComplexFabricator.IncrementRecipeQueueCount),
            nameof(ComplexFabricator.DecrementRecipeQueueCount),
            nameof(ComplexFabricator.SetRecipeQueueCount)
        )
        .AddMethods(typeof(PassengerRocketModule), nameof(PassengerRocketModule.RequestCrewBoard))
        .AddMethods(typeof(RocketControlStation), nameof(RocketControlStation.RestrictWhenGrounded))
        .AddMethods(typeof(ICheckboxControl), nameof(ICheckboxControl.SetCheckboxValue))
        .AddMethods(typeof(SuitLocker), nameof(SuitLocker.ConfigNoSuit), nameof(SuitLocker.ConfigRequestSuit))
        .AddMethods(
            typeof(IThresholdSwitch),
            nameof(IThresholdSwitch.Threshold),
            nameof(IThresholdSwitch.ActivateAboveThreshold)
        )
        .AddMethods(typeof(ISliderControl), nameof(ISingleSliderControl.SetSliderValue))
        .AddMethods(typeof(Valve), nameof(Valve.ChangeFlow))
        .AddMethods(
            typeof(SingleEntityReceptacle),
            nameof(SingleEntityReceptacle.OrderRemoveOccupant),
            nameof(SingleEntityReceptacle.CancelActiveRequest),
            nameof(SingleEntityReceptacle.CreateOrder),
            nameof(SingleEntityReceptacle.SetPreview)
        )
        .AddMethods(typeof(LimitValve), nameof(LimitValve.Limit), nameof(LimitValve.ResetAmount))
        .AddMethods(
            typeof(ILogicRibbonBitSelector),
            nameof(ILogicRibbonBitSelector.SetBitSelection),
            nameof(ILogicRibbonBitSelector.UpdateVisuals)
        )
        .AddMethods(typeof(CreatureLure), nameof(CreatureLure.ChangeBaitSetting))
        .AddMethods(typeof(MonumentPart), nameof(MonumentPart.SetState))
        .AddMethods(typeof(INToggleSideScreenControl), nameof(INToggleSideScreenControl.QueueSelectedOption))
        .AddMethods(typeof(Artable), nameof(Artable.SetUserChosenTargetState), nameof(Artable.SetDefault))
        .AddMethods(typeof(Automatable), nameof(Automatable.SetAutomationOnly))
        .AddMethods(
            typeof(IDispenser),
            nameof(IDispenser.OnCancelDispense),
            nameof(IDispenser.OnOrderDispense),
            nameof(IDispenser.SelectItem)
        )
        .AddMethods(typeof(FlatTagFilterable), nameof(FlatTagFilterable.ToggleTag))
        .AddMethods(typeof(GeneShuffler), nameof(GeneShuffler.RequestRecharge))
        .AddMethods(
            typeof(GeneticAnalysisStation.StatesInstance),
            nameof(GeneticAnalysisStation.StatesInstance.SetSeedForbidden)
        )
        .AddMethods(typeof(IHighEnergyParticleDirection), nameof(IHighEnergyParticleDirection.Direction))
        .AddMethods(
            typeof(CraftModuleInterface),
            nameof(CraftModuleInterface.CancelLaunch),
            nameof(CraftModuleInterface.TriggerLaunch)
        )
        .AddMethods(
            typeof(IActivationRangeTarget),
            nameof(IActivationRangeTarget.ActivateValue),
            nameof(IActivationRangeTarget.DeactivateValue)
        )
        .AddMethods(typeof(ISidescreenButtonControl), nameof(ISidescreenButtonControl.OnSidescreenButtonPressed))
        .AddMethods(typeof(IUserControlledCapacity), nameof(IUserControlledCapacity.UserMaxCapacity))
        .AddMethodAndArgs(typeof(Assignable), [nameof(Assignable.Assign), nameof(Assignable.Unassign)], [1, 0])
        .AddMethods(
            typeof(AccessControl),
            nameof(AccessControl.SetPermission),
            nameof(AccessControl.ClearPermission),
            nameof(AccessControl.DefaultPermission)
        )
        .AddMethods(typeof(LogicBroadcastReceiver), nameof(LogicBroadcastReceiver.SetChannel))
        .AddMethods(typeof(LaunchConditionManager), nameof(LaunchConditionManager.Launch))
        .AddMethods(typeof(GeoTuner.Instance), nameof(GeoTuner.Instance.AssignFutureGeyser))
        .AddMethods(typeof(IConfigurableConsumer), nameof(IConfigurableConsumer.SetSelectedOption))
        .AddMethods(typeof(LogicTimerSensor), nameof(LogicTimerSensor.ResetTimer))
        .AddMethods(
            typeof(IEmptyableCargo),
            nameof(IEmptyableCargo.AutoDeploy),
            nameof(IEmptyableCargo.EmptyCargo),
            nameof(IEmptyableCargo.ChosenDuplicant)
        )
        .AddMethods(
            typeof(IPlayerControlledToggle),
            nameof(IPlayerControlledToggle.ToggleRequested),
            nameof(IPlayerControlledToggle.ToggledByPlayer)
        )
        // TODO decide how to proper patch KMonoBehaviour#Trigger
        // .AddMethods(
        //     typeof(ReorderableBuilding),
        //     nameof(ReorderableBuilding.SwapWithAbove),
        //     nameof(ReorderableBuilding.SwapWithBelow),
        //     nameof(ReorderableBuilding.Trigger)
        // )
        .AddBaseType(typeof(KMonoBehaviour))
        .AddBaseType(typeof(StateMachine.Instance))
        //.CheckArgumentsSerializable(true)
        .Build();

    internal static IEnumerable<MethodBase> TargetMethods()
    {
        return targets.Resolve();
    }

    [HarmonyPrefix]
    internal static void ObjectEventsPrefix() => ExecutionManager.EnterOverrideSection(ExecutionLevel.Component);

    [HarmonyPostfix]
    internal static void ObjectEventsPostfix(object __instance, MethodBase __originalMethod, object[] __args)
    {
        ExecutionManager.LeaveOverrideSection();
        ProcessObjectEvent(__instance, __originalMethod, __args);
    }

    internal static void ProcessObjectEvent(object __instance, MethodBase __originalMethod, object[] __args)
    {
        if (ExecutionManager.LevelIsActive(ExecutionLevel.Game))
            switch (__instance)
            {
                case KMonoBehaviour kMonoBehaviour:
                    EventManager.TriggerEvent<ComponentMethodCalled>(new(new ComponentEventsArgs(kMonoBehaviour, __originalMethod, __args)));
                    return;
                case StateMachine.Instance stateMachine:
                    EventManager.TriggerEvent<StateMachineMethodCalled>(new(new StateMachineEventsArgs(stateMachine, __originalMethod, __args)));
                    return;
                default:
                    throw new NotSupportedException($"{__instance} has un supported type");
            }
    }
}
