namespace MultiplayerMod.Commands.NetCommands;

[Serializable]
public class SetPersonalPrioritiesAdvancedCommand(bool isAdvanced) : BaseCommandEvent
{
    public bool IsAdvanced => IsAdvanced;
}
