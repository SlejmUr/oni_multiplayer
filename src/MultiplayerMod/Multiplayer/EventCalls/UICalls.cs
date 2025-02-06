using OniMP.Events.Common;

namespace OniMP.Multiplayer.EventCalls;

internal class UICalls
{
    internal static void OnConnectionLost(ConnectionLostEvent @event)
    {
        var screen = (InfoDialogScreen)GameScreenManager.Instance.StartScreen(
            ScreenPrefabs.Instance.InfoDialogScreen.gameObject,
            GameScreenManager.Instance.ssOverlayCanvas.gameObject
        );
        screen.SetHeader("Multiplayer");
        screen.AddPlainText("Connection has been lost. Further play can not be synced");
        screen.AddOption(
            "OK",
            _ => PauseScreen.Instance.OnQuitConfirm(false)
        );
    }
}
