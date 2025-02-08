using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.NetCommands.Args;
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
        var telepad = command.Target.Resolve();
        telepad.OnAcceptDelivery(command.Deliverable);
        var capture = TelepadPatch.AcceptedGameObject;

        var minionMultiplayer = capture.GetComponent<MultiplayerInstance>();
        minionMultiplayer.Register(command.GameObjectId);

        var minionIdentity = capture.GetComponent<MinionIdentity>();
        if (minionIdentity != null)
        {
            var proxyMultiplayer = minionIdentity.GetMultiplayerInstance();
            proxyMultiplayer.Register(command.ProxyId);
        }

        ImmigrantScreenPatch.Deliverables = null;
    }

    internal static void RejectDeliveryCommand_Event(RejectDeliveryCommand command)
    {
        TelepadPatch.IsRequestedByCommand = true;
        command.Target.Resolve().RejectAll();
        ImmigrantScreenPatch.Deliverables = null;
    }

    internal static void ResearchEntryCommand_Event(ResearchEntryCommand command)
    {
        var screen = ManagementMenu.Instance.researchScreen;
        var entry = screen.entryMap.Values.FirstOrDefault(entry => entry.targetTech.Id == command.TechId);
        if (entry == null)
        {
            Debug.LogWarning($"Tech {command.TechId} is not found.");
            return;
        }
        if (command.IsCancel)
        {
            entry.OnResearchCanceled();
        }
        else
        {
            entry.OnResearchClicked();
        }
    }

    internal static void PermitConsumableByDefaultCommand_Event(PermitConsumableByDefaultCommand command)
    {
        ConsumerManager.instance.DefaultForbiddenTagsList.Clear();
        ConsumerManager.instance.DefaultForbiddenTagsList.AddRange(command.PermittedList);
        var screen = ManagementMenu.Instance.consumablesScreen;
        foreach (var row in screen.rows.Where(row => row.rowType == TableRow.RowType.Default))
        {
            foreach (var widget in row.widgets.Where(entry => entry.Key is ConsumableInfoTableColumn).Select(entry => entry.Value))
            {
                screen.on_load_consumable_info(null, widget);
            }
        }
    }

    internal static void PermitConsumableToMinionCommand_Event(PermitConsumableToMinionCommand command)
    {
        ConsumableConsumerPatch.IsCommandSent = true;
        var consumableConsumer = command.Instance.Resolve().GetComponent<ConsumableConsumer>();
        if (consumableConsumer == null) return;

        consumableConsumer.SetPermitted(command.ConsumableId, command.IsAllowed);
        var screen = ManagementMenu.Instance.consumablesScreen;
        foreach (var row in screen.rows)
        {
            var minion = row.GetIdentity();
            row.widgets.
                Where(entry => entry.Key is ConsumableInfoTableColumn).
                Select(entry => entry.Value).
                ForEach(widget => screen.on_load_consumable_info(minion, widget));
        }
    }

    public static void ChangeSchedulesListCommand_Event(ChangeSchedulesListCommand command)
    {
        var manager = ScheduleManager.Instance;
        var schedules = manager.schedules;

        for (var i = 0; i < Math.Min(command.SerializableSchedules.Count, schedules.Count); i++)
        {
            ScheduleScreenEventsPatch.IsCommandSent = true;
            var schedule = schedules[i];
            var changedSchedule = command.SerializableSchedules[i];
            schedule.name = changedSchedule.Name;
            schedule.alarmActivated = changedSchedule.AlarmActivated;
            schedule.assigned = changedSchedule.Assigned;
            schedule.SetBlocksToGroupDefaults(changedSchedule.Groups); // Triggers "Changed"
        }

        if (Math.Abs(command.SerializableSchedules.Count - schedules.Count) > 1)
            Debug.LogWarning("Schedules update contains more than one schedule addition / removal");

        if (command.SerializableSchedules.Count > schedules.Count)
        {
            ScheduleScreenEventsPatch.IsCommandSent = true;
            // New schedules was added
            var newSchedule = command.SerializableSchedules.Last();
            var schedule = manager.AddSchedule(newSchedule.Groups, newSchedule.Name, newSchedule.AlarmActivated);
            schedule.assigned = newSchedule.Assigned;
            schedule.Changed();
        }
        else if (schedules.Count > command.SerializableSchedules.Count)
        {
            ScheduleScreenEventsPatch.IsCommandSent = true;
            // A schedule was removed
            manager.DeleteSchedule(schedules.Last());
        }
    }
}
