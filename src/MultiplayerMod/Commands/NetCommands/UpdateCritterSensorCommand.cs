using MultiplayerMod.Commands.NetCommands.Args;

namespace MultiplayerMod.Commands.NetCommands;

[Serializable]
public class UpdateCritterSensorCommand(CritterSensorSideScreenEventArgs args) : BaseCommandEvent
{
    public CritterSensorSideScreenEventArgs Args => args;
}
