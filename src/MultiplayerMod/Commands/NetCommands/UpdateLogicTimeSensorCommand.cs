using MultiplayerMod.Commands.NetCommands.Args;

namespace MultiplayerMod.Commands.NetCommands;

[Serializable]
public class UpdateLogicTimeSensorCommand(TimerSideScreenEventArgs args) : BaseCommandEvent
{
    public TimerSideScreenEventArgs Args => args;
}
