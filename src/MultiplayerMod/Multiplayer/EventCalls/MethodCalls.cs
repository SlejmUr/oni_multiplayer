using OniMP.Commands.NetCommands;
using OniMP.Core;
using OniMP.Events.Others;

namespace OniMP.Multiplayer.EventCalls;

internal class MethodCalls
{
    internal static void ComponentMethodCalled_Event(ComponentMethodCalled called)
    {
        if (MultiplayerManager.Instance.NetClient == null)
            return;
        MultiplayerManager.Instance.NetClient.Send(new CallMethodCommand(called.Args));
    }

    internal static void ComponentMethodCalled_Event(StateMachineMethodCalled called)
    {
        if (MultiplayerManager.Instance.NetClient == null)
            return;
        MultiplayerManager.Instance.NetClient.Send(new CallMethodCommand(called.Args));
    }
}
