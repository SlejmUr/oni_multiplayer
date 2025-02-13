namespace MultiplayerMod.StateMachines;

/// <summary>
/// This is a stub class for referencing
/// <see cref="GameStateMachine{StateMachineType,StateMachineInstanceType,MasterType,DefType}"/>
/// members via nameof.
/// </summary>
public class StateMachineMemberReference : GameStateMachine<StateMachineMemberReference, StateMachineMemberReference.Instance, KMonoBehaviour, object>
{

    public new class Instance : GameInstance
    {
        public Instance(KMonoBehaviour master, object def) : base(master, def) { }
        public Instance(KMonoBehaviour master) : base(master) { }
    }

    public new class Parameter : Parameter<object>
    {
        public override StateMachine.Parameter.Context CreateContext() => throw new NotImplementedException();

        public new class Context : Parameter<object>.Context
        {
            public Context(StateMachine.Parameter parameter, object default_value) : base(parameter, default_value) { }

            public override void Serialize(BinaryWriter writer)
            {
                throw new NotImplementedException();
            }

            public override void Deserialize(IReader reader, StateMachine.Instance smi)
            {
                throw new NotImplementedException();
            }

            public override void ShowEditor(StateMachine.Instance base_smi)
            {
                throw new NotImplementedException();
            }

            public override void ShowDevTool(StateMachine.Instance base_smi)
            {
                throw new NotImplementedException();
            }
        }
    }

}
