using OniMP.Network.Common.Interfaces;
using Steamworks;

namespace OniMP.Network.Steam;

/// <summary>
/// Creating steam Lobby Endpoint
/// </summary>
/// <param name="lobbyID"></param>
public class SteamServerEndpoint(CSteamID lobbyID) : IEndPoint
{
    /// <summary>
    /// Steam Lobby EndPoint
    /// </summary>
    public CSteamID LobbyID => lobbyID;
}