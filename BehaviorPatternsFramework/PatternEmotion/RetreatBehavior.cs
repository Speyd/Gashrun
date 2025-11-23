using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using FpsLib;
using SFML.System;

namespace BehaviorPatternsFramework.PatternEmotion;
public class RetreatBehavior : IAIBehavior
{
    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }

    public long MovementDurationMs { get; set; } = 0;

    private bool _hasTurnedBack = false;
    private double _elapsedMs = 0;

    public void Update(AIContext context)
    {
        if (context.Owner is null || context.Owner.Map is null || (IsBlocked?.Invoke(context) ?? false))
        {
            Status = BehaviorStatus.Failure;
            return;
        }

        double deltaTimeMs = FPS.GetDeltaTime();
        _elapsedMs += deltaTimeMs;

        if (!_hasTurnedBack)
        {
            context.Owner.Angle += Math.PI;
            _hasTurnedBack = true;
        }

        float distance = context.Owner.MoveSpeed * (float)deltaTimeMs;
        Vector2f velocity = distance * context.Owner.LookDirection;

        MoveLib.Move.Collision.TryMoveWithCollision(
            context.Owner,
            velocity.X,
            velocity.Y,
            context.Owner.IgnoreCollisionObjects.Keys.ToList()
        );

        if (_elapsedMs >= MovementDurationMs / 1000f)
        {
            Reset();
            Status = BehaviorStatus.Success;
        }
        else
            Status = BehaviorStatus.Running;
    }

    private void Reset()
    {
        _hasTurnedBack = false;
        _elapsedMs = 0;
    }


    public void Enter(AIContext context)
    {
        Reset();
    }

    public void Exit(AIContext context)
    {
        Reset();
    }

    public BehaviorStatus GetNextEvent(AIContext context)
    {
        return Status;
    }

    public IAIBehavior GetDeepCopy()
    {
        var tempRetreatBehavior = new RetreatBehavior();
        tempRetreatBehavior.MovementDurationMs = MovementDurationMs;

        return tempRetreatBehavior;
    }
}
