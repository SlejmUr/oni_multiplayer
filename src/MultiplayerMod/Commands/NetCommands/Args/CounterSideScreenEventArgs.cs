using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands.Args;

[Serializable]
public class CounterSideScreenEventArgs(
        ComponentResolver<LogicCounter> target,
        int currentCount,
        int maxCount,
        bool advancedMode) : BaseCommandEvent
{
    public ComponentResolver<LogicCounter> Target => target;
    public int CurrentCount => currentCount;
    public int MaxCount => maxCount;
    public bool AdvancedMode => advancedMode;
}
