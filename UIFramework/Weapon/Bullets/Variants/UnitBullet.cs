using System.Collections.Concurrent;
using ControlLib.Buttons;
using ProtoRender.Object;
using SFML.System;
using MoveLib.Move;
using RayTracingLib;
using ObstacleLib.SpriteLib;
using InteractionFramework.Audio.SoundType;
using SFML.Audio;
using HitBoxLib.HitBoxSegment;
using MoveLib.Move.Result;
using ScreenLib;
using CollisionResult = MoveLib.Move.Result.CollisionResult;
using HitBoxLib.PositionObject;
using HitBoxLib.Segment.SignsTypeSide;
using TextureLib.Textures.Pair;


namespace UIFramework.Weapon.Bullets.Variants;
public class UnitBullet : Bullet
{
    public override IUnit? Owner
    {
        get => _owner;
        protected set
        {
            if (value != _owner && _owner is not null)
                ignoreCollisionList.TryRemove(_owner, out _);

            _owner = value;
        }
    }
    public Unit Unit { get; set; }
    public ISound? SoundFly { get; set; }
    private Sound? tempFlySound = null;

    private float Speed { get; set; }
    private const float baseSpeed = 10;

    private ConcurrentDictionary<IObject, byte> ignoreCollisionList = new();


    public UnitBullet(float damage, float speed, Unit unit, ButtonBinding? hitObject)
        : base(damage, hitObject)
    {
        Unit = new Unit(unit);
        Speed = speed;
    }
    public UnitBullet(float damage, Unit unit, ButtonBinding? hitObject)
       : this(damage, baseSpeed, unit, hitObject)
    { }
    public UnitBullet(Unit unit, ButtonBinding? hitObject)
        : this(IBullet.BaseDamage, baseSpeed, unit, hitObject)
    { }
    public UnitBullet(UnitBullet bullet)
        : base(bullet)
    {
        Unit = new Unit(bullet.Unit);

        Speed = bullet.Speed;
        Damage = bullet.Damage;
        ignoreCollisionList = bullet.ignoreCollisionList;

    }

    private void MoveAngle(Unit newUnit, Vector3f? resultObject)
    {
        if (resultObject is null)
            return;

        var unitCoordinate = new Vector2f((float)newUnit.X.Axis, (float)newUnit.Y.Axis);
        var targetCoordinate = new Vector2f(resultObject.Value.X, resultObject.Value.Y);

        Vector2f direction = targetCoordinate - unitCoordinate;

        float angleInRadians = (float)Math.Atan2(direction.Y, direction.X);
        float angleInDegrees = angleInRadians * (180f / (float)Math.PI);
        newUnit.Angle = angleInRadians;
    }
    private void UseEffect(IUnit? owner, Unit bulletUnit, MoveLib.Move.Result.CollisionResult collisionObject)
    {
        try
        {
            if (owner is null || bulletUnit.Map is null)
                throw new Exception("Owner or bulletUnit map is null");

            MoveAngle(bulletUnit, collisionObject.CollisionCoordinate);
            var raycastResult = Raycast.RaycastFun(bulletUnit);

            (IObject?, Vector3f?) result = raycastResult.Item1 is null || raycastResult.Item1 != collisionObject.CollisionObject ?
                (collisionObject.CollisionObject, collisionObject.CollisionCoordinate) :
                (raycastResult.Item1, raycastResult.Item2);
            OnHit(owner, bulletUnit, result.Item1, result.Item2 ?? new Vector3f());

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UseEffect: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
        finally
        {
            Destroy();
        }
    }
    private void PlaySound()
    {
        if (Unit.Map is null)
            return;

        if (tempFlySound is null || tempFlySound.Status == SoundStatus.Stopped)
            tempFlySound = SoundFly?.Play(Unit.Map, new Vector3f((float)Unit.X.Axis, (float)Unit.Z.Axis, (float)Unit.Y.Axis));
        else if (tempFlySound is not null || tempFlySound.Status == SoundStatus.Playing)
            tempFlySound.Position = new Vector3f((float)Unit.X.Axis, (float)Unit.Z.Axis, (float)Unit.Y.Axis);
    }

    private MoveLib.Move.Result.CollisionResult MoveBullet()
    {
        PlaySound();

        double deltaX = Unit.Direction.X * Speed;
        double deltaY = Unit.Direction.Y * Speed;

        double deltaZ = -Unit.VerticalAngle * 100;
        Unit.Z.Axis += deltaZ;

        return MoveLib.Move.Collision.TryMoveAndGetCollision(Unit, deltaX, deltaY, System.Linq.Enumerable.ToList(ignoreCollisionList.Keys));
    }

    public override void Update()
    {

        if (!IsActive || Unit.Map is null) return;

        Vector3f positionBullet = new Vector3f((float)Unit.X.Axis, (float)Unit.Y.Axis, (float)Unit.Z.Axis);
        float distance = DataPipes.MathUtils.CalculateDistance(positionBullet, PositionOwner);

        var collisionObject = MoveBullet();

        if (collisionObject.CollisionObject != null ||
            (distance > FlightDistance && FlightDistance != IBullet.InfinityFlightDistance))
        {
            UseEffect(Owner, Unit, collisionObject);
            return;
        }
    }
    private void Destroy()
    {

        if (!IsActive) return;
        IsActive = false;

        Unit.Map?.DeleteObstacle(Unit);
        SpriteObstacle.RemoveObstacle(Unit);

        ignoreCollisionList.TryRemove(Unit, out _);
        Owner?.IgnoreCollisionObjects.TryRemove(Unit, out _);

        tempFlySound?.Stop();
        tempFlySound = null;
    }

    public void InitializeUnit(IUnit owner)
    {
        owner.Map?.AddObstacle((int)owner.X.AxisMap, (int)owner.Y.AxisMap, Unit);
        Unit.X.Axis = owner.X.Axis;
        Unit.Y.Axis = owner.Y.Axis;
        Unit.Z.Axis = owner.Z.Axis;
        PositionOwner = new Vector3f((float)owner.X.Axis, (float)owner.Y.Axis, (float)owner.Z.Axis);

        Unit.Angle = owner.Angle;
        Unit.VerticalAngle = owner.VerticalAngle;

        Unit.IsPassability = true;
        Unit.HasGravity = false;
    }
    public override void Flight(IUnit owner)
    {
        if (owner.Map is null)
            return;

        UnitBullet newUnit = new UnitBullet(this);
        newUnit.Owner = owner;
        newUnit.SoundFly = SoundFly;
        newUnit.IsActive = true;

        ignoreCollisionList.TryAdd(newUnit.Unit, 0);
        ignoreCollisionList.TryAdd(owner, 0);
        owner.IgnoreCollisionObjects.TryAdd(newUnit.Unit, 0);

        newUnit.InitializeUnit(owner);
        BulletHandler.Add(newUnit);      
    }

    public override IBullet GetCopy()
    {
        return new UnitBullet(this);
    }
}