using System.Runtime.Serialization;
using UnityEngine;

namespace MultiplayerMod.Core.Serialization.Surrogates;

/// <summary>
/// Unity Vector 2 for Serialization
/// </summary>
public class Vector2NullSerializationSurrogate : ISerializationSurrogate, ISurrogateType
{
    /// <inheritdoc/>
    public Type Type => typeof(Vector2?);

    /// <inheritdoc/>
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        var vector = (Vector2?)obj;
        if (!vector.HasValue)
        {
            info.AddValue("isnull", true);
            return;
        }
        info.AddValue("isnull", false);
        info.AddValue("x", vector.Value.x);
        info.AddValue("y", vector.Value.y);
    }

    /// <inheritdoc/>
    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        if (info.GetBoolean("isnull"))
            return null;
        var vector = (Vector2)obj;
        vector.x = info.GetSingle("x");
        vector.y = info.GetSingle("y");
        return (Vector2?)vector;
    }
}
