using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.Tools.Args;

namespace MultiplayerMod.Commands.Tools;

/// <summary>
/// 
/// </summary>
[Serializable]
public class DragToolCommand<T>(DragCompleteEventArgs args) : BaseCommandEvent where T : DragTool, new()
{
    /// <summary>
    /// Argument for <see cref="DragTool"/>
    /// </summary>
    public DragCompleteEventArgs Args => args;
}
