namespace MultiplayerMod.Core.Exceptions;

public class StateMachineStateNotFoundException(StateMachine stateMachine, string name) : Exception($"State \"{name}\" not found in \"{stateMachine.name}\"");
