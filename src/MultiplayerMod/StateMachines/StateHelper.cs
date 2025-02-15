using MultiplayerMod.Extensions;
using MultiplayerMod.StateMachines.States;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MultiplayerMod.StateMachines;

internal static class StateHelper
{
    public static StateMachine.BaseState GetMonitoredState(StateMachine sm, string StateToMonitorName)
    {
        var stateName = StateToMonitorName;
        object findInObject = sm;
        while (stateName.Contains("."))
        {
            var firstSplit = StateToMonitorName.IndexOf('.');
            findInObject = findInObject.GetType().GetField(stateName.Substring(0, firstSplit))
                .GetValue(findInObject);
            stateName = stateName.Substring(firstSplit + 1);
        }
        return (StateMachine.BaseState) findInObject.GetType().GetField(stateName).GetValue(findInObject);
    }
    public static int GetParameterIndex(StateMachine.Instance smi, string parameterName)
    {
        var sm = smi.GetType().GetProperty(nameof(StateMachineMemberReference.Instance.sm))!.GetValue(smi);
        var parameter = sm.GetType().GetField(parameterName).GetValue(sm);
        return (int) parameter.GetType()
            .GetField(nameof(StateMachineMemberReference.Parameter.idx))
            .GetValue(parameter);
    }

    public static object GetParameterValue(StateMachine.Instance smi, string parameterName)
    {
        var sm = smi.GetType().GetProperty(nameof(StateMachineMemberReference.Instance.sm))!.GetValue(smi);
        var parameter = sm.GetType().GetField(parameterName).GetValue(sm);
        var parameterIndex = (int) parameter.GetType()
            .GetField(nameof(StateMachineMemberReference.Parameter.idx))
            .GetValue(parameter);
        var parameterContext = smi.parameterContexts[parameterIndex];
        return parameterContext.GetType()
            .GetField(nameof(StateMachineMemberReference.Parameter.Context.value))
            .GetValue(parameterContext);
    }

    public static object GetStateTarget(StateMachine.Instance smi)
    {
        return smi.stateMachine.GetFieldValue(nameof(StateMachineMemberReference.stateTarget));
    }

    public static Stack<StateMachine.BaseState> GetGoToStack(StateMachine.Instance smi)
    {
        return smi.GetFieldValue<Stack<StateMachine.BaseState>>(nameof(StateMachineMemberReference.Instance.gotoStack));
    }

    public const string WaitStateName = "WaitHostState";
    public const string ContinuationName = "ContinuationState";

    public static void AllowTransition(Chore chore, string targetState, Dictionary<int, object> args)
    {
        var smi = GetSmi(chore);

        var waitHostState = GetWaitHostState(smi);
        waitHostState.GetType().GetMethod(nameof(IWaitHostState.AllowTransition))!
        .Invoke(
            waitHostState,
            [smi, targetState, args]
        );
    }

    public static void AddAndTransitToWaiStateUponEnter(StateMachine.BaseState state)
    {
        var sm = GetSm(state);
        InjectWaitHostState(sm);
        var callbackType = typeof(StateMachine<,,,>)
            .GetNestedType("State")
            .GetNestedType("Callback")
            .MakeGenericType(sm.GetType().BaseType.GetGenericArguments().Append(typeof(object)));
        var method = typeof(StateHelper).GetMethod(
            nameof(TransitToWaitState),
            BindingFlags.NonPublic | BindingFlags.Static
        )!;
        var callback = Delegate.CreateDelegate(callbackType, method);
        state.enterActions.Add(new StateMachine.Action("Transit to waiting state", callback));
    }

    public static StateMachine.BaseState AddContinuationState(StateMachine.BaseState state)
    {
        var sm = GetSm(state);
        var genericType = typeof(ContinuationState<,,,>).MakeGenericType(
            sm.GetType().BaseType.GetGenericArguments().Append(typeof(object))
        );
        return (StateMachine.BaseState) Activator.CreateInstance(genericType, sm, state);
    }

    public static void InjectWaitHostState(StateMachine sm)
    {
        var genericType = typeof(WaitHostState<,,,>).MakeGenericType(
            sm.GetType().BaseType.GetGenericArguments().Append(typeof(object))
        );
        Activator.CreateInstance(genericType, sm);
    }

    public static StateMachine.BaseState GetWaitHostState(Chore chore)
    {
        return GetWaitHostState(GetSmi(chore));
    }

    public static StateMachine.Instance GetSmi(Chore chore)
    {
        return (StateMachine.Instance) chore.GetType().GetProperty(nameof(Chore<StateMachine.Instance>.smi))
            .GetValue(chore);
    }

    private static StateMachine GetSm(StateMachine.BaseState state)
    {
        return state.GetFieldValue<StateMachine>("sm");
    }

    private static StateMachine.BaseState GetWaitHostState(StateMachine.Instance smi) =>
        smi.stateMachine.GetState("root." + WaitStateName);

    private static void TransitToWaitState(StateMachine.Instance smi)
    {
        smi.GoTo(GetWaitHostState(smi));
    }

    private static readonly Regex stateNameRegex = new(@"nameof\(.*?\..*?\.(.*?)\)"); // Chore.State.(chained.name)

    public static string GetChainedStateName(string value, [CallerArgumentExpression(nameof(value))] string expression = default!) =>
        stateNameRegex.Match(expression).Groups[1].Value;
}
