using MultiplayerMod.Core.Objects.Resolvers;

namespace MultiplayerMod.Commands.NetCommands;

[Serializable]
public class MasterSkillCommand(GameObjectResolver minionIdentityReference, string skillId) : BaseCommandEvent
{
    public GameObjectResolver MinionIdentityReference => minionIdentityReference;
    
    public string SkillId => skillId;
}
