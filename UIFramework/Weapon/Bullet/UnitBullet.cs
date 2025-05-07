using AnimationLib;
using ControlLib;
using MapLib;
using ObjectFramework;
using ObstacleLib.SpriteLib;
using ProtoRender.Object;
using RayTracingLib;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UIFramework.Weapon.Patron;
using SFML.System;
using ScreenLib;
using FpsLib;
using System.IO;
using HitBoxLib.HitBoxSegment;
using HitBoxLib.PositionObject;
using HitBoxLib.Segment.SignsTypeSide;
using ProtoRender.RenderAlgorithm;
using static System.Net.Mime.MediaTypeNames;
using RayTracingLib.Detection;
using ObstacleLib.TexturedWallLib;
using System.Collections.Concurrent;
using ObstacleLib;
using HitBoxLib.Data.HitBoxObject;
using HitBoxLib.Data.Observer;
using MoveLib.Angle;

namespace UIFramework.Weapon.Bullet;
public class UnitBullet : IBullet
{
    public ControlLib.BottomBinding? HitDrawbleObject { get; set; } = null;
    public ControlLib.BottomBinding? HitObject { get; set; } = null;
    public HitEffect? HitEffect { get; set; } = null;
    public HitEffect? BulletEffect { get; set; } = null;

    public float Damage { get; set; } = 50;
    public Unit Unit { get; set; }
    private double Speed { get; set; } = 20;

    private IUnit? Owner;

    private ConcurrentDictionary<IUnit, byte> ignoreCollisionList = new();


    public UnitBullet(Unit unit, ControlLib.BottomBinding? hitDrawbleObject, ControlLib.BottomBinding? hitObject, HitEffect? hitEffect)
    {
        HitDrawbleObject = hitDrawbleObject;
        BulletEffect = new HitEffect(unit, new TimeSpan(-1));
        HitEffect = hitEffect;
        HitObject = hitObject;

        Unit = new Unit(unit);
    }
    public UnitBullet(UnitBullet bullet)
    {
        HitDrawbleObject = bullet.HitDrawbleObject;
        HitEffect = bullet.HitEffect;
        HitObject = bullet.HitObject;

        Unit = bullet.Unit;
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
    private void UseEffect(Unit newUnit, (IObject?, Vector3f?) collisionObject)
    {
        if (Owner is null)
        {
            newUnit.Map.DeleteObstacle(newUnit);
            ignoreCollisionList.TryRemove(newUnit, out _);
            return;
        }

        MoveAngle(newUnit, collisionObject.Item2);
        var reycastObject = Raycast.RaycastFun(newUnit.Map, newUnit);   
        if (reycastObject.Item1 is null)
        {
            newUnit.Map.DeleteObstacle(newUnit);

            ignoreCollisionList.TryRemove(newUnit, out _);
            Owner.IgnoreCollisionObjects.TryRemove(newUnit, out _);
            return;
        }


        HitEffect?.Create(Owner, reycastObject.Item2.X, reycastObject.Item2.Y, reycastObject.Item2.Z);
        if (reycastObject.Item1 is IDrawable drawable)
        {
            HitDrawbleObject?.Listen(drawable, newUnit);
        }
        if (reycastObject.Item1 is Unit unit)
        {
            unit.DamageAction?.Invoke(Damage);
            HitObject?.Listen();
        }
        newUnit.Map.DeleteObstacle(newUnit);
    }

    public async Task ProcessMovementAsync(Unit newUnit)
    {
        try
        {
            if (Owner is null)
                return;

            double deltaX = newUnit.Direction.X * newUnit.MoveSpeed;
            double deltaY = newUnit.Direction.Y * newUnit.MoveSpeed;

            int? idEffect = BulletEffect?.Create(Owner, newUnit);

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

                    UseEffect(newUnit, collisionObject);

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
    public void ProcessMovement(Unit newUnit, bool isFreezeTask = true)
    {
        try
        {
            if (Owner is null)
                return;

            double deltaX = newUnit.Direction.X * newUnit.MoveSpeed;
            double deltaY = newUnit.Direction.Y * newUnit.MoveSpeed;

            int? idEffect = BulletEffect?.Create(Owner, newUnit);

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

            UseEffect(newUnit, collisionObject);
            if (idEffect is not null)
                HitEffect.Remove(idEffect.Value);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in task ProcessMovement: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }

    public async Task FlightAsync(IUnit owner)
    {
        try
        {

            await Task.Run(() =>
            {
                Owner = owner;
                Unit newUnit = new Unit(Unit, false);

                ignoreCollisionList.TryAdd(newUnit, 0);
                ignoreCollisionList.TryAdd(owner, 0);
                Owner.IgnoreCollisionObjects.TryAdd(newUnit, 0);

                newUnit.Map.AddObstacle((int)owner.X.AxisMap, (int)owner.Y.AxisMap, newUnit);
                newUnit.X.Axis = owner.X.Axis;
                newUnit.Y.Axis = owner.Y.Axis;
                newUnit.Z.Axis = owner.Z.Axis;

                newUnit.Angle = owner.Angle;
                newUnit.VerticalAngle = owner.VerticalAngle;

                ProcessMovement(newUnit);
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in FlightAsync: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }
    public void Flight(IUnit owner)
    {
        try
        {
            Owner = owner;
            Unit newUnit = new Unit(Unit, false);

            ignoreCollisionList.TryAdd(newUnit, 0);
            ignoreCollisionList.TryAdd(owner, 0);
            Owner.IgnoreCollisionObjects.TryAdd(newUnit, 0);

            newUnit.Map.AddObstacle((int)owner.X.AxisMap, (int)owner.Y.AxisMap, newUnit);

            newUnit.X.Axis = owner.X.Axis;
            newUnit.Y.Axis = owner.Y.Axis;
            newUnit.Z.Axis = owner.Z.Axis;

            newUnit.Angle = owner.Angle;
            newUnit.VerticalAngle = owner.VerticalAngle;

            ProcessMovement(newUnit);
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in FlightAsync: " + ex.Message);
            Console.WriteLine(ex.StackTrace);
        }
    }


    public IBullet GetCopy()
    {
        return new UnitBullet(this);
    }
}