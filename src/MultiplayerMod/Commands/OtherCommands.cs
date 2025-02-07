using MultiplayerMod.Commands.NetCommands;

namespace MultiplayerMod.Commands;

internal class OtherCommands
{
    private static void SetDisinfectSettingsCommand_Event(SetDisinfectSettingsCommand playerCommand)
    {
        SaveGame.Instance.enableAutoDisinfect = playerCommand.EnableAutoDisinfect;
        SaveGame.Instance.minGermCountForDisinfect = playerCommand.MinGerm;
    }
}
