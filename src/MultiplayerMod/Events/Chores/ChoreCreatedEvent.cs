using MultiplayerMod.Core.Objects;

namespace MultiplayerMod.Events.Chores;

public class ChoreCreatedEvent(Chore chore, MultiplayerId id, Type type, object[] arguments) : BaseEvent
{
    public Chore Chore => chore;
    public MultiplayerId Id => id;
    public Type Type = type;
    public object[] Arguments => arguments;
}
