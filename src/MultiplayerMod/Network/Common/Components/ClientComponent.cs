using OniMP.Network.Common.Interfaces;
using UnityEngine;

namespace OniMP.Network.Common.Components;

/// <summary>
/// Component for Client
/// </summary>
public class ClientComponent : MonoBehaviour
{
    /// <summary>
    /// The Client
    /// </summary>
    public INetClient client;

    internal void Update() => client?.Tick();
}
