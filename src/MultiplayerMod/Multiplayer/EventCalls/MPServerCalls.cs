using OniMP.Core;
using OniMP.Core.Exceptions;
using OniMP.Core.Player;
using OniMP.Events.Common;
using OniMP.Events;
using OniMP.Events.Others;
using OniMP.Network.Common;
using OniMP.Commands.NetCommands;
using OniMP.Network.Common.Interfaces;

namespace OniMP.Multiplayer.EventCalls;

/// <summary>
/// Mulitplayer Server related Event calls
/// </summary>
public class MPServerCalls
{
    internal static readonly Dictionary<INetId, Guid> identities = [];

    /// <summary>
    /// Registering <see cref="INetServer"/> Events
    /// </summary>
    public static void Registers()
    {
        MultiplayerManager.Instance.NetServer.ClientDisconnected += OnClientDisconnected;
        MultiplayerManager.Instance.NetServer.StateChanged += OnServerStateChanged;
    }

    internal static void OnServerStateChanged(NetStateServer state)
    {
        if (state == NetStateServer.Started)
            MultiplayerManager.Instance.NetClient.Connect(MultiplayerManager.Instance.NetServer.Endpoint);
    }

    internal static void OnClientDisconnected(INetId id)
    {
        if (!identities.TryGetValue(id, out var playerId))
            throw new PlayersManagementException($"No associated player found for client {id}");

        var player = MultiplayerManager.Instance.MultiGame.Players[playerId];
        MultiplayerManager.Instance.NetServer.Send(new RemovePlayerCommand(player.Id, "Client Disconnected"));
        identities.Remove(id);
        Debug.Log($"Client {id} disconnected {{ Id = {player.Id} }}");
    }

    internal static void OnClientInitializationRequested(ClientInitializationRequestEvent @event)
    {
        var host = @event.ClientId.Equals(MultiplayerManager.Instance.NetClient.Id);
        var role = host ? PlayerRole.Server : PlayerRole.Client;
        var player = new CorePlayer(role, @event.Profile);
        if (identities.ContainsKey(@event.ClientId))
            identities[@event.ClientId] = player.Id;
        else
            identities.Add(@event.ClientId, player.Id);

        MultiplayerManager.Instance.NetServer.Send(@event.ClientId, new SyncPlayersCommand([.. MultiplayerManager.Instance.MultiGame.Players]));
        MultiplayerManager.Instance.NetServer.Clients.ForEach(it => MultiplayerManager.Instance.NetServer.Send(it, new AddPlayerCommand(player, it.Equals(@event.ClientId))));
        Debug.Log($"Client {@event.ClientId} initialized {{ Role = {role}, Id = {player.Id} }}");
    }

    [UnsubAfterCall]
    internal static void ResumeGameOnReady(PlayersReadyEvent _)
    {
        // we currently skip this one here.
        /*
        if (!@event.IsPaused)
            MultiplayerManager.Instance.NetServer.Send(new ResumeGameCommand());
        */
    }
}
