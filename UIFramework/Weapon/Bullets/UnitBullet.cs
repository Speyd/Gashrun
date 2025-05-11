using System.Collections.Concurrent;
using ObjectFramework;
using ProtoRender.Object;
using SFML.System;
using ScreenLib;
using RayTracingLib;
using HitBoxLib.HitBoxSegment;
using HitBoxLib.PositionObject;
using HitBoxLib.Segment.SignsTypeSide;
using ProtoRender.Map;
using DataPipes;
using HitBoxLib.Data.HitBoxObject;
using HitBoxLib.Data.Observer;
using System.Threading.Tasks.Dataflow;

namespace UIFramework.Weapon.Bullets;
public class UnitBullet : Bullet
{
    public HitEffect? BulletEffect { get; set; } = null;
    public Unit Unit { get; set; }
    private float Speed { get; set; } = 20;

    private const float baseSpeed = 10;


    private ConcurrentDictionary<IUnit, byte> ignoreCollisionList = new();


    public UnitBullet(float damage, float speed, Unit unit, ControlLib.BottomBinding? hitDrawbleObject, ControlLib.BottomBinding? hitObject, HitEffect? hitEffect)
        :base(damage, hitDrawbleObject, hitObject, hitEffect)
    {
        BulletEffect = new HitEffect(unit, -1);
        Unit = new Unit(unit);

        Speed = speed;
    }
    public UnitBullet(float damage, Unit unit, ControlLib.BottomBinding? hitDrawbleObject, ControlLib.BottomBinding? hitObject, HitEffect? hitEffect)
       : this(damage, baseSpeed, unit, hitDrawbleObject, hitObject, hitEffect)
    {}
    public UnitBullet(Unit unit, ControlLib.BottomBinding? hitDrawbleObject, ControlLib.BottomBinding? hitObject, HitEffect? hitEffect)
        : this(baseDamage, baseSpeed, unit, hitDrawbleObject, hitObject, hitEffect)
    {}
    public UnitBullet(UnitBullet bullet)
        :base(bullet)
    {
        BulletEffect = bullet.BulletEffect;
        Unit = bullet.Unit;

        Speed = bullet.Speed;
        ignoreCollisionList = bullet.ignoreCollisionList;
    }
    

    private void MoveAngle(Unit newUnit, Vector3f? resultObject)
    {
        var unitCoordinate = new Vector2f((float)newUnit.X.Axis, (float)newUnit.Y.Axis);
        var targetCoordinate = new Vector2f((float)resultObject.Value.X, (float)resultObject.Value.Y);

        Vector2f direction = targetCoordinate - unitCoordinate;

        float angleInRadians = (float)Math.Atan2(direction.Y, direction.X);
        float angleInDegrees = angleInRadians * (180f / (float)Math.PI);
        newUnit.Angle = angleInRadians;
    }
    private bool ValidateMapAndCleanupIfNull(IUnit owner, Unit unit)
    {
        if (unit.Map is null)
        {
            ignoreCollisionList.TryRemove(unit, out _);
            owner?.IgnoreCollisionObjects.TryRemove(unit, out _);
            return false;
        }
        return true;
    }


