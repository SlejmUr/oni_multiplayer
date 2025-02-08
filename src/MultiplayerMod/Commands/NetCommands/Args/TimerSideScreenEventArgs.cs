using MultiplayerMod.Core.Objects.Resolvers;
namespace MultiplayerMod.Commands.NetCommands.Args;

[Serializable]
public class TimerSideScreenEventArgs(
    ComponentResolver<LogicTimerSensor> target,
    bool displayCyclesMode,
    float onDuration,
    float offDuration,
    float timeElapsedInCurrentState)
{
    public ComponentResolver<LogicTimerSensor> Target => target;
    public bool DisplayCyclesMode => displayCyclesMode;
    public float OnDuration => onDuration;
    public float OffDuration => offDuration;
    public float TimeElapsedInCurrentState => timeElapsedInCurrentState;
}
