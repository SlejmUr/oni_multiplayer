namespace MultiplayerMod.StateMachines.States;

/// <summary>
/// Interface for waiting the host to send the new state.
/// </summary>
internal interface IWaitHostState
{
    void AllowTransition(StateMachine.Instance smi, string target, Dictionary<int, object> args);
}
