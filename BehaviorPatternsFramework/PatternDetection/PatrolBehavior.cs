using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using ObjectFramework;
using ProtoRender.Object;
using RayTracingLib;
using SFML.System;


namespace BehaviorPatternsFramework.PatternDetection;
public class PatrolBehavior : IAIBehavior
{
    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }

    public int UpdateIntervalMs { get; set; } = 50;
    public RaycastOptions RaycastOptions { get; set; }

    private static DateTime _lastUpdate = DateTime.Now;


    public PatrolBehavior()
    {
        RaycastOptions = new()
        {
            UseIgnoreList = true,
            RaycastMode = RaycastMode.AllHits,
            LimitType = RayLimitType.MaxRenderTiles,
        };
    }


    public void Update(AIContext context)
    {
        if (context.Owner is null || (IsBlocked?.Invoke(context) ?? false))
        {
            Status = BehaviorStatus.Failure;
            return;
        }
        if ((DateTime.Now - _lastUpdate).TotalMilliseconds < UpdateIntervalMs)
        {
            Status = BehaviorStatus.Running;
            return;
        }

        _lastUpdate = DateTime.Now;
        foreach (var item in GetObjectsInFOV(context.Owner, 8))
        {
            if (IsObjectInFovByHitbox(context.Owner, item) && item.UUID == Camera.CurrentUnit?.UUID)
            {
                context.TargetObject = item;
                return;
            }
        }

        context.TargetObject = null;
        Status = BehaviorStatus.Success;
    }

    public bool IsObjectInFovByHitbox(IUnit observer, IObject obj)
    {
        Vector2f posObs = new((float)observer.X.Axis, (float)observer.Y.Axis);

        var sides = Raycast.GetSideObject(obj);
        Vector2f[] corners =
        {
        new((float)obj.X.Axis, (float)obj.Y.Axis),
        new(sides.minX, sides.minY),
        new(sides.maxX, sides.minY),
        new(sides.maxX, sides.maxY),
        new(sides.minX, sides.maxY)
    };

        foreach (var corner in corners)
        {
            double angleToCorner = DataPipes.MathUtils.CalculateAngleToTarget(corner, posObs);
            double delta = DataPipes.MathUtils.NormalizeAngleDifference(observer.Angle, angleToCorner);
            if (Math.Abs(delta) <= observer.HalfHorizontalFov)
                return true;
        }

        return false;
    }

    List<IObject> GetObjectsInFOV(IUnit unit, int rayCount)
    {
        if (unit is null || unit.Map is null)
            return new();

        HashSet<IObject> visibleObjects = new();

        double startAngle = unit.Angle - unit.HorizontalFov / 2.0;
        double endAngle = unit.Angle + unit.HorizontalFov / 2.0;
        double step = (endAngle - startAngle) / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            double angle = startAngle + i * step;
            if (unit is null || unit.Map is null)
                return new();

            var hit = Raycast.RaycastAtAngle(unit, angle, RaycastOptions);

            if (hit != null)
                visibleObjects.Add(hit);
        }
        return visibleObjects.ToList();
    }

    public void Enter(AIContext context)
    {
        // Console.WriteLine("Entering Patrol");
    }

    public void Exit(AIContext context)
    {
        // Console.WriteLine("Exiting Patrol");
    }

    public BehaviorStatus GetNextEvent(AIContext context)
    {
        return Status;
    }
}