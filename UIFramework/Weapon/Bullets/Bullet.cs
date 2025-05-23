using ProtoRender.Object;
using RayTracingLib;
using SFML.System;
using ControlLib;
using InteractionFramework.Death;
using SFML.Graphics;
using TextureLib;
using RayTracingLib.Detection;
using InteractionFramework.VisualImpact;
using InteractionFramework.HitAction;
using ObstacleLib.SpriteLib;
using HitBoxLib.HitBoxSegment;
using HitBoxLib.PositionObject;
using HitBoxLib.Segment.SignsTypeSide;
using ObstacleLib.TexturedWallLib;
using ObstacleLib;
using ProtoRender.RenderAlgorithm;
using ProtoRender.RenderInterface;
using ScreenLib;


namespace UIFramework.Weapon.Bullets;
public abstract class Bullet : IBullet
{
    public float Damage { get; set; }
    protected const float baseDamage = 1;

    public BottomBinding? HitObject { get; set; } = null;

    public Bullet(float damage, BottomBinding? hitObject) 
    {
        Damage = damage;

        HitObject = hitObject;
    }
    public Bullet(BottomBinding? hitObject)
        :this(1, hitObject)
    {}
    public Bullet(Bullet bullet)
        :this(bullet.Damage,  bullet.HitObject)
    {}

    public abstract Task FlightAsync(IUnit owner);
    public abstract void Flight(IUnit owner);
    SFML.Graphics.Sprite dddd = new SFML.Graphics.Sprite(ImageLoader.TexturesLoad(ResourceManager.GetPath(Path.Combine("Resources", "Image", "WallTexture", "nek.png"))).First().Texture);
    HitPoint hitPoint = new HitPoint();
    public void DrawingSprite(IObject? obstacle, IUnit unit, SFML.Graphics.Sprite sprite)
    {
        if (obstacle is not null && obstacle is IDrawable drawable)
        {

            //hitPoint = RayDetectionX.DetermineHitObjectSides(obstacle, unit);
            //float radius = circleShape.Radius;

            //float textureX = drawable.CalculateTextureX(hitPoint.UV, hitPoint.TextureWallDetermine);

            //float newRadius = drawable.BringingToStandard(radius);
            //float textureY = RayDetectionY.GetTextureCoordinate(hitPoint, drawable, unit, newRadius);

            //if (drawable.IsInsideTexture(textureX, textureY))
            //    return;

            //float addHeight = newRadius >= radius ? 0f : radius;
            //Vector2f pointPosition = new Vector2f(textureX + addHeight, textureY);

            hitPoint = RayDetectionX.DetermineHitObjectSides(obstacle, unit);

            float textureX = drawable.CalculateTextureX(hitPoint.UV, hitPoint.TextureWallDetermine);

            float height = drawable.BringingToStandard(sprite.Texture.Size.Y);
            float width = drawable.BringingToStandard(sprite.Texture.Size.X);

            float textureY = RayDetectionY.GetTextureCoordinate(hitPoint, drawable, unit, 0);
            if (drawable.IsInsideTexture(textureX, textureY))
                 return;

            float addHeight = height >= sprite.Texture.Size.Y ? 0f : sprite.Texture.Size.Y;
            float scaleY = (height / (sprite.Texture.Size.Y / sprite.Scale.Y));
            float scaleX = (width / (sprite.Texture.Size.X / sprite.Scale.X));

            float x = textureX - (sprite.Texture.Size.X * scaleX) / 2;
            float y = textureY - (sprite.Texture.Size.Y * scaleY) / 2;

            SFML.Graphics.Sprite newSprite = new SFML.Graphics.Sprite(sprite)
            {
                Scale = new Vector2f(scaleX, scaleY),
                Origin = new Vector2f(0, 0),
                Position = new Vector2f(x, y),
            };
            drawable.DrawObjectAsync(newSprite);
        }
    }
    public virtual void OnHit(IUnit owner, IUnit bullet, IObject? target, Vector3f hitPosition)
    {
        if (target is null)
            return;

        var hitEffect = HitDataCache.Get(target.GetUsedTexture(owner)?.PathTexture);


        if (hitEffect?.VisualImpact is not null)
        {
            BeyondRenderManager.Create(owner, hitEffect.VisualImpact.GetCopy(), hitPosition.X, hitPosition.Y, hitPosition.Z);
        }
        if (target is IDrawable drawable && hitEffect?.DrawableBatch is not null)
        {
            var batch = hitEffect.DrawableBatch.Get();
            //if (batch != null)
            //    DrawLib.Drawing.DrawingObject(target, bullet, batch);
            dddd.Scale = new Vector2f(dddd.Scale.X, 0.5f);

            DrawingSprite(target, bullet, dddd);
        }
        if (target is IDamageable damageable)
        {
            damageable.DamageAction?.Invoke(Damage);
            HitObject?.Listen();
        }
    }
    public abstract IBullet GetCopy();
}
