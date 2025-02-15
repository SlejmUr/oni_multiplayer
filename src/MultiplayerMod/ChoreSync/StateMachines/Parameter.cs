using MultiplayerMod.Core.Wrappers;
using System.Reflection;

namespace MultiplayerMod.ChoreSync.StateMachines;

public class Parameter<T>(StateMachine.Instance instance, StateMachine.Parameter parameter)
{
    private readonly MethodBase setter = parameter.GetType().GetMethod(nameof(StateMachineMemberReference.Parameter.Set))!;

    public void Set(T value, bool silenceEvents = false) => setter.Invoke(parameter, [value, instance, silenceEvents]);
}
