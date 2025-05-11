using ProtoRender.Object;
using RayTracingLib;
using SFML.System;
using ControlLib;
using ObjectFramework.Death;
namespace UIFramework.Weapon.Bullets;
public abstract class Bullet : IBullet
{
    public float Damage { get; set; }
    protected const float baseDamage = 1;

    public BottomBinding? HitDrawbleObject { get; set; } = null;
    public BottomBinding? HitObject { get; set; } = null;
    public HitEffect? HitEffect { get; set; } = null;

    public Bullet(float damage, BottomBinding? hitDrawbleObject, BottomBinding? hitObject, HitEffect? hitEffect) 
    {
        Damage = damage;

        HitDrawbleObject = hitDrawbleObject;
        HitObject = hitObject;
        HitEffect = hitEffect;
    }
    public Bullet(BottomBinding? hitDrawbleObject, BottomBinding? hitObject, HitEffect? hitEffect)
        :this(1, hitDrawbleObject, hitObject, hitEffect)
    {}
    public Bullet(Bullet bullet)
        :this(bullet.Damage, bullet.HitDrawbleObject, bullet.HitObject, bullet.HitEffect)
    {}

    public abstract Task FlightAsync(IUnit owner);
    public abstract void Flight(IUnit owner);

    public virtual void OnHit(IUnit owner, IUnit bullet, IObject? target, Vector3f hitPosition)
    {
        HitEffect?.Create(owner, hitPosition.X, hitPosition.Y, hitPosition.Z);

        if (target is IDrawable drawable)
        {
            HitDrawbleObject?.Listen(drawable, bullet);
        }
        if (target is IDamageable damageable)
        {
            damageable.DamageAction?.Invoke(Damage);
            HitObject?.Listen();
        }
    }

    public abstract IBullet GetCopy();
}
