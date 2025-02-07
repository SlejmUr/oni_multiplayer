using UnityEngine;

namespace MultiplayerMod.Events.EventArgs;

/// <summary>
/// Arguments that is used to update the mouse position
/// </summary>
/// <param name="position"></param>
/// <param name="positionWithinScreen"></param>
/// <param name="screenName"></param>
/// <param name="screenType"></param>
[Serializable]
public class MouseMovedEventArgs(Vector2 position, Vector2 positionWithinScreen, string screenName, Type screenType)
{
    /// <summary>
    /// 
    /// </summary>
    public Vector2 Position => position;

    /// <summary>
    /// 
    /// </summary>
    public Vector2 PositionWithinScreen => positionWithinScreen;

    /// <summary>
    /// 
    /// </summary>
    public string ScreenName => screenName;

    /// <summary>
    /// 
    /// </summary>
    public Type ScreenType => screenType;
}
