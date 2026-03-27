using System;
using System.Collections.Generic;

public static class Gr_EventManager
{
    private static readonly Dictionary<Type, Delegate> events = new();

    public static void AddListener<T>(Action<T> listener)
    {
        var type = typeof(T);

        if (events.TryGetValue(type, out var existing))
            events[type] = Delegate.Combine(existing, listener);
        else
            events[type] = listener;
    }

    public static void RemoveListener<T>(Action<T> listener)
    {
        var type = typeof(T);

        if (!events.TryGetValue(type, out var existing))
            return;

        var current = Delegate.Remove(existing, listener);

        if (current == null)
            events.Remove(type);
        else
            events[type] = current;
    }

    public static void Notify<T>(T eventData)
    {
        var type = typeof(T);

        if (events.TryGetValue(type, out var del))
        {
            if (del is Action<T> callback)
                callback.Invoke(eventData);
        }
    }

    public static void Clear()
    {
        events.Clear();
    }
}
