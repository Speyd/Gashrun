using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using FpsLib;
using HitBoxLib.Operations;
using ProtoRender.Object;
using ProtoRender.Physics;
using SFML.System;
using SFML.Window;

namespace BehaviorPatternsFramework.Move;
public class PingPongMovement : IAIBehavior
{
    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }


    private int _movementDurationMs;
    public int MovementDurationMs
    {
        get => _movementDurationMs;
        set
        {
            if (value <= 0)
                return;

            _movementDurationMs = value;
        }
    }

    public int HalfMovementDurationMs => MovementDurationMs / 2;
    private double MovementCycleSeconds => MovementDurationMs / 1000f;
    private double HalfMovementCycleSeconds => HalfMovementDurationMs / 1000f;

    public bool IsReturnPhase { get; private set; } = false;
    public bool IsHalfTurnDone { get; private set; } = false;
    public bool IsCollisionTurnPending { get; private set; } = false;


    private double _elapsedTimeMs = 0;
    private double _totalElapsedTimeMs = 0;


  
    public void Update(AIContext context)
    {
        Status = BehaviorStatus.Running;
        var owner = context.Owner;

        if (owner is null || owner.Map is null || (IsBlocked?.Invoke(context) ?? false))
        {
            Status = BehaviorStatus.Failure;
            return;
        }

        double deltaTimeMs = FPS.GetDeltaTime();
        _elapsedTimeMs += deltaTimeMs;
        _totalElapsedTimeMs += deltaTimeMs;


        HandlePingPongTiming(context);

        float distance = owner.MoveSpeed * (float)deltaTimeMs;
        Vector2f velocity = distance * owner.LookDirection;
        Vector2f previousPosition = new Vector2f((float)owner.X.Axis, (float)owner.Y.Axis);

        var moveResult = MoveLib.Move.Collision.TryMoveWithCollision(
            owner,
            velocity.X,
            velocity.Y,
            owner.IgnoreCollisionObjects.Keys.ToList()
        );

        if (moveResult.CollisionObject is not null)
            HandleCollision(context, previousPosition);

        if (_totalElapsedTimeMs >= MovementCycleSeconds)
        {
            Status = BehaviorStatus.Success;
            _totalElapsedTimeMs = 0;
            _elapsedTimeMs = 0;
            IsReturnPhase = false;
            IsHalfTurnDone = false;
            IsCollisionTurnPending = false;
        }
    }

    private void HandlePingPongTiming(AIContext context)
    {
        if (_elapsedTimeMs >= HalfMovementCycleSeconds && !IsHalfTurnDone)
        {
            if (!IsCollisionTurnPending)
                context.Owner!.Angle += Math.PI;
            else
                IsCollisionTurnPending = false;

            IsHalfTurnDone = true;
        }

        if (_elapsedTimeMs >= MovementCycleSeconds)
        {
            _elapsedTimeMs = 0;
            context.Owner!.Angle -= Math.PI;
            IsHalfTurnDone = false;
            IsReturnPhase = !IsReturnPhase;
        }
    }

    private void HandleCollision(AIContext context, Vector2f previousPosition)
    {
        context.Owner!.X.Axis = previousPosition.X;
        context.Owner!.Y.Axis = previousPosition.Y;

        if (_elapsedTimeMs < HalfMovementCycleSeconds && IsHalfTurnDone)
        {
            _elapsedTimeMs += HalfMovementCycleSeconds;
            IsHalfTurnDone = false;
        }
        else
        {
            _elapsedTimeMs = MovementCycleSeconds - _elapsedTimeMs;
            context.Owner!.Angle += Math.PI;
            IsHalfTurnDone = false;
            IsCollisionTurnPending = true;
        }
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
        var tempPingPongMovement = new PingPongMovement();
        tempPingPongMovement.MovementDurationMs = MovementDurationMs;

        return tempPingPongMovement;
    }
}
