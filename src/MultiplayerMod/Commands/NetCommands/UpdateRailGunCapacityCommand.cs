using MultiplayerMod.Commands.NetCommands.Args;

namespace MultiplayerMod.Commands.NetCommands;

[Serializable]
public class UpdateRailGunCapacityCommand(RailGunSideScreenEventArgs args) : BaseCommandEvent
{
    public RailGunSideScreenEventArgs Args => args;
}
