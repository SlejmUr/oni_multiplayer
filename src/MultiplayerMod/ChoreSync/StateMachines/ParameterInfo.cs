namespace MultiplayerMod.ChoreSync.StateMachines;

public class ParameterInfo<T>(string name, bool shared = true, T defaultValue = default)
{
    public string Name => name;
    public Type ValueType => typeof(T);
    public bool Shared => shared;
    public object DefaultValue => defaultValue;
}
