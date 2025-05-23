using System.Collections.Concurrent;
using ObjectFramework;
using ProtoRender.Object;
using SFML.System;
using ScreenLib;
using RayTracingLib;
using InteractionFramework.VisualImpact;
using InteractionFramework.VisualImpact.Data;


namespace UIFramework.Weapon.Bullets;
public class UnitBullet : Bullet
{
    public Unit Unit { get; set; }
    private float Speed { get; set; } = 20;

    private const float baseSpeed = 10;


    private ConcurrentDictionary<IUnit, byte> ignoreCollisionList = new();


    public UnitBullet(float damage, float speed, Unit unit, ControlLib.BottomBinding? hitObject)
        :base(damage, hitObject)
    {
        Unit = new Unit(unit);
        Speed = speed;
    }
    public UnitBullet(float damage, Unit unit, ControlLib.BottomBinding? hitObject)
       : this(damage, baseSpeed, unit, hitObject)
    {}
    public UnitBullet(Unit unit, ControlLib.BottomBinding? hitObject)
        : this(baseDamage, baseSpeed, unit, hitObject)
    {}
    public UnitBullet(UnitBullet bullet)
        :base(bullet)
    {
        Unit = bullet.Unit;

        Speed = bullet.Speed;
        ignoreCollisionList = bullet.ignoreCollisionList;
    }
    

    private void MoveAngle(Unit newUnit, Vector3f? resultObject)
    {
        if (resultObject is null)
            return;

        var unitCoordinate = new Vector2f((float)newUnit.X.Axis, (float)newUnit.Y.Axis);
        var targetCoordinate = new Vector2f((float)resultObject.Value.X, (float)resultObject.Value.Y);

        Vector2f direction = targetCoordinate - unitCoordinate;

        float angleInRadians = (float)Math.Atan2(direction.Y, direction.X);
        float angleInDegrees = angleInRadians * (180f / (float)Math.PI);
        newUnit.Angle = angleInRadians;
    }
    private void UseEffect(IUnit owner, Unit newUnit, (IObject?, Vector3f?) collisionObject)
    {
        try
        {
            if (newUnit.Map is null)
                throw new Exception("newUnit map is null");

            MoveAngle(newUnit, collisionObject.Item2);
            var raycastResult = Raycast.RaycastFun(newUnit.Map, newUnit);

            var result = raycastResult.Item1 is null || raycastResult.Item1 != collisionObject.Item1 ?
                collisionObject :
                raycastResult;
            OnHit(owner, newUnit, result.Item1, result.Item2 ?? new Vector3f());

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UseEffect: {ex.Message}");
            Console.WriteLine(ex.StackTrace);

        }
        finally
        {
            newUnit.Map?.DeleteObstacle(newUnit);
            ignoreCollisionList.TryRemove(newUnit, out _);
            owner?.IgnoreCollisionObjects.TryRemove(newUnit, out _);
        }
    }
    public async Task ProcessMovementAsync(IUnit owner, Unit newUnit)
    {
        try
        {
            if (newUnit.Map is null)
                return;

            double deltaX = newUnit.Direction.X * newUnit.MoveSpeed;
            double deltaY = newUnit.Direction.Y * newUnit.MoveSpeed;

            int? idEffect = BeyondRenderManager.Create(owner, new VisualImpactData(newUnit, -1, false));
            await Task.Run(() =>
            {
                try
                {
                    (IObject?, Vector3f?) collisionObject;
                    while ((collisionObject = MoveLib.Move.Collision.GetCollisionObject(newUnit.Map, newUnit, deltaX, deltaY, ignoreCollisionList.Keys.ToList())).Item1 is null)
                    {
                        double deltaZ = newUnit.MoveSpeed * -(newUnit.VerticalAngle < 0
                            ? newUnit.VerticalAngle * (2.25 * Screen.ScreenRatio)
                            : newUnit.VerticalAngle * (1.8 * Screen.ScreenRatio));

                        newUnit.Z.Axis += deltaZ;

                        Thread.Sleep(30);
                    }

                    UseEffect(owner, newUnit, collisionObject);  
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error in task ProcessMovementAsync: " + ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
                finally
                {
                    if (idEffect is not null)
                        BeyondRenderManager.Remove(idEffect.Value);
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

            int? idEffect = BeyondRenderManager.Create(owner, new VisualImpactData(newUnit, -1, false));
            try
            {
                (IObject?, Vector3f?) collisionObject;
                while ((collisionObject = MoveLib.Move.Collision.GetCollisionObject(newUnit.Map, newUnit, deltaX, deltaY, ignoreCollisionList.Keys.ToList())).Item1 is null)
                {
                    double deltaZ = newUnit.MoveSpeed * -(newUnit.VerticalAngle < 0
                                 ? newUnit.VerticalAngle * (2.25 * Screen.ScreenRatio)
                                 : newUnit.VerticalAngle * (1.8 * Screen.ScreenRatio));

                    newUnit.Z.Axis += deltaZ;

                    if (isFreezeTask)
                        Thread.Sleep(30);
                }

                UseEffect(owner, newUnit, collisionObject);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in cycle while ProcessMovement: " + ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine($"Inner stack ProcessMovement: {ex.InnerException?.StackTrace}");
            }
            finally
            {
                if (idEffect is not null)
                    BeyondRenderManager.Remove(idEffect.Value);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in ProcessMovement: " + ex.Message);
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
            Console.WriteLine($"Inner stack: {ex.InnerException?.StackTrace}");
            Console.WriteLine($"Inner stack FLIGHTASYNC: {ex.StackTrace}");
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