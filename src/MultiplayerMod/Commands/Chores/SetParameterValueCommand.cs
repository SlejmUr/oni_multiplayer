using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Core.Weak;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.Chores;

[Serializable]
public class SetParameterValueCommand : BaseCommandEvent
{
    public ComponentResolver<StateMachineController> Controller { get; }
    public Type StateMachineInstanceType { get; }
    public int ParameterIndex { get; }
    public object Value { get; }

    public SetParameterValueCommand(StateMachine.Instance smi, StateMachine.Parameter parameter, object value)
    {
        var runtime = StateMachineWeak.Get(smi);
        Controller = runtime.GetController().GetComponentResolver();
        StateMachineInstanceType = smi.GetType();
        ParameterIndex = parameter.idx;
        this.Value = ArgumentUtils.WrapObject(value);
    }
}
