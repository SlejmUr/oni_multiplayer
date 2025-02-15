using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core.Objects.Resolvers;
using MultiplayerMod.Extensions;
using UnityEngine;

namespace MultiplayerMod.Commands.Chores;

[Serializable]
public class SynchronizeObjectPositionCommand : BaseCommandEvent
{
    public SynchronizeObjectPositionCommand(GameObject gameObject)
    {
        Resolver = gameObject.GetGOResolver();
        Position = gameObject.transform.GetPosition();
        var facing = gameObject.GetComponent<Facing>();
        if (facing != null)
            FacingLeft = facing.facingLeft;
    }

    public GameObjectResolver Resolver { get; }
    public Vector3 Position { get; }
    public bool? FacingLeft { get; }
}
