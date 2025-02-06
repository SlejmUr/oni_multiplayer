using OniMP.Core.Player;
using OniMP.Events;
using OniMP.Events.Common;
using OniMP.Extensions;
using UnityEngine;

namespace OniMP.Core.Behaviour;

internal class PlayerCursor : KMonoBehaviour
{
    public override void OnSpawn()
    {
        EventManager.SubscribeEvent<PlayerJoinedEvent>(OnPlayerJoined);
        MultiplayerManager.Instance.MultiGame.Players.ForEach(CreatePlayerCursor);
    }

    private void OnPlayerJoined(PlayerJoinedEvent @event) => CreatePlayerCursor(@event.Player);

    public override void OnForcedCleanUp() => EventManager.UnsubscribeEvent<PlayerJoinedEvent>(OnPlayerJoined);

    private void CreatePlayerCursor(CorePlayer player)
    {
        if (player == MultiplayerManager.Instance.MultiGame.Players.Current)
            return;
        Debug.Log("CreatePlayerCursor!");
        var canvas = GameScreenManager.Instance.GetParent(GameScreenManager.UIRenderTarget.ScreenSpaceOverlay);
        Debug.Log("canvas!");
        var cursorName = $"{player.Profile.PlayerName}'s cursor";
        Debug.Log("cursorname!");
        var cursor = new GameObject(cursorName) { transform = { parent = canvas.transform } };
        Debug.Log("cursor!");
        cursor.AddComponent<PlayerAssigner>().Player = player;
        cursor.AddComponent<CursorComponent>();
        cursor.AddComponent<DestroyOnPlayerLeave>();
        Debug.Log("added componennts!");
    }
}
