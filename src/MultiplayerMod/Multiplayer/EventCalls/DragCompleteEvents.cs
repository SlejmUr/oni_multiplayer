using MultiplayerMod.Commands;
using MultiplayerMod.Commands.Tools;
using MultiplayerMod.Core;
using MultiplayerMod.Events.Others;
using MultiplayerMod.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace MultiplayerMod.Multiplayer.EventCalls;

internal class DragCompleteEvents
{

    internal static void CopySettings(DragCompletedEvent @event)
    {
        if (@event.Sender is not CopySettingsTool)
            return;

        var lastSelection = GameStatePatch.LastSelectedObject;
        if (lastSelection == null)
            return;

        var cell = Grid.PosToCell(lastSelection.GetComponent<Transform>().GetPosition());

        var component = lastSelection.GetComponent<BuildingComplete>();
        if (component == null)
        {
            Debug.LogWarning($"Component 'BuildingComplete' not found in {lastSelection.GetProperName()} at cell #{cell}");
            return;
        }

        var layer = component.Def.ObjectLayer;

        MultiplayerManager.Instance.NetClient.Send(new CopySettingsCommand(new(@event.Args, cell, layer)));
    }
}
