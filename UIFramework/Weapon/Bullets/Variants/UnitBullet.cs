using System.Collections.Concurrent;
using ControlLib.Buttons;
using ProtoRender.Object;
using SFML.System;
using MoveLib;
using ScreenLib;
using RayTracingLib;
using InteractionFramework.VisualImpact;
using InteractionFramework.VisualImpact.Data;


namespace UIFramework.Weapon.Bullets.Variants;
public class UnitBullet : Bullet
{
    public Unit Unit { get; set; }
    private float Speed { get; set; }
    private const float baseSpeed = 10;

    public int? IdEffect { get; private set; }

    private ConcurrentDictionary<IUnit, byte> ignoreCollisionList = new();


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
    private void UseEffect(IUnit? owner, Unit bulletUnit, (IObject?, Vector3f?) collisionObject)
    {
        try
        {
            if (owner is null || bulletUnit.Map is null)
                throw new Exception("newUnit map is null");

            MoveAngle(bulletUnit, collisionObject.Item2);
            var raycastResult = Raycast.RaycastFun(bulletUnit);

            (IObject?, Vector3f?) result = raycastResult.Item1 is null || raycastResult.Item1 != collisionObject.Item1 ?
                collisionObject :
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

    private (IObject? Obj, Vector3f? Coordinate) MoveBullet()
    {
        double deltaX = Unit.Direction.X * Unit.MoveSpeed;
        double deltaY = Unit.Direction.Y * Unit.MoveSpeed;

        double deltaZ = Unit.MoveSpeed * -(Unit.VerticalAngle < 0
         ? Unit.VerticalAngle * (2.25 * Screen.ScreenRatio)
         : Unit.VerticalAngle * (1.8 * Screen.ScreenRatio));

        Unit.Z.Axis += deltaZ;

       return MoveLib.Move.Collision.GetCollisionObject(Unit, deltaX, deltaY, ignoreCollisionList.Keys.ToList());
    }

    public override void Update()
    {
        if (!IsActive || Unit.Map is null) return;

        Vector3f positionBullet = new Vector3f((float)Unit.X.Axis, (float)Unit.Y.Axis, (float)Unit.Z.Axis);
        float distance = DataPipes.MathUtils.CalculateDistance(positionBullet, PositionOwner);

        var collisionObject = MoveBullet();
        if (collisionObject.Item1 != null ||
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

        if (IdEffect is not null)
        {
            BeyondRenderManager.Remove(IdEffect.Value);
            IdEffect = null;
        }
        Unit.Map?.DeleteObstacle(Unit);

        ignoreCollisionList.TryRemove(Unit, out _);
        Owner?.IgnoreCollisionObjects.TryRemove(Unit, out _);
        IsActive = false;
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

        IdEffect = BeyondRenderManager.Create(owner, new VisualImpactData(Unit, -1, false));
    }
    public override void Flight(IUnit owner)
    {
        if (owner.Map is null)
            return;

        UnitBullet newUnit = new UnitBullet(this);
        newUnit.Owner = owner;
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