    private void UseEffect(IUnit owner, Unit newUnit, (IObject?, Vector3f?) collisionObject)
    {
        if (!ValidateMapAndCleanupIfNull(owner, newUnit))
            return;

        MoveAngle(newUnit, collisionObject.Item2);
        var raycastResult = Raycast.RaycastFun(newUnit.Map, newUnit);
        Console.WriteLine(raycastResult.Item2.ToString());
        var result = raycastResult.Item1 is null || raycastResult.Item1 != collisionObject.Item1? collisionObject: raycastResult;
        OnHit(owner, newUnit, result.Item1, result.Item2 ?? new Vector3f());
        newUnit.Map.DeleteObstacle(newUnit);
    }
    public async Task ProcessMovementAsync(IUnit owner, Unit newUnit)
    {
        try
        {
            if (newUnit.Map is null)
                return;

            double deltaX = newUnit.Direction.X * newUnit.MoveSpeed;
            double deltaY = newUnit.Direction.Y * newUnit.MoveSpeed;

            int? idEffect = BulletEffect?.Create(owner, newUnit);

            await Task.Run(() =>
            {
                try
                {
                    (IObject?, Vector3f?) collisionObject;
                    while ((collisionObject = MoveLib.Move.Collision.GetCollisionDetails(newUnit.Map, newUnit, deltaX, deltaY, ignoreCollisionList.Keys.ToList())).Item1 is null)
                    {
                        double deltaZ = newUnit.MoveSpeed * -(newUnit.VerticalAngle < 0
                            ? newUnit.VerticalAngle * (2.25 * Screen.ScreenRatio)
                            : newUnit.VerticalAngle * (1.8 * Screen.ScreenRatio));

                        newUnit.Z.Axis += deltaZ;

                        Thread.Sleep(30);
                    }

                    UseEffect(owner, newUnit, collisionObject);

                    if (idEffect is not null)
                        HitEffect.Remove(idEffect.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in task ProcessMovementAsync: " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error out task  ProcessMovementAsync: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }
    public void ProcessMovement(IUnit owner, Unit newUnit, bool isFreezeTask = true)
    {
        try
        {
            if (newUnit.Map is null)
                return;

            double deltaX = newUnit.Direction.X * newUnit.MoveSpeed;
            double deltaY = newUnit.Direction.Y * newUnit.MoveSpeed;

            int? idEffect = BulletEffect?.Create(owner, newUnit);

            (IObject?, Vector3f?) collisionObject;
            while ((collisionObject = MoveLib.Move.Collision.GetCollisionDetails(newUnit.Map, newUnit, deltaX, deltaY, ignoreCollisionList.Keys.ToList())).Item1 is null)
            {
                double deltaZ = newUnit.MoveSpeed * -(newUnit.VerticalAngle < 0
                             ? newUnit.VerticalAngle * (2.25 * Screen.ScreenRatio)
                             : newUnit.VerticalAngle * (1.8 * Screen.ScreenRatio));

                newUnit.Z.Axis += deltaZ;

                if(isFreezeTask)
                    Thread.Sleep(30);
            }

            UseEffect(owner, newUnit, collisionObject);
            if (idEffect is not null)
                HitEffect.Remove(idEffect.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in task ProcessMovement: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }

    public override async Task FlightAsync(IUnit owner)
    {
        try
        {
            if (owner.Map is null)
                return;

            await Task.Run(() =>
            {
                Unit newUnit = new Unit(Unit, false);

                ignoreCollisionList.TryAdd(newUnit, 0);
                ignoreCollisionList.TryAdd(owner, 0);
                owner.IgnoreCollisionObjects.TryAdd(newUnit, 0);

                newUnit.Map?.AddObstacle((int)owner.X.AxisMap, (int)owner.Y.AxisMap, newUnit);
                newUnit.X.Axis = owner.X.Axis;
                newUnit.Y.Axis = owner.Y.Axis;
                newUnit.Z.Axis = owner.Z.Axis;

                newUnit.Angle = owner.Angle;
                newUnit.VerticalAngle = owner.VerticalAngle;

                ProcessMovement(owner, newUnit);
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in FlightAsync: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }
    public override void Flight(IUnit owner)
    {
        try
        {
            if (owner.Map is null)
                return;

            Unit newUnit = new Unit(Unit, false);

            ignoreCollisionList.TryAdd(newUnit, 0);
            ignoreCollisionList.TryAdd(owner, 0);
            owner.IgnoreCollisionObjects.TryAdd(newUnit, 0);

            newUnit.Map?.AddObstacle((int)owner.X.AxisMap, (int)owner.Y.AxisMap, newUnit);

            newUnit.X.Axis = owner.X.Axis;
            newUnit.Y.Axis = owner.Y.Axis;
            newUnit.Z.Axis = owner.Z.Axis;

            newUnit.Angle = owner.Angle;
            newUnit.VerticalAngle = owner.VerticalAngle;

            ProcessMovement(owner, newUnit);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in FlightAsync: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }


    public override IBullet GetCopy()
    {
        return new UnitBullet(this);
    }
}