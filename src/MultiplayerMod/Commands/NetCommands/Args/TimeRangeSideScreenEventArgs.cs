using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands.Args;

[Serializable]
public class TimeRangeSideScreenEventArgs(
    ComponentResolver<LogicTimeOfDaySensor> target,
    float startTime,
    float duration)
{
    public ComponentResolver<LogicTimeOfDaySensor> Target => target;
    public float StartTime => startTime;
    public float Duration => duration;
}
