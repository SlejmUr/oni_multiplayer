using OniMP.Events.EventArgs;

namespace OniMP.Commands.NetCommands;

/// <summary>
/// Event that updates <paramref name="playerId"/> Mouse position
/// </summary>
/// <param name="playerId"></param>
/// <param name="eventArgs"></param>
public class UpdatePlayerCursorPositionCommand(Guid playerId, MouseMovedEventArgs eventArgs) : BaseCommandEvent
{
    /// <summary>
    /// Who send the update
    /// </summary>
    public Guid PlayerId => playerId;

    /// <summary>
    /// Mouse Moved Event
    /// </summary>
    public MouseMovedEventArgs EventArgs => eventArgs;
}
