using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using BehaviorPatternsFramework.PatternMove;
using FpsLib;
using ProtoRender.Object;
using SFML.System;

namespace BehaviorPatternsFramework.PatternEmotion;
public class PersecutionBehavior : IAIBehavior
{
    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }

    public Vector2f? TargetPosition { get; private set; } = null;
    public int TargetArrivalDistance { get; set; } = 10;
    public float ApproachDistance { get; set; } = -1;


    public void Update(AIContext context)
    {
        Status = BehaviorStatus.Running;

        if (context.Owner is null || (IsBlocked?.Invoke(context) ?? false))
        {
            Status = BehaviorStatus.Failure;
            return;
        }
        else if (context.TargetObject is null && TargetPosition is null)
        {
            Status = BehaviorStatus.Failure;
            return;
        }

        if (!DetectTargetCoordinate(context, context.TargetObject))
            return;

        MoveOwner(context);
    }

    private bool DetectTargetCoordinate(AIContext context, IObject? target)
    {
        if (target is not null)
        {
            TargetPosition = new Vector2f((float)target.X.Axis, (float)target.Y.Axis);
        }

        if (TargetPosition is not null)
        {
            float distanceToLastSeen = DataPipes.MathUtils.CalculateDistance(context.Owner!.OriginPosition, TargetPosition.Value);

            if (distanceToLastSeen < TargetArrivalDistance)
            {
                TargetPosition = null;
                Status = BehaviorStatus.Failure;
                return false;
            }
            if (distanceToLastSeen < ApproachDistance && ApproachDistance > 0)
            {
                Status = BehaviorStatus.Failure;
                return false;
            }
        }

        Status = BehaviorStatus.Success;
        return true;
    }
    private void MoveOwner(AIContext context)
    {
        if (TargetPosition is null)
            return;


        context.Owner!.Angle = DataPipes.MathUtils.CalculateAngleToTarget(TargetPosition.Value, context.Owner!.OriginPosition);

        float deltaTimeMs = FPS.GetDeltaTime();
        float distance = context.Owner.MoveSpeed * (float)deltaTimeMs;
        Vector2f velocity = distance * context.Owner.LookDirection;

        MoveLib.Move.Collision.TryMoveWithCollision(
            context.Owner,
            velocity.X,
            velocity.Y,
            context.Owner.IgnoreCollisionObjects.Keys.ToList()
        );
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

    public IAIBehavior GetDeepCopy()
    {
        var tempPersecutionBehavior = new PersecutionBehavior();
        tempPersecutionBehavior.TargetPosition = TargetPosition;
        tempPersecutionBehavior.TargetArrivalDistance = TargetArrivalDistance;
        tempPersecutionBehavior.ApproachDistance = ApproachDistance;

        return tempPersecutionBehavior;
    }
}
