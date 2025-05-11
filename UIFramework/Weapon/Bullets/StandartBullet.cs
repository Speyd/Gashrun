using RayTracingLib;
using ProtoRender.Object;
using ObjectFramework.Death;

namespace UIFramework.Weapon.Bullets;
public class StandartBullet : Bullet
{
    public StandartBullet(float damage, ControlLib.BottomBinding? hitDrawbleObject, ControlLib.BottomBinding? hitObject, HitEffect? hitEffect)
        :base(damage, hitDrawbleObject, hitObject, hitEffect)
    {}
    public StandartBullet(ControlLib.BottomBinding? hitDrawbleObject, ControlLib.BottomBinding? hitObject, HitEffect? hitEffect)
       : this(baseDamage, hitDrawbleObject, hitObject, hitEffect)
    {}
    public StandartBullet(StandartBullet standartBullet)
        : base(standartBullet)
    {}

    public override void Flight(ProtoRender.Object.IUnit owner)
    {
        if (owner.Map is null)
            return;

        var result = Raycast.RaycastFun(owner.Map, owner);


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
            var result = Raycast.RaycastFun(owner.Map, owner);
            if (result.Item1 is null)
                return;


            HitEffect?.Create(owner, result.Item2.X, result.Item2.Y, result.Item2.Z);
            if (result.Item1 is IDrawable drawable)
                HitDrawbleObject?.Listen(drawable, owner);
            else if (result.Item1 is IDamageable damageable)
            {
                damageable.DamageAction?.Invoke(Damage);
                HitObject?.Listen();
            }
        });
    }

    public override IBullet GetCopy()
    {
        return new StandartBullet(this);
    }
}