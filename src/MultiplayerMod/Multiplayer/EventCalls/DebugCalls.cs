using MultiplayerMod.Commands.NetCommands;
using MultiplayerMod.Core;
using MultiplayerMod.Events.Others;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class DebugCalls
{
    private static void DebugGameFrameStep_Event(DebugGameFrameStep step)
    {
        MultiplayerManager.Instance.NetClient.Send(new DebugGameFrameStepCommand());
    }

    private static void DebugSimulationStep_Event(DebugSimulationStep step)
    {
        MultiplayerManager.Instance.NetClient.Send(new DebugSimulationStepCommand());
    }
}
