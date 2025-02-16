namespace MultiplayerMod.Core.Context;

internal interface IContext
{
    public void Apply();
    public void Restore();
}
