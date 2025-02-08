using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

public class SetHatCommand(GameObjectResolver minionIdentityReference, string targetHat) : BaseCommandEvent
{
    public GameObjectResolver MinionIdentityReference => minionIdentityReference;

    /// <summary>
    /// Hat to set it, (can be null!)
    /// </summary>
    public string TargetHat => targetHat;
}
