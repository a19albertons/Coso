
using System;
using System.Reflection;

public static class ReflectionHelper
{
    public static FieldInfo GetField(Type type, string fieldName)
    {
        return type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
    }

    public static void ResetStaticField(System.Type t, string name)
    {
        var f = t.GetField(name, BindingFlags.Static | BindingFlags.NonPublic);
        if (f != null) f.SetValue(null, null);
    }

    public static MethodInfo GetMethodInfo(Type type, string methodName)
    {
        return type.GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
    }
}