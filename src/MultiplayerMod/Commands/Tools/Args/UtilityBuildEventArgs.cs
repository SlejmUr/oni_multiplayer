namespace MultiplayerMod.Commands.Tools.Args;

[Serializable]
public class UtilityBuildEventArgs(string prefabId, Tag[] materials, List<BaseUtilityBuildTool.PathNode> path, PrioritySetting priority)
{
    public string PrefabId => prefabId;
    public Tag[] Materials => materials;
    public List<BaseUtilityBuildTool.PathNode> Path => path;
    public PrioritySetting Priority => priority;
}
