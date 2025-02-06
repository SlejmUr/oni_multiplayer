using OniMP.Core.Player;

namespace OniMP.Commands.NetCommands;

/// <summary>
/// Request server to change player state
/// </summary>
/// <param name="playerId"></param>
/// <param name="state"></param>
[Serializable]
public class RequestPlayerStateChangeCommand(Guid playerId, PlayerState state) : BaseCommandEvent
{
    /// <summary>
    /// The requester PlayerId
    /// </summary>
    public Guid PlayerId => playerId;

    /// <summary>
    /// The Requested State
    /// </summary>
    public PlayerState State => state;
}
