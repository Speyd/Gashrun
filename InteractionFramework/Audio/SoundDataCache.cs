using SFML.Audio;
using System.Collections.Concurrent;
using TextureLib.DataCache;

namespace InteractionFramework.Audio;
public class SoundDataCache
{
    private static readonly PathDataCache<SoundBuffer> _cache = new();

    public static SoundBuffer? Get(string? path) => _cache.Get(path);

    public static SoundBuffer GetOrAdd(string path, Func<string, SoundBuffer> factory) =>
        _cache.GetOrAdd(path, factory);

    public static void Load(string path, SoundBuffer texture) => _cache.Load(path, texture);

    public static void Clear() => _cache.Clear();
}
