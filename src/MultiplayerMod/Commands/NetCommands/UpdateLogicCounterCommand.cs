using MultiplayerMod.Commands.NetCommands.Args;

namespace MultiplayerMod.Commands.NetCommands;

[Serializable]
public class UpdateLogicCounterCommand(CounterSideScreenEventArgs args) : BaseCommandEvent
{
    public CounterSideScreenEventArgs Args => args;
}
