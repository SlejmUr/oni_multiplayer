using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.Chores;

[Serializable]
public class GoToStateCommand(TypedResolver<StateMachine.Instance> resolver, string statename) : BaseCommandEvent
{
    public GoToStateCommand(TypedResolver<StateMachine.Instance> resolver, StateMachine.BaseState state) :
        this(resolver, state?.name)
    { }

    public TypedResolver<StateMachine.Instance> Resolver => resolver;
    public string StateName => statename;
}
