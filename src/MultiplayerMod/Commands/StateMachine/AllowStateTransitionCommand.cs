using MultiplayerMod.Commands.Chores;
using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Events;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.StateMachine;

[Serializable]
public class AllowStateTransitionCommand(MultiplayerId choreId, string targetState, Dictionary<int, object> args) : BaseCommandEvent
{
    public MultiplayerId ChoreId => choreId;
    public string TargetState => targetState;
    public Dictionary<int, object> Args => args;

    [NoAutoSubscribe]
    public static AllowStateTransitionCommand EnterTransition(ChoreTransitStateArgs transitData) =>
    new(
        transitData.Chore.MultiplayerId(),
        $"{transitData.TargetState!}_ContinuationState",
        transitData.Args.ToDictionary(a => a.Key, a => ArgumentUtils.WrapObject(a.Value))
    );

    [NoAutoSubscribe]
    public static AllowStateTransitionCommand ExitTransition(ChoreTransitStateArgs transitData) =>
        new(
            transitData.Chore.MultiplayerId(),
            transitData.TargetState,
            transitData.Args.ToDictionary(a => a.Key, a => ArgumentUtils.WrapObject(a.Value))
        );
}
