using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Events.Others;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class DebugCalls
{
    internal static void DebugGameFrameStep_Event(DebugGameFrameStep _)
    {
        MultiplayerManager.Instance.NetClient.Send(new DebugGameFrameStepCommand());
    }

    internal static void DebugSimulationStep_Event(DebugSimulationStep _)
    {
        MultiplayerManager.Instance.NetClient.Send(new DebugSimulationStepCommand());
    }
}
