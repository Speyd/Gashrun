using HitBoxLib.PositionObject;
using NGenerics.Extensions;
using SFML.Graphics;
using SFML.System;
using System.Collections.Concurrent;
using System.Numerics;
using TextureLib.DataCache;


namespace InteractionFramework.HitAction;
public static class HitDataCache
{
    private static readonly PathDataCache<(HitEffect Effect, EffectArea? Coordinate)> _cache = new();

    public static IReadOnlyList<HitEffect>? Get(string? path, Vector2f? coordinate = null)
    {
        var obj = _cache.Get(path);
        if (obj is null)
            return new List<HitEffect>();

        if (coordinate is null)
        {
            return obj
                .Where(eff => eff.Coordinate is null)
                .Select(eff => eff.Effect)
                .ToList();
        }

        var effectsInArea = obj
            .Where(eff => eff.Coordinate is not null &&
                          IsPointInside(coordinate.Value, eff.Coordinate.Value.FirstPoint, eff.Coordinate.Value.SeconsPoint))
            .Select(eff => eff.Effect)
            .ToList();

        return effectsInArea.Count > 0
            ? effectsInArea
            : obj
                .Where(eff => eff.Coordinate is null)
                .Select(eff => eff.Effect)
                .ToList();
    }
    public static IReadOnlyList<HitEffect>? Get(string? path, float x, float y)
    {
        return Get(path, new Vector2f(x, y));
    }

    public static bool IsPointInside(Vector2f point, Vector2f p1, Vector2f p2)
    {
        float minX = Math.Min(p1.X, p2.X);
        float maxX = Math.Max(p1.X, p2.X);
        float minY = Math.Min(p1.Y, p2.Y);
        float maxY = Math.Max(p1.Y, p2.Y);

        return point.X >= minX && point.X <= maxX &&
               point.Y >= minY && point.Y <= maxY;
    }

    public static IReadOnlyList<HitEffect> GetOrAdd(string path, Func<string, (HitEffect, EffectArea?)> factory)
    { 
        return _cache.GetOrAdd(path, factory)
            .Select(eff => eff.Effect)
            .ToList();
    }

    public static void Load(string path, HitEffect effect, EffectArea? box = null) => _cache.Load(path, (effect, box));

    public static void Append(string path, HitEffect effect, EffectArea? box = null) => _cache.Append(path, (effect, box));

    public static void Clear() => _cache.Clear();
}

public record struct EffectArea(Vector2f FirstPoint, Vector2f SeconsPoint);
