using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.Chores;

public class SetDriverChoreCommand(ChoreDriver driver, ChoreConsumer consumer, Chore chore, object data) : BaseCommandEvent
{
    public ComponentResolver<ChoreDriver> DriverReference => driver.GetComponentResolver();
    public ComponentResolver<ChoreConsumer> ConsumerReference => consumer.GetComponentResolver();
    public ChoreResolver ChoreReference => chore.GetResolver();
    public object Data => ArgumentUtils.WrapObject(data);
}
