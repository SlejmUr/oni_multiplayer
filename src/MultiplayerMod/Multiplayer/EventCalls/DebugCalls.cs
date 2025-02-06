using OniMP.Commands.NetCommands;
using OniMP.Core;
using OniMP.Events.Others;

namespace OniMP.Multiplayer.EventCalls;

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
