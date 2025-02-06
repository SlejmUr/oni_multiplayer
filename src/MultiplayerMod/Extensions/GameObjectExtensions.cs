using OniMP.Core.Objects.Resolvers;
using System.Runtime.CompilerServices;
using UnityEngine;
using OniMP.Core.Behaviour;

namespace OniMP.Extensions;

/// <summary>
/// 
/// </summary>
public static class GameObjectExtensions
{
    /// <summary>
    /// Getting <see cref="GameObjectResolver"/> from <paramref name="gameObject"/>
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static GameObjectResolver GetGOResolver(this GameObject gameObject)
    {
        var multiplayerId = gameObject.GetComponent<MultiplayerInstance>().Id;
        if (multiplayerId != null)
            return new MultiplayerIdReference(multiplayerId);

        return new GridReference(gameObject);
    }
}
