using OniMP.Core.Player;
using OniMP.Events;
using OniMP.Events.Common;

namespace OniMP.Core.Behaviour;

/// <summary>
/// When <see cref="CorePlayer"/> leaves the object will be destroyed.
/// </summary>
public class DestroyOnPlayerLeave : KMonoBehaviour
{

    [MyCmpReq]
    private readonly PlayerAssigner playerComponent = null!;

    /// <inheritdoc/>
    public override void OnSpawn()
    {
        var player = playerComponent.Player;
        Debug.Log("DestroyOnPlayerLeave.OnSpawn, player: " + player);
        EventManager.SubscribeEvent<PlayerLeftEvent>(OnLeave);
    }

    private void OnLeave(PlayerLeftEvent @event)
    {
        var player = playerComponent.Player;
        if (@event.Player == player)
            DestroyImmediate(gameObject);
    }

    /// <inheritdoc/>
    public override void OnForcedCleanUp() => EventManager.UnsubscribeEvent<PlayerLeftEvent>(OnLeave);
}
