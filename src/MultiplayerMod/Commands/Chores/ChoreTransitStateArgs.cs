namespace MultiplayerMod.Commands.Chores;

[Serializable]
public class ChoreTransitStateArgs(Chore chore, string targetState, Dictionary<int, object> args)
{
    public Chore Chore => chore;
    public string TargetState => targetState;
    public Dictionary<int, object> Args => args;
}
