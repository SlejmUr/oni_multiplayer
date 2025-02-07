using MultiplayerMod.Core.Objects;
using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

[Serializable]
public class AcceptDeliveryCommand(AcceptDeliveryEventArgs args) : BaseCommandEvent
{
    public AcceptDeliveryEventArgs Args => args;
}

[Serializable]
public class AcceptDeliveryEventArgs(ComponentResolver<Telepad> target, ITelepadDeliverable deliverable, MultiplayerId gameObjectId, MultiplayerId proxyId)
{
    public ComponentResolver<Telepad> Target => target;
    public ITelepadDeliverable Deliverable => deliverable;
    public MultiplayerId GameObjectId => gameObjectId;
    public MultiplayerId ProxyId => proxyId;
}
