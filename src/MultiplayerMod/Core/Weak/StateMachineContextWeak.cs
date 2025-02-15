using MultiplayerMod.Core.Wrappers;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MultiplayerMod.Core.Weak;

public class StateMachineContextWeak
{
    private static readonly ConditionalWeakTable<StateMachine.Parameter.Context, StateMachineContextWeak> cache = new();

    private readonly Type supportedParameterType = typeof(StateMachine<,,,>.Parameter<>);

    private readonly StateMachine.Parameter.Context context;
    private readonly MethodInfo setMethod;

    private StateMachineContextWeak(StateMachine.Parameter.Context context)
    {
        this.context = context;
        setMethod = context.GetType().GetMethod(
            nameof(StateMachineMemberReference.Parameter.Context.Set),
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
        )!;
    }

    public bool Set(StateMachine.Instance instance, object value)
    {
        var parameterType = context.parameter.GetType();
        var baseType = parameterType.BaseType;
        if (!parameterType.IsGenericType || baseType?.GetGenericTypeDefinition() != supportedParameterType)
        {
            Debug.LogError($"Unable to set context value: unsupported parameter type {parameterType}");
            return false;
        }
        var valueType = baseType.GenericTypeArguments.Last();
        if (value != null && valueType != value.GetType())
        {
            Debug.LogError(
                $"Unable to set context value: invalid value type {value.GetType()} ({valueType.GetType()} expected)"
            );
            return false;
        }
        setMethod.Invoke(context, [value, instance, false]);
        return true;
    }

    public static StateMachineContextWeak Get(StateMachine.Parameter.Context context)
    {
        if (cache.TryGetValue(context, out var tools))
            return tools;

        tools = new StateMachineContextWeak(context);
        cache.Add(context, tools);
        return tools;
    }
}
