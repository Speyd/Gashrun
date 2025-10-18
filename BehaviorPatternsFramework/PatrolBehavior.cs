using BehaviorPatternsFramework.Enum;
using ObjectFramework;
using ProtoRender.Object;
using RayTracingLib;
using SFML.System;
using SFML.Window;

namespace BehaviorPatternsFramework;
public class PatrolBehavior : IAIBehavior
{
    public static GameEventType SuccessfulUpdate { get; } = GameEventType.PlayerSeen;
    public static GameEventType ErrorUpdate { get; } = GameEventType.PlayerLost;


    public AIBehaviorType AIBehaviorType { get; } = AIBehaviorType.Vision;
    public GameEventType CurrentEventType { get; private set; } = GameEventType.PlayerLost;


    private static DateTime _lastUpdate = DateTime.Now;


    public void Update(AIContext aIContext)
    {
        if ((DateTime.Now - _lastUpdate).TotalMilliseconds < 50)
        {
            SetError(aIContext);
            return;
        }

        _lastUpdate = DateTime.Now;
        if (aIContext.Owner is null)
        {
            SetError(aIContext);
            return;
        }
   
        foreach (var item in GetObjectsInFOV(aIContext.Owner, 8))
        {
            if (IsObjectInFovByHitbox(aIContext.Owner, item) && item.UUID == Camera.CurrentUnit?.UUID)
            {
                aIContext.TargetObject = item;
                SetSuccess(aIContext);
                return;
            }
        }

        aIContext.TargetObject = null;
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
            if (Math.Abs(delta) <= observer.HalfFov)
                return true;
        }

        return false;
    }

    List<IObject> GetObjectsInFOV(IUnit unit, int rayCount)
    {
        HashSet<IObject> visibleObjects = new();

        double startAngle = unit.Angle - unit.Fov / 2.0;
        double endAngle = unit.Angle + unit.Fov / 2.0;
        double step = (endAngle - startAngle) / (rayCount - 1);

        for (int i = 0; i < rayCount; i++)
        {
            double angle = startAngle + i * step;

            var hit = Raycast.RaycastAtAngle(unit, angle, true, RayLimitType.MaxRenderTiles);
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

    public GameEventType GetNextEvent(AIContext context)
    {
        return CurrentEventType;
    }

    private void SetSuccess(AIContext context)
    {
        context.EventTypes[AIBehaviorType] = SuccessfulUpdate;
        CurrentEventType = SuccessfulUpdate;
    }
    private void SetError(AIContext context)
    {
        context.EventTypes[AIBehaviorType] = ErrorUpdate;
        CurrentEventType = ErrorUpdate;
    }
}