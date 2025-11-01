using BehaviorPatternsFramework.Behavior;
using BehaviorPatternsFramework.Enum;
using FpsLib;
using ProtoRender.Object;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BehaviorPatternsFramework.PatternMove;
public class CircularMovement : IAIBehavior
{
    public BehaviorStatus Status { get; private set; } = BehaviorStatus.Failure;
    public Func<AIContext, bool>? IsBlocked { get; set; }


    public int MovementDurationMs { get; set; } = 1000;


    public float Diametr { get; set; } = 1;

    private int _rotationDirection = 1;
    public int RotationDirection 
    {
        get => _rotationDirection;
        set
        {
            if (value < 0)
                _rotationDirection = -1;
            else if (value >= 1)
                _rotationDirection = 1;
        }
    }


    private double _elapsedMs = 0;
    //Vector2f offset = obj.Direction;
    //        Vector2f offset = new Vector2f(
    //    MathF.Cos((float)-obj.Angle), // <-- минус для против часовой <-- плюс для против часовой
    //    MathF.Sin((float)-obj.Angle)
    //) ;
    public void Update(AIContext context)
    {
        Status = BehaviorStatus.Running;
        var owner = context.Owner;

        if (owner is null || owner.Map is null || (IsBlocked?.Invoke(context) ?? false))
        {
            Status = BehaviorStatus.Failure;
            return;
        }

        float deltaTimeMs = FPS.GetDeltaTime();
        _elapsedMs += deltaTimeMs;

        float movementDurationSec = MovementDurationMs / 1000f;
        if (movementDurationSec <= 0f)
        {
            Status = BehaviorStatus.Failure;
            return;
        }

        owner.Angle = (owner.Angle + deltaTimeMs * (MathF.PI * 2 / movementDurationSec)) % (MathF.PI * 2);
        if (owner.Angle < 0)
            owner.Angle += MathF.PI * 2;

        Vector2f offset = new Vector2f(
            MathF.Cos((float)owner.Angle * RotationDirection),
            MathF.Sin((float)owner.Angle * RotationDirection)
        );


        Vector2f targetPos = owner.MoveSpeed / movementDurationSec * Diametr * deltaTimeMs * offset;

        var result = MoveLib.Move.Collision.TryMoveWithCollision(owner, targetPos.X, targetPos.Y, owner.IgnoreCollisionObjects.Keys.ToList());

        if (result.CollisionObject is not null)
            RotationDirection = -RotationDirection;
        if (_elapsedMs >= movementDurationSec)
        {
            Status = BehaviorStatus.Success;
            _elapsedMs = 0;
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
}

