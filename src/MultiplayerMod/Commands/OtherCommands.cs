using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Behaviour;
using MultiplayerMod.Extensions;
using MultiplayerMod.Patches;

namespace MultiplayerMod.Commands;

internal class OtherCommands
{
    internal static void SetDisinfectSettingsCommand_Event(SetDisinfectSettingsCommand playerCommand)
    {
        SaveGame.Instance.enableAutoDisinfect = playerCommand.EnableAutoDisinfect;
        SaveGame.Instance.minGermCountForDisinfect = playerCommand.MinGerm;
    }

    internal static void AcceptDeliveryCommand_Event(AcceptDeliveryCommand command)
    {
        TelepadPatch.IsRequestedByCommand = true;
        var telepad = command.Args.Target.Resolve();
        telepad.OnAcceptDelivery(command.Args.Deliverable);
        var capture = TelepadPatch.AcceptedGameObject;

        var minionMultiplayer = capture.GetComponent<MultiplayerInstance>();
        minionMultiplayer.Register(command.Args.GameObjectId);

        var minionIdentity = capture.GetComponent<MinionIdentity>();
        if (minionIdentity != null)
        {
            var proxyMultiplayer = minionIdentity.GetMultiplayerInstance();
            proxyMultiplayer.Register(command.Args.ProxyId);
        }

        ImmigrantScreenPatch.Deliverables = null;
    }

    internal static void RejectDeliveryCommand_Event(RejectDeliveryCommand command)
    {
        TelepadPatch.IsRequestedByCommand = true;
        command.Target.Resolve().RejectAll();
        ImmigrantScreenPatch.Deliverables = null;
    }
}
