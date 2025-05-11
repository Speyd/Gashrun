using ObstacleLib.SpriteLib;
using System.Collections.Concurrent;
using ProtoRender.Object;
using ProtoRender.RenderAlgorithm;
using System.Diagnostics;

namespace UIFramework.Weapon;
public class HitEffect
{
    public static ConcurrentDictionary<int, HitEffect> ListUpdateEffect = new();
    private static ConcurrentQueue<int> FreeIds = new();
    private static readonly Result jammerResult = new Result();

    private readonly long LifetimeMilliseconds;
    private readonly Stopwatch Stopwatch = new();

    private int Id { get; set; }
    public SpriteObstacle ObjectEffect { get; set; }
    private IUnit? Owner;


    private static readonly object _lock = new();

    public static void Render(IUnit observer)
    {
        List<int> toRemove = new();
        List<HitEffect> effectsSnapshot = new();

        lock (ListUpdateEffect)
        {
            effectsSnapshot = ListUpdateEffect.Values.ToList();
        }

        Parallel.ForEach(effectsSnapshot, effect =>
        {
            effect.UpdateCheckRemove(toRemove);
            if (effect.Owner is not null)
                effect.ObjectEffect?.Render(jammerResult, observer);
        });

        lock (_lock)
        {
            foreach (var id in toRemove)
                Remove(id);
        }
    }



    public HitEffect(SpriteObstacle objectEffect, long lifetimeMilliseconds)
    {
        LifetimeMilliseconds = lifetimeMilliseconds;
        Stopwatch.Restart();

        Id = GetNextId();
        ObjectEffect = new SpriteObstacle(objectEffect);
    }
    public HitEffect(long lifetimeMilliseconds)
    {
        LifetimeMilliseconds = lifetimeMilliseconds;
        Stopwatch.Restart();

        Id = GetNextId();
    }


    private static int _nextId = 0;
    private static int GetNextId()
    {
        if (FreeIds.TryDequeue(out int id))
            return id;

        return Interlocked.Increment(ref _nextId) - 1;
    }


    public void Create(IUnit owner, double x, double y, double z)
    {
        lock (_lock)
        {
            var newEffect = new HitEffect(ObjectEffect, LifetimeMilliseconds);
            newEffect.Owner = owner;

            newEffect.ObjectEffect.X.Axis = x;
            newEffect.ObjectEffect.Y.Axis = y;
            newEffect.ObjectEffect.Z.Axis = z;

            ListUpdateEffect.AddOrUpdate(newEffect.Id, newEffect, (key, oldValue) => this);
        }
    }
    public int Create(IUnit owner, SpriteObstacle spriteObstacle)
    {
        var newEffect = new HitEffect(spriteObstacle, -1);
        newEffect.ObjectEffect = spriteObstacle;
        newEffect.Owner = owner;


        ListUpdateEffect.AddOrUpdate(newEffect.Id, newEffect, (key, oldValue) => this);
        return newEffect.Id;
    }
    public static void Remove(int id)
    {
        lock (_lock)
        {
            if (ListUpdateEffect.TryRemove(id, out _))
            {
                FreeIds.Enqueue(id);
            }
        }
    }
    public void UpdateCheckRemove(List<int> toRemove)
    {
        lock (_lock)
        {
            if (LifetimeMilliseconds != -1 &&
                Stopwatch.ElapsedMilliseconds >= LifetimeMilliseconds &&
                ObjectEffect is not null)
            {
                toRemove.Add(Id);
            }
        }

    }
}