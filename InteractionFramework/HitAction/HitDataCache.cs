using System.Collections.Concurrent;


namespace InteractionFramework.HitAction;
public static class HitDataCache
{
    private static readonly ConcurrentDictionary<string, HitEffect> _cache = new();
    public static HitEffect? Get(string? path)
    {
        if (path is null || path == string.Empty)
            return null;

        if (_cache.ContainsKey(path))
            return _cache[path];
        else
        {
            string fullPath = Path.GetFullPath(path);

            foreach (var kvp in _cache)
            {
                string cachedDir = Path.GetFullPath(kvp.Key);

                if (fullPath.StartsWith(cachedDir + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                {
                    return kvp.Value;
                }
            }
        }
        return null;
    }
    public static void Load(string path, HitEffect hitEffectData)
    {
        if (!File.Exists(path) && !Directory.Exists(path))
            throw new FileNotFoundException($"Texture file not found: {path}");

        _cache[path] = hitEffectData;
    }
    public static void Clear() => _cache.Clear();
}
