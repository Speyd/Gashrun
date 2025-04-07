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
using EntityLib.Player;
using EntityLib;

namespace UIFramework.Weapon;
public class HitEffect
{
    public static ConcurrentDictionary<int, HitEffect> ListUpdateEffect = new();
    private static ConcurrentQueue<int> FreeIds = new();

    private readonly TimeSpan Lifetime;
    private readonly DateTime CreationTime;


    private int Id { get; set; }
    public SpriteObstacle ObjectEffect { get; set; }
    private readonly Result jammerResult = new Result();

    public Entity? Owner;

    public static void Render()
    {
        List<HitEffect> effectsSnapshot = ListUpdateEffect.Values.ToList();
        Parallel.ForEach(effectsSnapshot, (effect, token) => 
        {
            effect.Update();

            if(effect.Owner is not null)
                effect.ObjectEffect?.Render(effect.jammerResult, effect.Owner);
        });
    }


    public HitEffect(SpriteObstacle objectEffect, TimeSpan lifetime)
    {
        Lifetime = lifetime;
        CreationTime = DateTime.UtcNow;

        Id = GetNextId();

        ObjectEffect = new SpriteObstacle(objectEffect);
    }

    private static int _nextId = 0;
    private static int GetNextId()
    {
        if (FreeIds.TryDequeue(out int id))
            return id;

        return Interlocked.Increment(ref _nextId) - 1;
    }

    public void Create(Entity entity, double x, double y, double z)
    {
        var newEffect = new HitEffect(ObjectEffect, Lifetime);
        newEffect.Owner = entity;

        //Console.WriteLine($"X: {entity.Direction.X} | Y: {entity.Direction.Y}");
        //Console.WriteLine($"Xx: {x} | Yy: {y}");
       
        newEffect.ObjectEffect.X.Axis = x;
        newEffect.ObjectEffect.Y.Axis = y;
        newEffect.ObjectEffect.Z.Axis = z;

        ListUpdateEffect.AddOrUpdate(newEffect.Id, newEffect, (key, oldValue) => this);
    }
    public void Update()
    {
        if (DateTime.UtcNow - CreationTime >= Lifetime && ObjectEffect is not null)
        {
            ListUpdateEffect.TryRemove(Id, out _);
            FreeIds.Enqueue(Id);
            return;
        }
    }
}