using OniMP.Core.Player;
using Steamworks;

namespace OniMP.Network.Steam;

/// <summary>
/// Creates a Steam Player Profile Provider with userName
/// </summary>
public class SteamPlayerProfileProvider : IPlayerProfileProvider
{
    /// <inheritdoc/>
    public PlayerProfile GetPlayerProfile() => new(SteamFriends.GetPersonaName());
}
