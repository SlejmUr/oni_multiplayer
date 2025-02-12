namespace MultiplayerMod.Events.Chores;

public class ChoreCleanupEvent(Chore chore) : BaseEvent
{
    public Chore Chore => chore;
}
