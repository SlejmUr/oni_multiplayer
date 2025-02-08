using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands.Args;


[Serializable]
public class CritterSensorSideScreenEventArgs(
        ComponentResolver<LogicCritterCountSensor> target,
        bool countCritters,
        bool countEggs)
{
    public ComponentResolver<LogicCritterCountSensor> Target => target;
    public bool CountCritters => countCritters;
    public bool CountEggs => countEggs;
}
