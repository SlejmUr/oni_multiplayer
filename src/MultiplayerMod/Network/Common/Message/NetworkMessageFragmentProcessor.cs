using MultiplayerMod.Core.Exceptions;
using MultiplayerMod.Core.Serialization;
using System.Collections.Concurrent;
using System.Runtime.Serialization.Formatters.Binary;
using static MultiplayerMod.Network.Common.Configuration;

namespace MultiplayerMod.Network.Common.Message;

/// <summary>
/// Processing <see cref="NetworkMessageFragment"/>
/// </summary>
public class NetworkMessageFragmentProcessor
{
    private readonly ConcurrentDictionary<uint, ConcurrentDictionary<int, FragmentsBuffer>> fragments = new();

    /// <summary>
    /// Processing <paramref name="handle"/>
    /// </summary>
    /// <param name="clientId"></param>
    /// <param name="handle"></param>
    /// <returns>null if message is in progressm or <see cref="NetworkMessage"/></returns>
    public NetworkMessage Process(uint clientId, INetworkMessageHandle handle) =>
        CoreSerializer.Deserialize<INetworkMessage>(handle.Message.Take((int) handle.Size).ToArray()) switch
        {
            NetworkMessage message => message,
            NetworkMessageFragmentsHeader header => ProcessFragmentsHeader(clientId, header),
            NetworkMessageFragment fragment => ProcessMessageFragment(clientId, fragment),
            _ => null
        };

    private NetworkMessage ProcessFragmentsHeader(uint clientId, NetworkMessageFragmentsHeader header)
    {
        fragments.TryGetValue(clientId, out var index);
        if (index == null)
        {
            index = new ConcurrentDictionary<int, FragmentsBuffer>();
            fragments[clientId] = index;
        }
        var buffer = new FragmentsBuffer(header.FragmentsCount);
        buffer.Timeout += () =>
        {
            Debug.LogWarning($"Fragments buffer timed out (message id: {header.MessageId})");
            index.TryRemove(header.MessageId, out _);
        };
        index[header.MessageId] = buffer;
        return null;
    }

    private NetworkMessage ProcessMessageFragment(uint clientId, NetworkMessageFragment fragment)
    {
        string ExceptionMessage = $"Message (id: {fragment.MessageId}) fragment received, but no fragments buffer found";

        if (!fragments.TryGetValue(clientId, out var index))
        {
            Debug.LogWarning(ExceptionMessage);
            return null;
        }

        if (!index.TryGetValue(fragment.MessageId, out var buffer))
        {
            Debug.LogWarning(ExceptionMessage);
            return null;
        }

        var message = buffer.Append(fragment);
        if (message != null)
            index.TryRemove(fragment.MessageId, out _);

        return message;
    }

    private class FragmentsBuffer
    {
        private const int watchdogIntervalMs = 5000;

        private int index;
        private readonly int count;
        private readonly byte[] buffer;

        public event System.Action Timeout;

        private readonly System.Timers.Timer watchdog = new(watchdogIntervalMs)
        {
            Enabled = true,
            AutoReset = false
        };

        public FragmentsBuffer(int count)
        {
            this.count = count;
            buffer = new byte[count * MaxFragmentDataSize];
            watchdog.Elapsed += (_, _) => Timeout?.Invoke();
        }

        public NetworkMessage Append(NetworkMessageFragment fragment)
        {
            if (index >= count)
                throw new NetworkPlatformException("Invalid fragmentation: more fragments than expected.");

            watchdog.Interval = watchdogIntervalMs;
            Buffer.BlockCopy(fragment.Data, 0, buffer, index * MaxFragmentDataSize, fragment.Data.Length);
            if (++index != count)
                return null;

            watchdog.Enabled = false;
            using var stream = new MemoryStream(buffer);
            return (NetworkMessage) new BinaryFormatter().Deserialize(stream);
        }
    }
}
