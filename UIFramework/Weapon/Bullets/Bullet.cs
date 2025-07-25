﻿using ProtoRender.Object;
using SFML.System;
using ControlLib.Buttons;
using InteractionFramework.Death;
using InteractionFramework.VisualImpact;
using InteractionFramework.HitAction;

namespace UIFramework.Weapon.Bullets;
public abstract class Bullet : IBullet
{
    public IUnit? Owner { get; protected set; }
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

        var hitEffect = HitDataCache.Get(target.GetUsedTexture(owner)?.PathTexture)?.First();


        if (hitEffect?.VisualImpact is not null)
        {
            BeyondRenderManager.Create(owner, hitEffect.VisualImpact.GetCopy(), hitPosition.X, hitPosition.Y, hitPosition.Z);
        }
        if (target is IDrawable drawable && hitEffect?.DrawableBatch is not null)
        {
            var batch = hitEffect.DrawableBatch.Get();
            if (batch != null)
                DrawLib.Drawing.DrawingObject(target, bullet, batch);
        }
        if (target is IDamageable damageable)
        {
            damageable.Hp.Decrease(Damage);
            HitObject?.Listen();
        }
        if (hitEffect?.SoundHit is not null && bullet.Map is not null)
        {
            hitEffect.SoundHit.Play(bullet.Map, new Vector3f(
                 hitPosition.X,
                 hitPosition.Y,
                 hitPosition.Z
             ));
        }
    }
    public abstract IBullet GetCopy();
}