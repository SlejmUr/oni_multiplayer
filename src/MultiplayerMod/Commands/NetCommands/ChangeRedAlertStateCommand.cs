namespace MultiplayerMod.Commands.NetCommands;

public class ChangeRedAlertStateCommand(bool isEnabled) : BaseCommandEvent
{
    public bool IsEnabled => isEnabled;
}
