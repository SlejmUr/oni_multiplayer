namespace MultiplayerMod.StateMachines.States;

internal class WaitHostState<StateMachineType, StateMachineInstanceType, MasterType, DefType> : StateMachine<StateMachineType,
    StateMachineInstanceType, MasterType, DefType>.State, IWaitHostState
    where StateMachineInstanceType : StateMachine.Instance
    where MasterType : IStateMachineTarget
{
    public WaitStateParam<bool> TransitionAllowed { get; }
    public WaitStateParam<string> TargetState { get; }
    public WaitStateParam<Dictionary<int, object>> ParametersArgs { get; }
    public WaitHostState(StateMachine sm)
    {
        TransitionAllowed = InitParam(sm, false);
        TargetState = InitParam(sm, (string) null);
        ParametersArgs = InitParam(sm, new Dictionary<int, object>());

        enterActions = [new("Wait for host transition", new Callback(TransitIfAllowed))];

        // we are getting the root State here
        var root = sm.GetType().GetField(nameof(StateMachineMemberReference.root)).GetValue(sm);
        // calling BindState with root_state as root, state as this state and name of this state
        sm.GetType().GetMethod(nameof(StateMachineMemberReference.BindState))!.Invoke(sm, [root, this, "WaitHostState"]);
    }

    public void AllowTransition(StateMachine.Instance smi, string target, Dictionary<int, object> args)
    {
        TransitionAllowed.Set(true, smi);
        TargetState.Set(target, smi);
        ParametersArgs.Set(args, smi);
        TransitIfAllowed(smi);
    }

    private void TransitIfAllowed(StateMachine.Instance smi)
    {
        if (smi.GetCurrentState() != this)
        {
            return;
        }
        if (!TransitionAllowed.Get(smi))
        {
            return;
        }
        foreach (var (parameterIndex, value) in ParametersArgs.Get(smi))
        {
            var parameterContext = smi.parameterContexts[parameterIndex];
            parameterContext.GetType().GetMethod(nameof(StateMachineMemberReference.Parameter.Context.Set))!
            .Invoke(
                parameterContext,
                [value, smi, false]
            );
        }
        smi.GoTo(TargetState.Get(smi));
    }

    private WaitStateParam<ParamType> InitParam<ParamType>(StateMachine sm, ParamType value)
    {
        var param = new WaitStateParam<ParamType>(value)
        {
            name = nameof(WaitStateParam<ParamType>),
            idx = sm.parameters.Length
        };
        sm.parameters = sm.parameters.Append(param);
        return param;
    }

    public class WaitStateParam<ParameterType>(ParameterType defaultValue) : StateMachine.Parameter
    {
        private readonly ParameterType defaultValue = defaultValue;

        public ParameterType Get(StateMachine.Instance smi) =>((WaitStateContext<ParameterType>) smi.GetParameterContext(this)).Value;

        public void Set(ParameterType value, StateMachine.Instance smi) => ((WaitStateContext<ParameterType>) smi.GetParameterContext(this)).Set(value);

        public override Context CreateContext() => new WaitStateContext<ParameterType>(this, defaultValue);
    }

    private class WaitStateContext<ParameterType>(WaitStateParam<ParameterType> parameter, ParameterType defaultValue) : StateMachine.Parameter.Context(parameter)
    {
        public ParameterType Value = defaultValue;

        public void Set(ParameterType value)
        {
            if (EqualityComparer<ParameterType>.Default.Equals(value, Value))
                return;

            Value = value;
        }

        public override void Serialize(BinaryWriter _) { }

        public override void Deserialize(IReader reader, StateMachine.Instance _) { }

        public override void ShowEditor(StateMachine.Instance _) { }

        public override void ShowDevTool(StateMachine.Instance _) { }
    }
}
