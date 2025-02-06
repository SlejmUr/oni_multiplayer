namespace OniMP.Commands.NetCommands;

[Serializable]
internal class SetDisinfectSettingsCommand(int minGerm, bool enableAutoDisinfect) : BaseCommandEvent
{
    public int MinGerm => minGerm;
    public bool EnableAutoDisinfect => enableAutoDisinfect;
}
