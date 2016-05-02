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

    public static void InvokeSafe<T1, T2>(this Action<T1, T2> act, T1 param1, T2 param2)
    {
        if (act != null) act.Invoke(param1, param2);
    }
}
