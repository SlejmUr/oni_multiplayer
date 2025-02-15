namespace MultiplayerMod.StateMachines.ChoreStates;

internal static class ChoreStatesList
{
    public static List<IChoreState> Chores = [];

    static ChoreStatesList()
    {
        Chores.Add(new MingleChoreStates());
        Chores.Add(new IdleChoreStates());
    }
    public static void Register(IChoreState chore)
    {
        Chores.Add(chore);
    }

    public static IChoreState GetChoreByType(Type t)
    {
        return Chores.FirstOrDefault(x=>x.ChoreType == t);
    }

    public static IChoreState GetChoreByType<T>(T type)
    {
        return Chores.FirstOrDefault(x => x.ChoreType == type.GetType());
    }
}
