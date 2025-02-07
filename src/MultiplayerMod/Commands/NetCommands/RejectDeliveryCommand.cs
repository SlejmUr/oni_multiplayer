using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

public class RejectDeliveryCommand(ComponentResolver<Telepad> target) : BaseCommandEvent
{
    public ComponentResolver<Telepad> Target => target;
}
