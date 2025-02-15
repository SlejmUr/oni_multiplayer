namespace MultiplayerMod.ChoreSync.StateMachines;

public class StateInfo(string name)
{
    public string Name { get; } = name;
    public string ReferenceName { get; } = $"root.{name}";
}
