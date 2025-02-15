using MultiplayerMod.ChoreSync.StateMachines;
using MultiplayerMod.Core.Exceptions;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Extensions;
using System.Runtime.CompilerServices;

namespace MultiplayerMod.Core.Weak;

public class StateMachineWeak
{
    private static readonly ConditionalWeakTable<StateMachine.Instance, StateMachineWeak> cache = new();

    private readonly StateMachine.Instance instance;

    private StateMachineWeak(StateMachine.Instance instance)
    {
        this.instance = instance;
    }

    public Parameter<T> FindParameter<T>(ParameterInfo<T> parameterInfo)
    {
        var parameters = instance.stateMachine.parameters;

        // There are not many parameters, caching is probably unjustified
        for (var i = 0; i < parameters.Length; i++)
        {
            if (parameters[i].name == parameterInfo.Name)
                return new Parameter<T>(instance, parameters[i]);
        }
        return null;
    }

    public void GoToState(string name)
    {
        if (name != null)
        {
            var state = instance.stateMachine.GetState(name);
            if (state == null)
                throw new StateMachineStateNotFoundException(instance.stateMachine, name);
            instance.GoTo(state);
        }
        else
            instance.GoTo((StateMachine.BaseState)null);
    }

    public StateMachineController GetController() => (StateMachineController) instance.GetFieldValue(nameof(StateMachineMemberReference.Instance.controller));

    public static StateMachineWeak Get(StateMachine.Instance instance)
    {
        if (cache.TryGetValue(instance, out var tools))
            return tools;
        tools = new StateMachineWeak(instance);
        cache.Add(instance, tools);
        return tools;
    }
}
