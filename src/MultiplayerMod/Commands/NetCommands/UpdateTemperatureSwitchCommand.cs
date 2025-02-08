using MultiplayerMod.Commands.NetCommands.Args;

namespace MultiplayerMod.Commands.NetCommands;

[Serializable]
internal class UpdateTemperatureSwitchCommand(TemperatureSwitchSideScreenEventArgs args) : BaseCommandEvent
{
    public TemperatureSwitchSideScreenEventArgs Args => args;
}
