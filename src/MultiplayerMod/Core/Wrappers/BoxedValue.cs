namespace MultiplayerMod.Core.Wrappers;

public class BoxedValue<T>(T value)
{
    public T Value { get; set; } = value;
}
