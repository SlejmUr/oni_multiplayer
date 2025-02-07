using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace MultiplayerMod.Core.Serialization.Surrogates;

/// <summary>
/// Serialization helper class for <see cref="BinaryFormatter"/>
/// </summary>
public static class SerializationSurrogates
{
    /// <summary>
    /// Selector for adding formatter types into it.
    /// </summary>
    public static readonly SurrogateSelector Selector = new();

    static SerializationSurrogates()
    {
        Selector.Add(new Vector2SerializationSurrogate());
        Selector.Add(new Vector2fSerializationSurrogate());
        Selector.Add(new Vector3SerializationSurrogate());
    }

    /// <summary>
    /// Add <paramref name="surrogate"/> into <paramref name="selector"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="selector"></param>
    /// <param name="surrogate"></param>
    public static void Add<T>(this SurrogateSelector selector, T surrogate)
        where T : ISerializationSurrogate, ISurrogateType
    {
        selector.AddSurrogate(surrogate.Type, new StreamingContext(StreamingContextStates.All), surrogate);
    }

    /// <summary>
    /// Checks if <see cref="Selector"/> has <paramref name="type"/>.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool HasSurrogate(Type type)
    {
        return Selector.GetSurrogate(type, new StreamingContext(StreamingContextStates.All), out var _) != null;
    }
}
