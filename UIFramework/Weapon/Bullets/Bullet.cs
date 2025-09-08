using ProtoRender.Object;
using SFML.System;
using ControlLib.Buttons;
using InteractionFramework.Death;
using InteractionFramework.VisualImpact;
using InteractionFramework.HitAction;
using RayTracingLib.Detection;
using SFML.Graphics;

namespace UIFramework.Weapon.Bullets;
public abstract class Bullet : IBullet
{
    protected IUnit? _owner = null;
    public virtual IUnit? Owner { get => _owner; protected set => _owner = value; }
    protected Vector3f PositionOwner { get; set; } = new();


    public virtual float Damage { get; set; }
    public virtual bool IsActive { get; protected set; } = true;
    public float FlightDistance { get; set; } = IBullet.InfinityFlightDistance;


    public virtual ButtonBinding? HitObject { get; set; } = null;


    public Bullet(float damage, ButtonBinding? hitObject) 
    {
        Damage = damage;

        HitObject = hitObject;
    }
    public Bullet(ButtonBinding? hitObject)
        :this(1, hitObject)
    {}
    public Bullet(Bullet bullet)
        :this(bullet.Damage,  bullet.HitObject)
    {}

    public abstract void Flight(IUnit owner);
    public abstract void Update();
    public virtual void OnHit(IUnit owner, IUnit bullet, IObject? target, Vector3f hitPosition)
    {
        if (target is null)
            return;

        var hitEffect = GetHitEffect(owner, bullet, target);
        if (hitEffect is null)
            return;

        foreach (var effect in hitEffect)
        {
            HandleVisualImpact(effect, hitPosition, owner);
            HandleDrawable(effect, target, bullet);
            HandleSound(effect, bullet, hitPosition);
        }
        HandleDamage(target);
    }

    #region Handle
    private List<HitEffect>? GetHitEffect(IUnit owner, IUnit bullet, IObject target)
    {
        var coordinate = DrawLib.Drawing.GetDrawingCoordinte(target, bullet);
        var texturePath = target.GetUsedTexture(owner)?.PathTexture;
        return HitDataCache.Get(texturePath, coordinate)?.ToList();
    }

    private void HandleVisualImpact(HitEffect hitEffect, Vector3f position, IUnit owner)
    {
        if (hitEffect.VisualImpact != null)
        {
            BeyondRenderManager.Create(owner, hitEffect.VisualImpact.GetCopy(), position.X, position.Y, position.Z);
        }
    }

    private void HandleDrawable(HitEffect hitEffect, IObject target, IUnit bullet)
    {
        if (target is IDrawable drawable && hitEffect.DrawableBatch != null)
        {
            var batch = hitEffect.DrawableBatch.Get();
            if (batch != null)
                DrawLib.Drawing.DrawingObject(target, bullet, batch);
        }
    }

    private void HandleDamage(IObject target)
    {
        if (target is IDamageable damageable)
        {
            damageable.Hp.Decrease(Damage);
            HitObject?.Listen();
        }
    }

    private void HandleSound(HitEffect hitEffect, IUnit bullet, Vector3f position)
    {
        if (hitEffect.SoundHit != null && bullet.Map != null)
        {
            hitEffect.SoundHit.Play(bullet.Map, position);
        }
    }
    #endregion


    public abstract IBullet GetCopy();
}