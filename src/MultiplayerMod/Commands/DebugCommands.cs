using OniMP.Commands.NetCommands;

namespace OniMP.Commands;

internal class DebugCommands
{
    private static void DebugGameFrameStepCommand_Event(DebugGameFrameStepCommand command)
    {
        SpeedControlScreen.Instance.DebugStepFrame();
    }

    private static void DebugSimulationStepCommandEvent(DebugSimulationStepCommand command)
    {
        global::Game.Instance.ForceSimStep();
    }
}
