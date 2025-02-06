using OniMP.Core.Player;

namespace OniMP.Commands.NetCommands;

/// <summary>
/// Sync <paramref name="players"/> between clients
/// </summary>
/// <param name="players"></param>
[Serializable]
public class SyncPlayersCommand(CorePlayer[] players) : BaseCommandEvent
{
    /// <summary>
    /// An array of <see cref="CorePlayer"/> to sync.
    /// </summary>
    public CorePlayer[] Players => players;
}
