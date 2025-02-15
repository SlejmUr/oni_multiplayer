using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Commands.Tools.Args;
using MultiplayerMod.Events;

namespace MultiplayerMod.Commands.Tools;

[Serializable]
public class BuiltUtilityCommand<T>(UtilityBuildEventArgs args) : BaseCommandEvent
    where T : BaseUtilityBuildTool, new()
{
    public UtilityBuildEventArgs Args => args;
}

internal class test
{

    [NoAutoSubscribe]
    internal static void BuiltUtilityCommand_UtilityBuildTool_Event<T>(BuiltUtilityCommand<T> command) where T : BaseUtilityBuildTool, new()
    {
        var definition = Assets.GetBuildingDef(command.Args.PrefabId);
        var tool = new UtilityBuildTool
        {
            def = definition,
            conduitMgr = definition.BuildingComplete.GetComponent<IHaveUtilityNetworkMgr>().GetNetworkManager(),
            selectedElements = command.Args.Materials,
            path = command.Args.Path
        };
        //GameContext.Override(new PrioritySettingsContext(Arguments.Priority), () => tool.BuildPath());
    }

}



