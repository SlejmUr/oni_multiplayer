using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands.Args;

[Serializable]
public class AlarmSideScreenEventArgs(
        ComponentResolver<LogicAlarm> target,
        string notificationName,
        string notificationTooltip,
        bool pauseOnNotify,
        bool zoomOnNotify,
        NotificationType notificationType)
{
    public ComponentResolver<LogicAlarm> Target => target;
    public string NotificationName => notificationName;
    public string NotificationTooltip => notificationTooltip;
    public bool PauseOnNotify => pauseOnNotify;
    public bool ZoomOnNotify => zoomOnNotify;
    public NotificationType NotificationType => notificationType;
}
