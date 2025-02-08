using MultiplayerMod.Commands.NetCommands.Args;

namespace MultiplayerMod.Commands.NetCommands;

[Serializable]
public class ChangeSchedulesListCommand : BaseCommandEvent
{
    public readonly List<SerializableSchedule> SerializableSchedules;

    public ChangeSchedulesListCommand(List<global::Schedule> schedules)
    {
        SerializableSchedules = schedules.Select(schedule => new SerializableSchedule(schedule)).ToList();
    }
}
