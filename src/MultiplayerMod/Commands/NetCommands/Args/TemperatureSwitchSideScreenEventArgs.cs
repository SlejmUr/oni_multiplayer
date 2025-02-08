using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands.Args;

[Serializable]
public class TemperatureSwitchSideScreenEventArgs(
        ComponentResolver<TemperatureControlledSwitch> target,
        float thresholdTemperature,
        bool activateOnWarmerThan)
{
    public ComponentResolver<TemperatureControlledSwitch> Target => target;
    public float ThresholdTemperature => thresholdTemperature;
    public bool ActivateOnWarmerThan => activateOnWarmerThan;
}
