namespace MultiplayerMod.ChoreSync;

internal static class ChoreSyncList
{
    public static List<IChoreSync> Chores = [];

    static ChoreSyncList()
    {

    }

    public static void Register(IChoreSync chore)
    {
        Chores.Add(chore);
    }

    public static IChoreSync GetSync(Type ChoreSyncType)
    {
        return Chores.FirstOrDefault(chore => chore.SyncType == ChoreSyncType);
    }
}
