using MultiplayerMod.Extensions;

namespace MultiplayerMod.Core.Objects.Resolvers;

/// <summary>
/// Type resolver for the basic tyle <see cref="Chore"/>
/// </summary>
/// <param name="chore"></param>
public class ChoreResolver(Chore chore) : TypedResolver<Chore>
{
    private MultiplayerId id = chore.MultiplayerId();

    /// <inheritdoc/>
    public override Chore Resolve() => MultiplayerManager.Instance.MPObjects.Get<Chore>(id)!;
}
