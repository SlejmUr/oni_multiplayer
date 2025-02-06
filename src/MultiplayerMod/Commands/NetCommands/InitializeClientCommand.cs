﻿using OniMP.Core.Player;

namespace OniMP.Commands.NetCommands;

/// <summary>
/// Initialize new client to server
/// </summary>
/// <param name="playerProfile"></param>
[Serializable]
public class InitializeClientCommand(PlayerProfile playerProfile) : BaseCommandEvent
{
    /// <summary>
    /// The client profile
    /// </summary>
    public PlayerProfile Profile => playerProfile;
}
