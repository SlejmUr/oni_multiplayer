using MultiplayerMod.Commands.StateMachine;
using MultiplayerMod.Core;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.StateMachines;

namespace MultiplayerMod.Commands;

internal static class StateMachineCommands
{
    internal static void AllowStateTransitionCommand_Event(AllowStateTransitionCommand command)
    {
        var args = command.Args.ToDictionary(a => a.Key, a => ArgumentUtils.UnWrapObject(a.Value));
        var chore = MultiplayerManager.Instance.MPObjects.Get<Chore>(command.ChoreId)!;
        StateHelper.AllowTransition(chore, command.TargetState, args);

    }
}
