using HitBoxLib.PositionObject;
using HitBoxLib.Segment.SignsTypeSide;
using ProtoRender.Map;
using ProtoRender.Object;
using RayTracingLib;
using ScreenLib;
using SFML.System;
using System.Collections.Concurrent;


namespace UIFramework.Weapon.Bullets;
public class StandartBullet : Bullet
{
    public StandartBullet(float damage, ControlLib.ButtonBinding? hitObject)
        :base(damage, hitObject)
    {}
    public StandartBullet(ControlLib.ButtonBinding? hitObject)
       : this(baseDamage, hitObject)
    {}
    public StandartBullet(StandartBullet standartBullet)
        : base(standartBullet)
    {}

    public override void Flight(ProtoRender.Object.IUnit owner)
    {
        if (owner.Map is null)
            return;

        var result = Raycast.RaycastFun(owner);


        if (result.Item1 is null)
            return;


        OnHit(owner, owner, result.Item1, result.Item2);
    }
    public override async Task FlightAsync(ProtoRender.Object.IUnit owner)
    {
        if(owner.Map is null) 
            return;   


        await Task.Run(() =>
        {
            var result = Raycast.RaycastFun(owner);
            if (result.Item1 is null)
                return;


            OnHit(owner, owner, result.Item1, result.Item2);
        });
    }

    public override IBullet GetCopy()
    {
        return new StandartBullet(this);
    }
}
