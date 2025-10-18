using BehaviorPatternsFramework.Enum;
using FpsLib;
using ProtoRender.Object;
using SFML.System;

namespace BehaviorPatternsFramework;
public class PersecutionBehavior : IAIBehavior
{
    public static GameEventType SuccessfulUpdate { get; } = GameEventType.PersecutionCompleted;
    public static GameEventType ErrorUpdate { get; } = GameEventType.PersecutionError;


    public AIBehaviorType AIBehaviorType { get; } = AIBehaviorType.Movement;
    public GameEventType CurrentEventType { get; private set; } = GameEventType.PersecutionError;

    public Vector2f? TargetPosition { get; private set; } = null;
    public int TargetArrivalDistance { get; set; } = 10;



    public void Update(AIContext context)
    {
        if (context.EventTypes.TryGetValue(AIBehaviorType.Vision, out var value) && value != GameEventType.PlayerLost || context.Owner is null)
        {
            SetError(context);
            return;
        }

        var target = context.TargetObject;
        if (target is null && TargetPosition is null)
        {
            SetError(context);
            return;
        }

        DetectTargetCoordinate(context, target);
        MoveOwner(context);

    }

    private void DetectTargetCoordinate(AIContext context, IObject? target)
    {
        if (target is not null)
        {
            TargetPosition = new Vector2f((float)target.X.Axis, (float)target.Y.Axis);
        }
        else if (TargetPosition is not null)
        {
            float distanceToLastSeen = DataPipes.MathUtils.CalculateDistance(context.Owner!.OriginPosition, TargetPosition.Value);
            if (distanceToLastSeen < TargetArrivalDistance)
            {
                TargetPosition = null;
                SetError(context);
                return;
            }
        }

        SetSuccess(context);
    }
    private void MoveOwner(AIContext context)
    {
        if (TargetPosition is null)
            return;

        context.Owner!.Angle = DataPipes.MathUtils.CalculateAngleToTarget(TargetPosition.Value, context.Owner!.OriginPosition);

        float deltaTimeMs = FPS.GetDeltaTime();
        float distance = context.Owner!.MoveSpeed * deltaTimeMs;
        var velocity = distance * context.Owner.Direction;

        MoveLib.Move.Collision.TryMoveWithCollision(
            context.Owner,
            velocity.X,
            velocity.Y,
            context.Owner.IgnoreCollisionObjects.Keys.ToList()
        );

        context.TargetObject = null;
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
