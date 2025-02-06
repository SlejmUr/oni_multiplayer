using OniMP.Core.Objects;
using OniMP.Core.Player;
using OniMP.Multiplayer.Controllers;
using OniMP.Multiplayer.EventCalls;
using OniMP.Multiplayer.Managers.Server;
using OniMP.Network.Common;
using OniMP.Network.Common.Interfaces;
using OniMP.Network.Steam;

namespace OniMP.Core;

/// <summary>
/// Main point where Multiplayer related classes handled.
/// </summary>
public class MultiplayerManager
{
    /// <summary>
    /// Instance of <see cref="MultiplayerManager"/>
    /// </summary>
    public static MultiplayerManager Instance { get; private set; } = new();

    /// <summary>
    /// Checks if can run multiplayer code.
    /// </summary>
    /// <returns></returns>
    public static bool IsMultiplayer()
    {
        if (Instance == null)
            return false;
        if (Instance.NetClient == null)
            return false;
        if (Instance.NetServer == null)
            return false;
        if (Instance.NetClient.State != NetStateClient.Connected)
            return false;
        return true;
    }


    /// <summary>
    /// Creating Objects, Loading Platform related configuration, classes.
    /// </summary>
    public void Init()
    {
        Instance = this;
        MPObjects = new();
        MultiGame = new(MPObjects);
        if (!DistributionPlatform.Initialized)
            return;
        if (DistributionPlatform.Inst.Platform == "Steam")
            SteamNetwork.Init();
        if (DistributionPlatform.Inst.Platform == "Epic")
            InitEpic();
        InitMultiplayerLogics();
    }

    private void InitEpic()
    {

    }

    private void InitMultiplayerLogics()
    {
        WorldManager = new([]);
        MPCommandController.Registers();
        MPClientCalls.Registers();
        MPServerCalls.Registers();
    }

    /// <summary>
    /// Public instance for <see cref="MultiplayerObjects"/>
    /// </summary>
    public MultiplayerObjects MPObjects { get; internal set; }

    /// <summary>
    /// Public instance for <see cref="MultiplayerGame"/>
    /// </summary>
    public MultiplayerGame MultiGame { get; internal set; }

    /// <summary>
    /// Public instance for <see cref="IMultiplayerOperations"/>
    /// </summary>
    public IMultiplayerOperations MultiplayerOperations { get; internal set; }

    /// <summary>
    /// Public instance for <see cref="INetServer"/>
    /// </summary>
    public INetServer NetServer { get; internal set; }

    /// <summary>
    /// Public instance for <see cref="INetClient"/>
    /// </summary>
    public INetClient NetClient { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    public WorldManager WorldManager { get; internal set; }

    /// <summary>
    /// 
    /// </summary>
    public IPlayerProfileProvider PlayerProfileProvider { get; internal set; }
}
