﻿using OniMP.Commands.NetCommands;
using System.Reflection;

namespace OniMP.Commands;

internal class CallMethodCommands
{
    internal static void CallMethodCommand_Event(CallMethodCommand command)
    {
        var method = command.declaringType.GetMethod(
            command.methodName,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance |
            BindingFlags.DeclaredOnly
        );
        var obj = command.target.Resolve();
        if (obj != null)
            method?.Invoke(obj, ArgumentUtils.UnWrapObjects(command.args));
    }
}
