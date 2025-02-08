using MultiplayerMod.Commands.NetCommands.Args;

namespace MultiplayerMod.Commands.NetCommands;

[Serializable]
public class UpdateAlarmCommand(AlarmSideScreenEventArgs args) : BaseCommandEvent
{
    public AlarmSideScreenEventArgs Args => args;
}
