using System;

public static class ActionExtensions
{
    public static void InvokeSafe(this Action act)
    {
        if (act != null) act.Invoke();
    }

    public static void InvokeSafe<T>(this Action<T> act, T param)
    {
        if (act != null) act.Invoke(param);
    }
}
