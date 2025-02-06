using OniMP.Core.Exceptions;
using UnityEngine;

namespace OniMP.Core.Objects.Resolvers;

/// <summary>
/// Abstact class that creates <see cref="GameObject"/>
/// </summary>
[Serializable]
public abstract class GameObjectResolver : TypedResolver<GameObject>
{
    /// <summary>
    /// Resolve this to <see cref="GameObject"/>
    /// </summary>
    /// <returns></returns>
    protected abstract GameObject ResolveGameObject();

    /// <inheritdoc/>
    public override GameObject Resolve() => ResolveGameObject() ?? throw new ObjectNotFoundException(this);
}