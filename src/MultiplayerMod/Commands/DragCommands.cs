using MultiplayerMod.Commands.Tools;
using MultiplayerMod.Commands.Tools.Args;
using MultiplayerMod.Core.Context;
using MultiplayerMod.Events;
using MultiplayerMod.Extensions;
using static Mono.Cecil.Mixin;

namespace MultiplayerMod.Commands;

internal static class DragCommands
{
    internal static void DragToolCommand_Cancel(DragToolCommand<CancelTool> toolCommand)
    {
        var tool = new CancelTool();
        RunBasicCommandForTool(tool, toolCommand.Args, () => { toolCommand.Args.Cells.ForEach(it => tool.OnDragTool(it, 0)); });
    }

    internal static void DragToolCommand_Destruction(DragToolCommand<DeconstructTool> toolCommand)
    {
        var tool = new DeconstructTool();
        RunBasicCommandForTool(tool, toolCommand.Args, () => { toolCommand.Args.Cells.ForEach(it => tool.OnDragTool(it, 0)); });
    }

    internal static void DragToolCommand_Dig(DragToolCommand<DigTool> toolCommand)
    {
        var tool = new DigTool();
        RunBasicCommandForTool(tool, toolCommand.Args, () => { toolCommand.Args.Cells.ForEach(it => tool.OnDragTool(it, 0)); });
    }

    internal static void DragToolCommand_Disinfect(DragToolCommand<DisinfectTool> toolCommand)
    {
        var tool = new DisinfectTool();
        RunBasicCommandForTool(tool, toolCommand.Args, () => { toolCommand.Args.Cells.ForEach(it => tool.OnDragTool(it, 0)); });
    }

    internal static void DragToolCommand_EmptyPipe(DragToolCommand<EmptyPipeTool> toolCommand)
    {
        var tool = new EmptyPipeTool();
        RunBasicCommandForTool(tool, toolCommand.Args, () => { toolCommand.Args.Cells.ForEach(it => tool.OnDragTool(it, 0)); });
    }

    internal static void DragToolCommand_Clear(DragToolCommand<ClearTool> toolCommand)
    {
        var tool = new ClearTool();
        RunBasicCommandForTool(tool, toolCommand.Args, () => { toolCommand.Args.Cells.ForEach(it => tool.OnDragTool(it, 0)); });
    }

    internal static void DragToolCommand_Attack(DragToolCommand<AttackTool> toolCommand)
    {
        var tool = new AttackTool();
        RunBasicCommandForTool(tool, toolCommand.Args, () => { tool.OnDragComplete(toolCommand.Args.CursorDown, toolCommand.Args.CursorUp); });
    }

    internal static void DragToolCommand_Disconnect(DragToolCommand<DisconnectTool> toolCommand)
    {
        var tool = new DisconnectTool();
        RunBasicCommandForTool(tool, toolCommand.Args, () => { tool.OnDragComplete(toolCommand.Args.CursorDown, toolCommand.Args.CursorUp); });
    }

    internal static void DragToolCommand_Capture(DragToolCommand<CaptureTool> toolCommand)
    {
        var tool = new CaptureTool();
        RunBasicCommandForTool(tool, toolCommand.Args, () => { tool.OnDragComplete(toolCommand.Args.CursorDown, toolCommand.Args.CursorUp); });
    }

    internal static void DragToolCommand_Harvest(DragToolCommand<HarvestTool> toolCommand)
    {
        var tool = new HarvestTool();
        tool.downPos = toolCommand.Args.CursorDown;

        tool.options = new Dictionary<string, ToolParameterMenu.ToggleState>
        {
            ["HARVEST_WHEN_READY"] = ToolParameterMenu.ToggleState.Off,
            ["DO_NOT_HARVEST"] = ToolParameterMenu.ToggleState.Off
        };
        toolCommand.Args.Parameters?.ForEach(it => tool.options[it] = ToolParameterMenu.ToggleState.On);
        ContextRunner.Override(new PrioritySettingsContext(toolCommand.Args.Priority), () => { tool.OnDragComplete(toolCommand.Args.CursorDown, toolCommand.Args.CursorUp); });
    }

    internal static void DragToolCommand_Mop(DragToolCommand<MopTool> toolCommand)
    {
        var tool = new MopTool();
        tool.downPos = toolCommand.Args.CursorDown;
        tool.Placer = Assets.GetPrefab(new Tag("MopPlacer"));
        ContextRunner.Override(new PrioritySettingsContext(toolCommand.Args.Priority), () => { tool.OnDragComplete(toolCommand.Args.CursorDown, toolCommand.Args.CursorUp); });
    }

    internal static void DragToolCommand_Prioritize(DragToolCommand<PrioritizeTool> toolCommand)
    {
        var tool = new PrioritizeTool();
        tool.downPos = toolCommand.Args.CursorDown;

        if (tool is not FilteredDragTool filteredTool)
            return;

        filteredTool.currentFilterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>
        {
            [ToolParameterMenu.FILTERLAYERS.ALL] = ToolParameterMenu.ToggleState.Off
        };
        toolCommand.Args.Parameters?.ForEach(it => filteredTool.currentFilterTargets[it] = ToolParameterMenu.ToggleState.On);
        ContextRunner.Override(new ContextArray(new PrioritySettingsContext(toolCommand.Args.Priority), new DisablePriorityConfirmSound()), () => { tool.OnDragComplete(toolCommand.Args.CursorDown, toolCommand.Args.CursorUp); });
    }

    [NoAutoSubscribe]
    internal static void RunBasicCommandForTool<T>(T tool, DragCompleteEventArgs args, System.Action invokeAction) where T : DragTool, new()
    {
        tool.downPos = args.CursorDown;

        if (tool is not FilteredDragTool filteredTool)
            return;

        filteredTool.currentFilterTargets = new Dictionary<string, ToolParameterMenu.ToggleState>
        {
            [ToolParameterMenu.FILTERLAYERS.ALL] = ToolParameterMenu.ToggleState.Off
        };
        args.Parameters?.ForEach(it => filteredTool.currentFilterTargets[it] = ToolParameterMenu.ToggleState.On);
        ContextRunner.Override(new PrioritySettingsContext(args.Priority), invokeAction);
    }
}
