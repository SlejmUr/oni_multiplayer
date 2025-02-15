using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.Chores;

[Serializable]
public class ReleaseChoreDriverCommand(ChoreDriver driver) : BaseCommandEvent
{
    public ComponentResolver<ChoreDriver> DriverReference => driver.GetComponentResolver();
}
