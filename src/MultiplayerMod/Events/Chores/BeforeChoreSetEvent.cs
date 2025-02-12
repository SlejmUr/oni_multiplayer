namespace MultiplayerMod.Events.Chores;

public class BeforeChoreSetEvent : BaseEvent
{
    public ChoreDriver Driver { get; internal set; }
    public Chore PreviousChore { get; internal set; }
    public Chore.Precondition.Context Context { get; internal set; }
    public BeforeChoreSetEvent(ChoreDriver driver, Chore previousChore, ref Chore.Precondition.Context context)
    {
        Driver = driver;
        PreviousChore = previousChore;
        Context = context;
    }
}
