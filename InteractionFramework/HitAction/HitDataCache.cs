using SFML.Graphics;
using System.Collections.Concurrent;
using TextureLib.DataCache;


namespace InteractionFramework.HitAction;
public static class HitDataCache
{
    private static readonly PathDataCache<HitEffect> _cache = new();

    public static HitEffect? Get(string? path) => _cache.Get(path);

    public static HitEffect GetOrAdd(string path, Func<string, HitEffect> factory) =>
        _cache.GetOrAdd(path, factory);

    public static void Load(string path, HitEffect texture) => _cache.Load(path, texture);

    public static void Clear() => _cache.Clear();
}
