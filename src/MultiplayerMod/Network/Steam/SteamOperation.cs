using OniMP.Network.Common.Interfaces;
using Steamworks;

namespace OniMP.Network.Steam;

/// <summary>
/// Steam related <see cref="IMultiplayerOperations"/>
/// </summary>
public class SteamOperation : IMultiplayerOperations
{
    /// <inheritdoc/>
    public void Join() => SteamFriends.ActivateGameOverlay("friends");
}
