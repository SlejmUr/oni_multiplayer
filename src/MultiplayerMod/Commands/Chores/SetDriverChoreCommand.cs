using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Core.Wrappers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.Chores;

/// <summary>
/// Setting new <paramref name="driver"/> for <paramref name="chore"/>
/// </summary>
/// <param name="driver"></param>
/// <param name="consumer"></param>
/// <param name="chore"></param>
/// <param name="data"></param>
[Serializable]
public class SetDriverChoreCommand(ChoreDriver driver, ChoreConsumer consumer, Chore chore, object data) : BaseCommandEvent
{
    /// <summary>
    /// Resolver for the <see cref="ChoreDriver"/>
    /// </summary>
    public ComponentResolver<ChoreDriver> DriverReference => driver.GetComponentResolver();

    /// <summary>
    /// Resolver for the <see cref="ChoreConsumer"/>
    /// </summary>
    public ComponentResolver<ChoreConsumer> ConsumerReference => consumer.GetComponentResolver();

    /// <summary>
    /// Resolver for the <see cref="Chore"/>
    /// </summary>
    public ChoreResolver ChoreReference => chore.GetResolver();

    /// <summary>
    /// Wrapped object that send into network
    /// </summary>
    public object Data => ArgumentUtils.WrapObject(data);
}
