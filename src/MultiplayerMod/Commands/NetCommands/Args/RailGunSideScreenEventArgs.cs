using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands.Args;

[Serializable]
public class RailGunSideScreenEventArgs(ComponentResolver<RailGun> target, float launchMass)
{
    public ComponentResolver<RailGun> Target => target;
    public float LaunchMass = launchMass;
}
