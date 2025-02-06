using OniMP.Events;
using OniMP.Network.Common.Interfaces;

namespace OniMP.Commands.NetCommands;


/// <summary>
/// Base event for handling Commands
/// </summary>
[Serializable]
public abstract class BaseCommandEvent : BaseEvent
{
    /// <summary>
    /// Identification for the Command
    /// </summary>
    public Guid Id { get; } = Guid.NewGuid();

    /// <summary>
    /// 
    /// </summary>
    public INetId ClientId { get; internal set; }

    /// <inheritdoc/>
    public override string ToString() => $"Command [{Id:N}] {GetType().Name}";
}
