using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.Tools.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiplayerMod.Commands.Tools;

/// <summary>
/// 
/// </summary>
public class DragToolCommand<T>(DragCompleteEventArgs args) : BaseCommandEvent where T : DragTool, new()
{
    /// <summary>
    /// Argument for <see cref="DragTool"/>
    /// </summary>
    public DragCompleteEventArgs Args => args;
}
