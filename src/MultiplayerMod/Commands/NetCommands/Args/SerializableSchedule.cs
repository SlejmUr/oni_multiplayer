using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;

namespace MultiplayerMod.Commands.NetCommands.Args;

[Serializable]
public class SerializableSchedule
{
    public string Name { get; }
    public bool AlarmActivated { get; }
    private List<ComponentResolver<Schedulable>> assigned;
    private List<string> blocks;

    private static readonly Dictionary<string, ScheduleGroup> groups =
        Db.Get().ScheduleGroups.allGroups.ToDictionary(
            a => a.Id,
            // It is a group for 1 hour, so it's important to change defaultSegments value to '1' from the default.
            a => new ScheduleGroup(
                a.Id,
                null,
                1,
                a.Name,
                a.description,
                a.uiColor,
                a.notificationTooltip,
                a.allowedTypes,
                a.alarm
            )
        );

    public SerializableSchedule(global::Schedule schedule)
    {
        Name = schedule.name;
        AlarmActivated = schedule.alarmActivated;
        blocks = schedule.blocks.Select(block => block.GroupId).ToList();
        assigned = schedule.assigned
            .Select(@ref => @ref.obj.gameObject.GetComponent<Schedulable>().GetComponentResolver())
            .ToList();
    }

    public List<ScheduleGroup> Groups => blocks.Select(block => groups[block]).ToList();

    public List<Ref<Schedulable>> Assigned => assigned.Select(reference => new Ref<Schedulable>(reference.Resolve())).ToList();
}
