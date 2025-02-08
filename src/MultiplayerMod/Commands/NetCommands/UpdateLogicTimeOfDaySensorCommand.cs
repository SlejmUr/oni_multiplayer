using MultiplayerMod.Commands.NetCommands.Args;

namespace MultiplayerMod.Commands.NetCommands;

[Serializable]
public class UpdateLogicTimeOfDaySensorCommand(TimeRangeSideScreenEventArgs args) : BaseCommandEvent
{
    public TimeRangeSideScreenEventArgs Args => args;
}
