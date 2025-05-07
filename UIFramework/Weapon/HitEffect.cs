using AnimationLib;
using ObstacleLib.SpriteLib;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapLib;
using ScreenLib;
using System.Collections.Concurrent;
using ObstacleLib;
using ProtoRender.Object;
using ProtoRender.RenderAlgorithm;
using ObjectFramework;
using System.Reflection;

namespace UIFramework.Weapon;
public class HitEffect
{
    public static ConcurrentDictionary<int, HitEffect> ListUpdateEffect = new();
    private static ConcurrentQueue<int> FreeIds = new();
    private static readonly Result jammerResult = new Result();

    private readonly TimeSpan Lifetime;
    private readonly DateTime CreationTime;

    private int Id { get; set; }
    public SpriteObstacle ObjectEffect { get; set; }
    private IUnit? Owner;


    private static readonly object _lock = new();

    public static void Render()
    {
        List<int> toRemove = new();
        List<HitEffect> effectsSnapshot = new();

        lock (ListUpdateEffect)
        {
            effectsSnapshot = ListUpdateEffect.Values.ToList(); // copy safely
        }

        Parallel.ForEach(effectsSnapshot, effect =>
        {
            effect.UpdateCheckRemove(toRemove); // mark for removal
            if (effect.Owner is not null)
                effect.ObjectEffect?.Render(jammerResult, effect.Owner);
        });

        lock (_lock)
        {
            foreach (var id in toRemove)
                Remove(id);
        }
    }



    public HitEffect(SpriteObstacle objectEffect, TimeSpan lifetime)
    {
        Lifetime = lifetime;
        CreationTime = DateTime.UtcNow;

        Id = GetNextId();

        ObjectEffect = new SpriteObstacle(objectEffect);
    }
    public HitEffect(TimeSpan lifetime)
    {
        Lifetime = lifetime;
        CreationTime = DateTime.UtcNow;

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
            var newEffect = new HitEffect(ObjectEffect, Lifetime);
            newEffect.Owner = owner;

            newEffect.ObjectEffect.X.Axis = x;
            newEffect.ObjectEffect.Y.Axis = y;
            newEffect.ObjectEffect.Z.Axis = z;

            ListUpdateEffect.AddOrUpdate(newEffect.Id, newEffect, (key, oldValue) => this);
        }
    }
    public int Create(IUnit owner, SpriteObstacle spriteObstacle)
    {
        var newEffect = new HitEffect(spriteObstacle, new TimeSpan(-1));
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
            if (Lifetime.Ticks != -1 &&
            DateTime.UtcNow - CreationTime >= Lifetime &&
            ObjectEffect is not null)
            {
                toRemove.Add(Id); // откладываем удаление
            }
        }

    }
}