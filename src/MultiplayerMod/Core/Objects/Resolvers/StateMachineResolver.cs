namespace MultiplayerMod.Core.Objects.Resolvers;

/// <summary>
/// Type resolver for <see cref="StateMachine"/>
/// </summary>
/// <param name="controllerReference"></param>
/// <param name="stateMachineInstanceType"></param>
[Serializable]
public class StateMachineResolver(ComponentResolver<StateMachineController> controllerReference, Type stateMachineInstanceType)
    : TypedResolver<StateMachine.Instance>
{
    private ComponentResolver<StateMachineController> ControllerReference { get; set; } = controllerReference;
    private Type StateMachineInstanceType { get; set; } = stateMachineInstanceType;

    /// <inheritdoc/>
    public override StateMachine.Instance Resolve() => ControllerReference.Resolve().GetSMI(StateMachineInstanceType);

}

/// <summary>
/// Type resolver for <see cref="Chore"/>
/// </summary>
/// <typeparam name="StateMachineInstanceType"></typeparam>
/// <param name="chore"></param>
[Serializable]
public class ChoreStateMachineResolver<StateMachineInstanceType>(Chore<StateMachineInstanceType> chore)
    : TypedResolver<StateMachine.Instance> where StateMachineInstanceType : StateMachine.Instance
{
    private static readonly MultiplayerObjects objects = MultiplayerManager.Instance.MPObjects;
    private readonly MultiplayerId id = objects.Get(chore)!.Id;
    /// <inheritdoc/>
    public override StateMachine.Instance Resolve() => objects.Get<Chore<StateMachineInstanceType>>(id)!.smi;
    /// <inheritdoc/>
    public StateMachine.Instance Get() => Resolve();

}
