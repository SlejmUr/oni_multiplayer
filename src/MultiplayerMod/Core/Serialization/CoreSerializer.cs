using OniMP.Core.Serialization.Surrogates;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

namespace OniMP.Core.Serialization;

/// <summary>
/// Global accessable Serializer for Binary.
/// </summary>
public static class CoreSerializer
{
    internal static BinaryFormatter Formatter => new()
    {
        SurrogateSelector = SerializationSurrogates.Selector,      
    };

    /// <summary>
    /// Deserializing the <paramref name="data"/> to <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Any type</typeparam>
    /// <param name="data"></param>
    /// <returns>Type <typeparamref name="T"/> or default of type.</returns>
    public static T Deserialize<T>(byte[] data) where T : class
    {
        try
        {
            return (T) Formatter.Deserialize(new MemoryStream(data));
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
            return default;
        }
    }

    /// <summary>
    /// Deserializing the <paramref name="data"/> to <see cref="object"/>
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static object Deserialize(byte[] data)
    {
        return Formatter.Deserialize(new MemoryStream(data));
    }

    /// <summary>
    /// Serialize <paramref name="obj"/> to byte array.
    /// Type must be Serializable!
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns>Empty if type is not Serializable, otherwise serialized array</returns>
    public static byte[] Serialize<T>(T obj)
    {
        Debug.Log($"Serialize {typeof(T)}");
        try
        {
            using MemoryStream memory = new();
            Formatter.Serialize(memory, obj);
            var ret = memory.ToArray();
            Debug.Log($"Data: {BitConverter.ToString(ret).Replace("-", string.Empty)}");
            return ret;
        }
        catch (Exception ex)
        {
            Debug.LogWarning(ex);
            return [];
        }

    }
}
