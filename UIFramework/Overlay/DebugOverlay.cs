using ProtoRender.Object;


namespace UIFramework.Overlay;
public delegate string OverlayDelegate<in T>(T obj);
public static class DebugOverlay
{
    private static readonly Dictionary<Type, Func<object, string>> Overlays = new();
    private static readonly Dictionary<Type, Type[]> TypeInterfacesCache = new();
    private static readonly Dictionary<Type, Type[]> TypeHierarchyCache = new();
    private static readonly object cacheLock = new();

    static DebugOverlay()
    {
        RegisterOverlay<IUnit>(IUnitOverlay.GetInfo);
        RegisterOverlay<IObserver>(IObserverOverlay.GetInfo);
        RegisterOverlay<IObject>(IObjectOverlay.GetInfo);
    }

    public static void RegisterOverlay<T>(OverlayDelegate<T> overlay)
    {
        var wrapper = (Func<object, string>)(obj => overlay((T)obj));
        lock (cacheLock)
        {
            Overlays[typeof(T)] = wrapper;
        }
    }

    public static string GetInfoOverlay(object obj)
    {
        if (obj is null)
            return string.Empty;

        var type = obj.GetType();

        lock (cacheLock)
        {
            if (Overlays.TryGetValue(type, out var exactMatch))
                return exactMatch(obj);
        }

        var allTypes = GetAllTypes(type);
        string? result = null;

        lock (cacheLock)
        {
            foreach (var t in allTypes)
            {
                if (Overlays.TryGetValue(t, out var overlay))
                {
                    result ??= string.Empty;
                    result += overlay(obj) + Environment.NewLine;
                }
            }
        }

        return result ?? string.Empty;
    }

    private static Type[] GetAllTypes(Type type)
    {
        lock (cacheLock)
        {
            if (TypeHierarchyCache.TryGetValue(type, out var cached))
                return cached;

            var hierarchy = new List<Type>();
            for (Type? current = type; current != null; current = current.BaseType)
            {
                hierarchy.Add(current);
            }

            if (!TypeInterfacesCache.TryGetValue(type, out var interfaces))
            {
                interfaces = type.GetInterfaces();
                TypeInterfacesCache[type] = interfaces;
            }

            var result = new Type[hierarchy.Count + interfaces.Length];
            hierarchy.CopyTo(result, 0);
            interfaces.CopyTo(result, hierarchy.Count);

            TypeHierarchyCache[type] = result;
            return result;
        }
    }
}