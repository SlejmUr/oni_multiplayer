﻿using System;
using System.Reflection;

namespace MultiplayerMod.Core.Reflection;

public static class ReflectionExtension {

    public static object GetFieldValue(this object obj, string fieldName) {
        return GetFieldValue<object>(obj, fieldName);
    }

    public static T GetFieldValue<T>(this object obj, string fieldName) {
        var type = obj.GetType();
        while (type != typeof(object)) {
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (field != null) {
                return (T) field.GetValue(obj);
            }
            type = type.BaseType;
        }
        throw new Exception($"Field {fieldName} not found in {obj.GetType()}");
    }
}
