using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.Tools.Args;

namespace MultiplayerMod.Commands.Tools;

/// <summary>
/// Command for <see cref="BaseUtilityBuildTool"/>
/// </summary>
/// <param name="args"></param>
[Serializable]
public class BuildUtilityCommand(UtilityBuildEventArgs args) : BaseCommandEvent
{
    /// <summary>
    /// Argument for <see cref="BaseUtilityBuildTool"/>
    /// </summary>
    public UtilityBuildEventArgs Args => args;
}
