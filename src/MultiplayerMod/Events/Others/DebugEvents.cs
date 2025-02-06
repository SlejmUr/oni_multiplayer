namespace OniMP.Events.Others;

/// <summary>
/// Event that called when a game calling <see cref="SpeedControlScreen.DebugStepFrame"/>
/// </summary>
[Serializable]
public class DebugGameFrameStep : BaseEvent;

/// <summary>
/// Event that called when a game calling <see cref="global::Game.ForceSimStep"/>
/// </summary>
[Serializable]
public class DebugSimulationStep : BaseEvent;