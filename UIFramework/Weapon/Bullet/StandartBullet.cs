using ControlLib;
using EffectLib.VisualEffectLib.Effect;
using HitBoxLib.Data.Observer;
using HitBoxLib.HitBoxSegment;
using HitBoxLib.Operations;
using HitBoxLib.PositionObject;
using HitBoxLib.Segment.SignsTypeSide;
using MapLib;
using MoveLib.Move;
using ObstacleLib;
using ObstacleLib.SpriteLib;
using ProtoRender.Object;
using ProtoRender.RenderAlgorithm;
using RayTracingLib;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TextureLib;
using static UIFramework.Weapon.Patron.IBullet;
using ObjectFramework;
using MapLib.SettingLib;
using System.Data;
using DataPipes;
using HitBoxLib.Data.HitBoxObject;
using System.Runtime;
using static HitBoxLib.Data.HitBoxObject.RenderInfo;
using NGenerics.DataStructures.General;
using UIFramework.Weapon.Bullet;
using RayTracingLib.Detection;


namespace UIFramework.Weapon.Patron;
public class StandartBullet : IBullet
{
    public ControlLib.BottomBinding? HitDrawbleObject { get; set; } = null;
    public ControlLib.BottomBinding? HitObject { get; set; } = null;
    public HitEffect? HitEffect { get; set; } = null;
    private Map Map { get; set; }
    public float Damage { get; set; } = 50;

    public StandartBullet(Map map, ControlLib.BottomBinding? hitDrawbleObject, ControlLib.BottomBinding? hitObject, HitEffect? hitEffect)
    {
        HitDrawbleObject = hitDrawbleObject;
        HitEffect = hitEffect;
        HitObject = hitObject;

        Map = map;
    }
    public StandartBullet(StandartBullet standartBullet)
    {
        HitDrawbleObject = standartBullet.HitDrawbleObject;
        HitEffect = standartBullet.HitEffect;
        HitObject = standartBullet.HitObject;

        Map = standartBullet.Map;
        Damage = standartBullet.Damage;
    }

    public void Flight(ProtoRender.Object.IUnit owner)
    {
        var result = Raycast.RaycastFun(Map, owner);


        if (result.Item1 is null)
            return;


        HitEffect?.Create(owner, result.Item2.X, result.Item2.Y, result.Item2.Z);
        if (result.Item1 is IDrawable drawable)
            HitDrawbleObject?.Listen(drawable, owner);
        else if(result.Item1 is Unit unit)
        {
            unit.DamageAction?.Invoke(Damage);
            HitObject?.Listen();
        }
    }
    public async Task FlightAsync(ProtoRender.Object.IUnit owner)
    {
        await Task.Run(() =>
        {
            var result = Raycast1.RaycastFun(Map, owner);


            if (result.Item1 is null)
                return;


            HitEffect?.Create(owner, result.Item2.X, result.Item2.Y, result.Item2.Z);
            if (result.Item1 is IDrawable drawable)
                HitDrawbleObject?.Listen(drawable, owner);
            else if (result.Item1 is Unit unit)
            {
                unit.DamageAction?.Invoke(Damage);
                HitObject?.Listen();
            }
        });
    }

    public IBullet GetCopy()
    {
        return new StandartBullet(this);
    }
}